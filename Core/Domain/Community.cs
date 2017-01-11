// ------------------------------------------
// Community.cs, PS.CommunityGrapher
//
// Created by Pedro Sequeira, 2015/11/04
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommunityGrapher.Domain
{
	/// <summary>
	/// Implements the "Louvain method" for finding communities in large networks as described in 
	/// <a href="https://sites.google.com/site/findcommunities/"/>.
	/// </summary>
	public class Community : IDisposable
	{
		private static Random Random = new Random();
		#region Private Fields

		/// <summary>
		///     A new pass is computed if the last one has generated an increase
		///     greater than min_modularity.
		///     if 0. even a minor increase is enough to go for one more pass.
		/// </summary>
		private readonly double _minModularity;

		/// <summary>
		///     Number of pass for one level computation
		///     if -1, compute as many pass as needed to increase modularity.
		/// </summary>
		private readonly int _nbPass;

		/// <summary>
		///     Used to compute the modularity participation of each community.
		/// </summary>
		private double[] _inm;

		private uint[] _neighComm;
		private double[] _neighCommWeight;
		private uint _numNeighComm;

		/// <summary>
		///     used to compute the modularity participation of each community
		/// </summary>
		private double[] _tot;

		#endregion Private Fields

		#region Public Constructors

		/// <summary>
		///     Reads graph from file using graph constructor.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="popSize"></param>
		/// <param name="nbPass"></param>
		/// <param name="minModularity"></param>
		public Community(string filename, uint popSize, int nbPass, double minModularity)
			: this(NetworkGraph.LoadFile(filename), popSize, nbPass, minModularity)
		{
		}

		/// <summary>
		///     Copy graph.
		/// </summary>
		/// <param name="graph"></param>
		/// <param name="popSize"></param>
		/// <param name="nbPass"></param>
		/// <param name="minModularity"></param>
		public Community(NetworkGraph graph, uint popSize, int nbPass, double minModularity)
		{
			this.Graph = graph;
			this.Size = popSize;
			this._nbPass = nbPass;
			this._minModularity = minModularity;

			this.Init();
		}

		#endregion Public Constructors

		#region Public Properties

		public HashSet<uint>[] Communities { get; private set; }

		/// <summary>
		///     Network to compute communities for.
		/// </summary>
		public NetworkGraph Graph { get; }

		/// <summary>
		///     Community to which each node belongs.
		/// </summary>
		public uint[] NodeCommunity { get; private set; }

		/// <summary>
		///     Number of nodes in the network and size of all vectors.
		/// </summary>
		public uint Size { get; }

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		///     Display the community of each node
		/// </summary>
		public void Display()
		{
			for (var i = 0u; i < this.Size; i++)
				Console.WriteLine("Node: {0}, Comm: {1}, In: {2}, Tot: {3}", i, this.NodeCommunity[i], this._inm[i],
					this._tot[i]);
		}

		/// <summary>
		///     Display the nodes of each community
		/// </summary>
		public void DisplayCommunities()
		{
			for (var i = 0u; i < this.Size; i++)
			{
				var nodes = this.Communities[i];
				if (nodes.Count == 0) continue;
				Console.WriteLine("Community: {0}, Nodes: {1}", i, nodes.ToArray().ToVectorString());
			}
		}

		/// <summary>
		///     Displays the current partition (with communities renumbered from 0 to k-1).
		/// </summary>
		public void DisplayPartition()
		{
			uint final;
			var renumber = this.RenumberCommunities(out final);

			for (var i = 0u; i < this.Size; i++)
				Console.WriteLine("{0} {1}", i, renumber[this.NodeCommunity[i]]);
		}

		public void Dispose()
		{
			this.Graph.Clear();
			this.NodeCommunity = null;
			this.Communities = null;
			this._neighCommWeight = null;
			this._neighComm = null;
			this._inm = null;
			this._tot = null;
		}

		/// <summary>
		///     Compute the modularity of the current partition.
		/// </summary>
		/// <returns></returns>
		public double GetModularity()
		{
			var q = 0d;
			var m2 = this.Graph.TotalWeight;

			for (var i = 0; i < this.Size; i++)
				if (this._tot[i] > 0)
					q += (this._inm[i] / m2) - ((this._tot[i] / m2) * (this._tot[i] / m2));

			return q;
		}

		//
		/// <summary>
		///     Initializes the partition with something else than all nodes alone.
		/// </summary>
		/// <param name="filenamePart"></param>
		public void InitPartition(string filenamePart)
		{
			var finput = new StreamReader(filenamePart);

			// read partitions
			string line;
			while ((line = finput.ReadLine()) != null)
			{
				uint node;
				uint comm;
				var elems = line.Split(' ');
				if (!uint.TryParse(elems[0], out node)) continue;
				if (!uint.TryParse(elems[1], out comm)) continue;

				var oldComm = this.NodeCommunity[node];
				this.ComputeNeighourCommWeights(node);

				this.Remove(node, oldComm, this._neighCommWeight[oldComm]);

				var i = 0u;
				for (; i < this._numNeighComm; i++)
				{
					var neighPos = this._neighComm[i];
					var bestComm = neighPos;
					var bestNblinks = this._neighCommWeight[neighPos];
					if (bestComm != comm) continue;

					this.Insert(node, bestComm, bestNblinks);
					break;
				}

				if (i.Equals(this._numNeighComm))
					this.Insert(node, comm, 0);
			}

			finput.Close();
			finput.Dispose();
		}

		/// <summary>
		///     Compute communities of the graph for one level.
		/// </summary>
		/// <returns>true if some nodes have been moved.</returns>
		public bool OneLevel()
		{
			this.Reset();

			var improvement = false;
			int nbMoves;
			var newMod = this.GetModularity();
			double curMod;
			var nbPassDone = 0;
			//var randomOrder = GetRandomOrder(this.Size);

			// repeat while
			//   there is an improvement of modularity
			//   or there is an improvement of modularity greater than a given epsilon
			//   or a predefined number of pass have been done
			do
			{
				curMod = newMod;
				nbMoves = 0;
				nbPassDone++;

				// for each node: remove the node from its community and insert it in the best community
				// nodes are analysed in descending order to ensure higher numbered nodes go to lowest-number communities
				//for (var nodeTmp = 0; nodeTmp < this.Size; nodeTmp++)
				//for (var node = 0u; node < this.Size; node++)
				for (var node = this.Size - 1; node < this.Size && node >= 0; node--)
				{
					//var node = randomOrder[nodeTmp];
					var nodeComm = this.NodeCommunity[node];
					var nodeWeight = this.Graph.Weights[node];

					// computation of all neighboring communities of current node
					this.ComputeNeighourCommWeights(node);

					// remove node from its current community
					this.Remove(node, nodeComm, this._neighCommWeight[nodeComm]);

					// compute the nearest community for node
					// default choice for future insertion is the former community
					var bestNeighComm = nodeComm;
					var bestNeighCommWeight = double.MinValue;
					var bestModGain = double.MinValue;
					for (var i = 0u; i < this._numNeighComm; i++)
					{
						var neighComm = this._neighComm[i];
						var neighCommWeight = this._neighCommWeight[neighComm];
						var modGain = this.ModularityGain(neighComm, neighCommWeight, nodeWeight);
						if (modGain <= bestModGain) continue;

						bestNeighComm = neighComm;
						bestNeighCommWeight = neighCommWeight;
						bestModGain = modGain;
					}

					// insert node in the best, "nearest" community
					this.Insert(node, bestNeighComm, bestNeighCommWeight);

					if (bestNeighComm != nodeComm)
						nbMoves++;
				}

				//gets new modularity and checks improvement
				newMod = this.GetModularity();
				improvement |= nbMoves > 0;
			} while ((nbMoves > 0) && ((newMod - curMod) > this._minModularity) &&
					 (this._nbPass.Equals(-1) || (nbPassDone <= this._nbPass)));

			return improvement;
		}

		/// <summary>
		///     Displays the graph of communities as computed by one_level.
		/// </summary>
		public void Partition2Graph()
		{
			uint final;
			var renumber = this.RenumberCommunities(out final);

			for (var i = 0u; i < this.Size; i++)
			{
				var neighbours = this.Graph.AdjacentEdges(i);
				foreach (var edge in neighbours)
				{
					var neigh = edge.Source.Equals(i) ? edge.Target : edge.Source;
					Console.WriteLine("{0} {1}", renumber[this.NodeCommunity[i]], renumber[this.NodeCommunity[neigh]]);
				}
			}
		}

		/// <summary>
		///     Generates the binary graph of communities as computed by one_level.
		/// </summary>
		/// <returns></returns>
		public NetworkGraph Partition2GraphBinary()
		{
			uint final;
			var renumber = this.RenumberCommunities(out final);

			// Compute communities
			var commNodes = new List<uint>[(int)final];
			for (var node = 0u; node < Size; node++)
			{
				var comm = renumber[this.NodeCommunity[node]];
				if (commNodes[comm] == null)
					commNodes[comm] = new List<uint>();

				commNodes[comm].Add(node);
			}

			// Compute weighted graph
			var graphBinary = new NetworkGraph();
			for (var comm = 0u; comm < commNodes.Length; comm++)
			{
				var m = new Dictionary<uint, double>();
				foreach (var node in commNodes[comm])
				{
					var neighbours = this.Graph.AdjacentEdges(node);
					foreach (var edge in neighbours)
					{
						var neigh = edge.Source.Equals(node) ? edge.Target : edge.Source;
						var neighComm = renumber[this.NodeCommunity[neigh]];
						var neighWeight = edge.Weight;

						if (!m.ContainsKey(neighComm))
							m[neighComm] = neighWeight;
						else
							m[neighComm] += neighWeight;
					}
				}

				//add community vertex
				graphBinary.AddVertex(comm);

				//add community edges
				foreach (var commEdge in m)
					graphBinary.AddEdge(new NetworkEdge(comm, commEdge.Key, commEdge.Value));
			}

			return graphBinary;
		}

		/// <summary>
		///     Renumber communities according to their size (bigger first) and by changing
		///     their ID to the lowest one possible. Tries to keep communities IDs if possible
		///     to avoid renumbering.
		/// </summary>
		public void RenumberCommunities()
		{
			//collect stats about all communities
			var oldCommStats = new Dictionary<uint, uint>((int)this.Size);
			for (var node = 0u; node < this.Size; node++)
			{
				var comm = this.NodeCommunity[node];
				if (!oldCommStats.ContainsKey(comm))
					oldCommStats[comm] = 1;
				else
					oldCommStats[comm]++;
			}

			//creates dictionary with old -> new community ID and
			// put communities with ID within range -- these remain unchanged
			var newCommCount = oldCommStats.Count;
			var oldToNewComms = new Dictionary<uint, uint>((int)this.Size);
			/*var c = 0u;
			foreach (var oldComm in oldCommStats.Keys)
				oldToNewComms [oldComm] = c++;*/

			for (var comm = 0u; comm < newCommCount; comm++)
				if (oldCommStats.ContainsKey(comm))
				{
					oldToNewComms[comm] = comm;
					oldCommStats.Remove(comm);
				}

			// sorts remaining communities by size and
			// 	inserts each of them in a vague community ID
			var sortedOldComms = oldCommStats.Keys.ToList();
			sortedOldComms.Sort((x, y) => oldCommStats[y].CompareTo(oldCommStats[x]));
			var oldCommIdx = 0;
			for (var newComm = 0u; (oldCommIdx < sortedOldComms.Count) && (newComm < newCommCount); newComm++)
				if (!oldToNewComms.ContainsKey(newComm))
					oldToNewComms[sortedOldComms[oldCommIdx++]] = newComm;

			//replaces community for all nodes
			for (var node = 0u; node < this.Size; node++)
			{
				var oldComm = this.NodeCommunity[node];
				this.Remove(node, oldComm, this._neighCommWeight[oldComm]);

				var newComm = oldToNewComms[oldComm];
				this.Insert(node, newComm, this._neighCommWeight[newComm]);
			}
		}

		#endregion Public Methods

		#region Private Methods

		private static List<uint> GetRandomOrder(uint numElems)
		{
			var list = new List<uint>((int)numElems);
			for (var i = 0; i < numElems; i++)
				list.Add((uint)i);

			var randList = new List<uint>((int)numElems);
			while (list.Count > 0)
			{
				var index = Random.Next(list.Count);
				randList.Add(list[index]);
				list.RemoveAt(index);
			}

			return randList;
		}

		/// <summary>
		///     Compute the set of neighboring communities of node
		///     for each community, gives the number of links from node to comm.
		/// </summary>
		/// <param name="node"></param>
		private void ComputeNeighourCommWeights(uint node)
		{
			for (var i = 0u; i < this._numNeighComm; i++)
				this._neighCommWeight[this._neighComm[i]] = -1;

			//adds node community itself
			this._neighComm[0] = this.NodeCommunity[node];
			this._neighCommWeight[this._neighComm[0]] = 0;
			this._numNeighComm = 1;

			//adds all neighbour's communities
			var neighbours = this.GetSortedNeighbours(node);
			foreach (var edge in neighbours)
			{
				var neigh = edge.Key;
				var neighComm = this.NodeCommunity[neigh];
				var neighWeight = edge.Value;

				if (neigh.Equals(node)) continue;

				//adds neighbour weight to neighbour's community weight
				if (this._neighCommWeight[neighComm].Equals(-1))
				{
					this._neighCommWeight[neighComm] = 0.0;
					this._neighComm[this._numNeighComm++] = neighComm;
				}
				this._neighCommWeight[neighComm] += neighWeight;
			}

			//adds also first empty community, if any, ensures partitioning of communities
			for (var i = 0u; i < this.Size; i++)
				if ((i != this._neighComm[0]) && this.Communities[i].Count == 0)
				{
					this._neighCommWeight[i] = 0.0;
					this._neighComm[this._numNeighComm++] = i;
					break;
				}
		}

		private double GetSelfLoops(uint v)
		{
			if ((this.Graph == null) || this.Graph.IsAdjacentEdgesEmpty(v)) return 0d;

			var neighbours = this.Graph.AdjacentEdges(v);
			foreach (var edge in neighbours)
			{
				var neigh = edge.Source.Equals(v) ? edge.Target : edge.Source;
				if (neigh.Equals(v)) return edge.Weight;
			}
			return 0;
		}

		private SortedList<uint, double> GetSortedNeighbours(uint node)
		{
			// gets neighbors sorted by id in ascending order
			// this ensures analysis of low-numbered neighbors first
			var sortedList = new SortedList<uint, double>();
			var neighbourEdges = this.Graph.AdjacentEdges(node);
			foreach (var edge in neighbourEdges)
			{
				var neigh = edge.Source.Equals(node) ? edge.Target : edge.Source;
				sortedList.Add(neigh, edge.Weight);
			}

			return sortedList;
		}

		private void Init()
		{
			this._neighCommWeight = new double[this.Size];
			this._neighComm = new uint[this.Size];
			this._numNeighComm = 0;

			this.NodeCommunity = new uint[this.Size];
			this._inm = new double[this.Size];
			this._tot = new double[this.Size];
			this.Communities = new HashSet<uint>[this.Size];

			//inits each node in its own community
			for (var i = 0u; i < this.Size; i++)
			{
				this.NodeCommunity[i] = i;
				this.Communities[i] = new HashSet<uint> { i };
				this._neighCommWeight[i] = -1;
				this._tot[this.NodeCommunity[i]] += this.Graph.Weights[i];
			}
		}

		/// <summary>
		///     Insert the node in comm with which it shares dnodecomm links.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="comm"></param>
		/// <param name="dnodecomm"></param>
		private void Insert(uint node, uint comm, double dnodecomm)
		{
			this._tot[comm] += this.Graph.Weights[node];
			this._inm[comm] += 2 * dnodecomm + this.GetSelfLoops(node);
			this.Communities[comm].Add(node);
			this.NodeCommunity[node] = comm;
		}

		/// <summary>
		///     Compute the gain of modularity if node where inserted in comm
		///     given that node has dnodecomm links to comm.  The formula is:
		///     [(In(comm)+2d(node,comm))/2m - ((tot(comm)+deg(node))/2m)^2]-
		///     [In(comm)/2m - (tot(comm)/2m)^2 - (deg(node)/2m)^2]
		///     where In(comm)    = number of half-links strictly inside comm
		///     Tot(comm)   = number of half-links inside or outside comm (sum(degrees))
		///     d(node,com) = number of links from node to comm
		///     deg(node)   = node degree
		///     m           = number of links
		/// </summary>
		/// <param name="comm"></param>
		/// <param name="dnodecomm"></param>
		/// <param name="wDegree"></param>
		/// <returns></returns>
		private double ModularityGain(uint comm, double commWeight, double nodeWeight)
		{
			var commTot = this._tot[comm];
			var totWeight = this.Graph.TotalWeight;

			return (commWeight - ((commTot * nodeWeight) / totWeight));
		}

		/// <summary>
		///     Remove the node from its current community with which it has dnodecomm links.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="comm"></param>
		/// <param name="dnodecomm"></param>
		private void Remove(uint node, uint comm, double dnodecomm)
		{
			this._tot[comm] -= this.Graph.Weights[node];
			this._inm[comm] -= 2 * dnodecomm + this.GetSelfLoops(node);
			this.Communities[comm].Remove(node);
			this.NodeCommunity[node] = uint.MaxValue;
		}

		/// <summary>
		///     Renumber communities but does not change internal structure.
		/// </summary>
		/// <returns></returns>
		private uint[] RenumberCommunities(out uint final)
		{
			var renumber = new uint[this.Size];
			for (var node = 0; node < this.Size; node++)
				renumber[this.NodeCommunity[node]]++;

			final = 0;
			for (var i = 0u; i < this.Size; i++)
				if (renumber[i] != 0)
					renumber[i] = final++;
			return renumber;
		}

		/// <summary>
		///     Resets all variables but keeps community info from previous step.
		/// </summary>
		private void Reset()
		{
			//first re-calculates loops
			this.UpdateLoops();

			//zero weights
			this._tot.Initialize(0d);
			this._numNeighComm = 0;
			this._neighCommWeight.Initialize(0);
			this._neighComm.Initialize(0u);

			//reset weights accounting for possible edge changes
			for (var i = 0u; i < this.Size; i++)
			{
				this._neighCommWeight[i] = -1;
				this._tot[this.NodeCommunity[i]] += this.Graph.Weights[i];
			}
		}

		private void UpdateLoops()
		{
			if (this.Graph == null) return;
			for (var v = 0u; v < this.Size; v++)
				this._inm[v] = this.GetSelfLoops(v);
		}

		#endregion Private Methods
	}
}