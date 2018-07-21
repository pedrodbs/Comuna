// ------------------------------------------
// <copyright file="CommunityAlgorithm.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: Comuna
//    Last updated: 07/20/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Comuna
{
    /// <summary>
    ///     Implements the "Louvain method" for finding communities in large networks as described in [1]. The code corresponds
    ///     to a c# adaptation of the c++ code in <see href="https://sites.google.com/site/findcommunities/" />.
    ///     A feature was added so that multiple updates to the underlying network connections (graph links) is
    ///     supported, i.e., it allows the analysis of the evolution of communities in a network.
    /// </summary>
    /// <remarks>
    ///     [1] - Blondel, V. D., Guillaume, J. L., Lambiotte, R., &amp; Lefebvre, E. (2008). Fast unfolding of communities in
    ///     large networks. Journal of statistical mechanics: theory and experiment, 2008(10), Bristol: IOP Publishing Ltd.
    /// </remarks>
    public class CommunityAlgorithm : IDisposable
    {
        #region Fields

        /// <summary>
        ///     Used to compute the modularity participation of each community.
        /// </summary>
        private double[] _inm;

        private uint[] _neighComm;
        private double[] _neighCommWeight;
        private uint _numNeighComm;

        /// <summary>
        ///     Used to compute the modularity participation of each community
        /// </summary>
        private double[] _tot;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="CommunityAlgorithm" /> according to the provided graph.
        /// </summary>
        /// <param name="network">The network graph to extract communities from.</param>
        /// <param name="numPasses">
        ///     The number of passes for one level computation.  If -1, the algorithm computes as many passes as needed to increase
        ///     modularity.
        /// </param>
        /// <param name="minModularity">
        ///     The criterion used to perform a new pass. If 0, even a minor increase is enough to perform one more pass.
        /// </param>
        public CommunityAlgorithm(Network network, int numPasses = -1, double minModularity = 1E-6)
        {
            this.Network = network;
            this.NumPasses = numPasses;
            this.MinModularity = minModularity;

            // starts community analysis
            this.Init();
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the set of nodes belonging to each community.
        /// </summary>
        public HashSet<uint>[] Communities { get; private set; }

        /// <summary>
        ///     Gets the minimal modularity difference between passes. If 0, even a minor increase is enough to perform one more
        ///     pass.
        /// </summary>
        public double MinModularity { get; }

        /// <summary>
        ///     Gets the network graph used to compute communities.
        /// </summary>
        public Network Network { get; }

        /// <summary>
        ///     Gets the community of each node.
        /// </summary>
        public uint[] NodesCommunities { get; private set; }

        /// <summary>
        ///     Gets the number of passes for one level computation. If -1, the algorithm computes as many passes as needed to
        ///     increase modularity.
        /// </summary>
        public int NumPasses { get; }

        /// <summary>
        ///     Gets the number of nodes in the network and size of all vectors.
        /// </summary>
        public int Size => this.Network.VertexCount;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Displays the nodes belonging to each community.
        /// </summary>
        public void DisplayCommunities()
        {
            for (var i = 0u; i < this.Size; i++)
            {
                var nodes = this.Communities[i];
                if (nodes.Count == 0) continue;
                Console.WriteLine(
                    $"Community: {i}, Nodes: {nodes.ToArray().ToVectorString()}, In: {this._inm[i]}, Tot: {this._tot[i]}");
            }
        }

        /// <summary>
        ///     Displays the graph of the existing communities, i.e., without the nodes.
        /// </summary>
        public void DisplayCommunityGraph()
        {
            var renumber = this.RenumberCommunities(out _);
            for (var i = 0u; i < this.Size; i++)
            {
                var neighbours = this.Network.AdjacentEdges(i);
                foreach (var edge in neighbours)
                {
                    var neigh = edge.Source.Equals(i) ? edge.Target : edge.Source;
                    Console.WriteLine($"{renumber[this.NodesCommunities[i]]} {renumber[this.NodesCommunities[neigh]]}");
                }
            }
        }

        /// <summary>
        ///     Displays the community that each node in the network belongs to.
        /// </summary>
        public void DisplayNodesCommunities()
        {
            for (var i = 0u; i < this.Size; i++)
            {
                var community = this.NodesCommunities[i];
                Console.WriteLine(
                    $"Node: {i}, Community: {community}, In: {this._inm[community]}, Tot: {this._tot[community]}");
            }
        }

        /// <summary>
        ///     Displays the current partition (with communities renumbered from 0 to k-1).
        /// </summary>
        public void DisplayPartition()
        {
            var renumber = this.RenumberCommunities(out _);
            for (var i = 0u; i < this.Size; i++)
                Console.WriteLine($"{i} {renumber[this.NodesCommunities[i]]}");
        }

        /// <summary>
        ///     Generates a <see cref="Comuna.Network" /> of the existing communities, i.e., without the nodes.
        /// </summary>
        /// <returns>The network graph of communities.</returns>
        public Network GetCommunityNetwork()
        {
            // renumbers communities
            var renumber = this.RenumberCommunities(out var final);
            var commNodes = new List<uint>[(int) final];
            for (var node = 0u; node < this.Size; node++)
            {
                var comm = renumber[this.NodesCommunities[node]];
                if (commNodes[comm] == null)
                    commNodes[comm] = new List<uint>();

                commNodes[comm].Add(node);
            }

            // add community vertexes
            var commNetwork = new Network();
            for (var comm = 0u; comm < commNodes.Length; comm++)
                commNetwork.AddVertex(comm);

            // compute weighted graph
            for (var comm = 0u; comm < commNodes.Length; comm++)
            {
                // compute weighted edges
                var m = new Dictionary<uint, double>();
                foreach (var node in commNodes[comm])
                {
                    var neighbours = this.Network.AdjacentEdges(node);
                    foreach (var edge in neighbours)
                    {
                        var neigh = edge.Source.Equals(node) ? edge.Target : edge.Source;
                        var neighComm = renumber[this.NodesCommunities[neigh]];
                        var neighWeight = edge.Weight;

                        if (!m.ContainsKey(neighComm))
                            m[neighComm] = neighWeight;
                        else
                            m[neighComm] += neighWeight;
                    }
                }

                //add community edges
                foreach (var commEdge in m)
                    if (!commNetwork.ContainsEdge(comm, commEdge.Key) &&
                        !commNetwork.ContainsEdge(commEdge.Key, comm))
                        commNetwork.AddEdge(new Connection(comm, commEdge.Key, commEdge.Value));
            }

            return commNetwork;
        }

        /// <summary>
        ///     Gets the modularity of the current community partition.
        /// </summary>
        /// <returns>The modularity of the current community partition.</returns>
        public double GetModularity()
        {
            var q = 0d;
            var m2 = this.Network.TotalWeight;

            for (var i = 0; i < this.Size; i++)
                if (this._tot[i] > 0)
                    q += this._inm[i] / m2 - this._tot[i] / m2 * (this._tot[i] / m2);

            return q;
        }

        /// <summary>
        ///     Gets the number of active communities, i.e., the communities with one or more elements belonging to it.
        /// </summary>
        /// <returns>The number of active communities.</returns>
        public int GetNumberCommunities() => this.Communities.Count(comm => comm.Count > 0);

        /// <summary>
        ///     Initializes the community partition with the information stored in the given file.
        /// </summary>
        /// <param name="fileName">The path to the file containing the partition information.</param>
        public void LoadFromFile(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var finput = new StreamReader(fs, Encoding.UTF8))
            {
                // read partitions
                string line;
                while ((line = finput.ReadLine()) != null)
                {
                    var elems = line.Split(' ');
                    if (!uint.TryParse(elems[0], out var node)) continue;
                    if (!uint.TryParse(elems[1], out var comm)) continue;

                    var oldComm = this.NodesCommunities[node];
                    this.ComputeNeighourCommWeights(node);

                    this.Remove(node, oldComm, this._neighCommWeight[oldComm]);

                    var i = 0u;
                    for (; i < this._numNeighComm; i++)
                    {
                        var neighPos = this._neighComm[i];
                        var bestComm = neighPos;
                        var bestCommWeight = this._neighCommWeight[neighPos];
                        if (bestComm != comm) continue;

                        this.Insert(node, bestComm, bestCommWeight);
                        break;
                    }

                    if (i.Equals(this._numNeighComm))
                        this.Insert(node, comm, 0);
                }
            }
        }

        /// <summary>
        ///     Renumbers each community according to the total number of communities (larger first) and by changing their ID to
        ///     the lowest one possible. Tries to keep communities IDs if possible to avoid renumbering.
        /// </summary>
        public void RenumberCommunities()
        {
            //collect stats about all communities
            var oldCommStats = new Dictionary<uint, uint>(this.Size);
            for (var node = 0u; node < this.Size; node++)
            {
                var comm = this.NodesCommunities[node];
                if (!oldCommStats.ContainsKey(comm))
                    oldCommStats[comm] = 1;
                else
                    oldCommStats[comm]++;
            }

            // creates dictionary with old -> new community ID and put communities with ID within range -- these remain unchanged
            var newCommCount = oldCommStats.Count;
            var oldToNewComms = new Dictionary<uint, uint>(this.Size);
            for (var comm = 0u; comm < newCommCount; comm++)
                if (oldCommStats.ContainsKey(comm))
                {
                    oldToNewComms[comm] = comm;
                    oldCommStats.Remove(comm);
                }

            // sorts remaining communities by size and inserts each of them in a vague community ID
            var sortedOldComms = oldCommStats.Keys.ToList();
            sortedOldComms.Sort((x, y) => oldCommStats[y].CompareTo(oldCommStats[x]));
            var oldCommIdx = 0;
            for (var newComm = 0u; oldCommIdx < sortedOldComms.Count && newComm < newCommCount; newComm++)
                if (!oldToNewComms.ContainsKey(newComm))
                    oldToNewComms[sortedOldComms[oldCommIdx++]] = newComm;

            //replaces community for all nodes
            for (var node = 0u; node < this.Size; node++)
            {
                var oldComm = this.NodesCommunities[node];
                this.Remove(node, oldComm, this._neighCommWeight[oldComm]);

                var newComm = oldToNewComms[oldComm];
                this.Insert(node, newComm, this._neighCommWeight[newComm]);
            }
        }

        /// <summary>
        ///     Computes communities in the graph iteratively until there are changes in any node's community or the changes in
        ///     modularity are large enough.
        /// </summary>
        /// <param name="renumberCommunities">
        ///     Whether to renumber communities after the update by calling
        ///     <see cref="RenumberCommunities" />.
        /// </param>
        /// <returns><c>true</c>, if some node changed community, <c>false</c> otherwise.</returns>
        public bool Update(bool renumberCommunities = true)
        {
            this.Reset();

            var improvement = false;
            int numMoves;
            var newMod = this.GetModularity();
            double curMod;
            var numPassDone = 0;

            //var randomOrder = GetRandomOrder(this.Size);

            // repeat while:
            //   - there is an improvement of modularity greater than a given threshold OR
            //   - a predefined number of passes has been performed
            do
            {
                curMod = newMod;
                numMoves = 0;
                numPassDone++;

                // for each node: remove the node from its community and insert it in the best community
                // nodes are analyzed in descending order to ensure higher numbered nodes go to lowest-number communities
                for (var n = 0u; n < this.Size; n++)
                {
                    var node = (uint) this.Size - n - 1;

                    //var node = randomOrder[nodeTmp];
                    var nodeComm = this.NodesCommunities[node];
                    var nodeWeight = this.Network.Weights[node];

                    // compute all neighboring communities of current node
                    this.ComputeNeighourCommWeights(node);

                    // remove node from its current community
                    var commWeight = this._neighCommWeight[nodeComm];
                    this.Remove(node, nodeComm, commWeight);

                    // compute the nearest community for node
                    // default choice for future insertion is the former community
                    var bestNeighComm = nodeComm;
                    var bestNeighCommWeight = commWeight;
                    var bestModGain = 0d;
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

                    // increases moves if community for node changed
                    if (bestNeighComm != nodeComm)
                        numMoves++;
                }

                //gets new modularity and checks improvement
                newMod = this.GetModularity();
                improvement |= numMoves > 0;
            } while (numMoves > 0 && newMod - curMod > this.MinModularity &&
                     (this.NumPasses == -1 || numPassDone <= this.NumPasses));

            //renumbers communities if asked
            if (renumberCommunities)
                this.RenumberCommunities();

            return improvement;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Network.Clear();
            this.NodesCommunities = null;
            this.Communities = null;
            this._neighCommWeight = null;
            this._neighComm = null;
            this._inm = null;
            this._tot = null;
        }

        #endregion

        #region Private & Protected Methods

        /// <summary>
        ///     Computes the set of neighboring communities of a node for each community, gives the number of links from node to
        ///     community.
        /// </summary>
        /// <param name="node"></param>
        private void ComputeNeighourCommWeights(uint node)
        {
            for (var i = 0u; i < this._numNeighComm; i++)
                this._neighCommWeight[this._neighComm[i]] = -1;

            // adds node community itself
            this._neighComm[0] = this.NodesCommunities[node];
            this._neighCommWeight[this._neighComm[0]] = 0;
            this._numNeighComm = 1;

            // adds all neighbor's communities
            var neighbours = this.GetSortedNeighbours(node);
            foreach (var edge in neighbours)
            {
                var neigh = edge.Key;
                var neighComm = this.NodesCommunities[neigh];
                var neighWeight = edge.Value;

                if (neigh.Equals(node)) continue;

                // adds neighbor weight to neighbor's community weight
                if (this._neighCommWeight[neighComm].Equals(-1))
                {
                    this._neighCommWeight[neighComm] = 0.0;
                    this._neighComm[this._numNeighComm++] = neighComm;
                }

                this._neighCommWeight[neighComm] += neighWeight;
            }

            // adds also first empty community, if any (ensures partitioning of communities)
            for (var i = 0u; i < this.Size; i++)
                if (i != this._neighComm[0] && this.Communities[i].Count == 0)
                {
                    this._neighCommWeight[i] = 0.0;
                    this._neighComm[this._numNeighComm++] = i;
                    break;
                }
        }

        private double GetSelfLoops(uint node)
        {
            if (this.Network == null || this.Network.IsAdjacentEdgesEmpty(node)) return 0d;

            var neighbours = this.Network.AdjacentEdges(node);
            foreach (var edge in neighbours)
                if (edge.Source.Equals(edge.Target))
                    return edge.Weight;

            return 0;
        }

        private SortedList<uint, double> GetSortedNeighbours(uint node)
        {
            // gets neighbors sorted by id in ascending order
            // this ensures analysis of low-numbered neighbors first
            var sortedList = new SortedList<uint, double>();
            var neighbourEdges = this.Network.AdjacentEdges(node);
            foreach (var edge in neighbourEdges)
            {
                var neigh = edge.Source.Equals(node) ? edge.Target : edge.Source;
                sortedList.Add(neigh, edge.Weight);
            }

            return sortedList;
        }

        private void Init()
        {
            // inits all arrays
            this._neighCommWeight = new double[this.Size];
            this._neighComm = new uint[this.Size];
            this._numNeighComm = 0;

            this.NodesCommunities = new uint[this.Size];
            this._inm = new double[this.Size];
            this._tot = new double[this.Size];
            this.Communities = new HashSet<uint>[this.Size];

            // inits each node in its own community
            for (var i = 0u; i < this.Size; i++)
            {
                this.NodesCommunities[i] = i;
                this.Communities[i] = new HashSet<uint> {i};
                this._neighCommWeight[i] = -1;
                this._tot[i] += this.Network.Weights[i];
            }
        }

        /// <summary>
        ///     Insert the node in a community with which it shares the given weight.
        /// </summary>
        /// <param name="node">The node to be added to the community.</param>
        /// <param name="comm">The community in which to add the node.</param>
        /// <param name="weight">The weight associated with the node in the community.</param>
        private void Insert(uint node, uint comm, double weight)
        {
            this._tot[comm] += this.Network.Weights[node];
            this._inm[comm] += 2 * weight + this.GetSelfLoops(node);
            this.Communities[comm].Add(node);
            this.NodesCommunities[node] = comm;
        }

        /// <summary>
        ///     Compute the gain of modularity if a node with the given weight was inserted in the given community.
        /// </summary>
        /// <param name="comm">The community in which to insert the node.</param>
        /// <param name="commWeight">The weight of the community.</param>
        /// <param name="nodeWeight">The weight of the node in the community.</param>
        /// <returns>
        ///     The gain of modularity if a node with the given weight was inserted in the given community. A negative weight
        ///     indicates loss of modularity.
        /// </returns>
        private double ModularityGain(uint comm, double commWeight, double nodeWeight)
        {
            //The formula is:
            //     ΔQ = [((sumIn + kiIn) / 2m) - ((sumTot + ki) / 2m)^2] -
            //          [(sumIn / 2m) - (sumTot / 2m)^2 - (ki / 2m)^2]
            //     where:
            //     sumIn       = sum of the weights of the links inside comm
            //     sumTot      = sum of the weights of the links incident to nodes in comm
            //     ki          = sum of the weights of the links incident to the node
            //     kiIn        = sum of the weights of the links from the node to nodes in comm
            //     m           = sum of the weights of all the links in the network
            var m2 = 2 * this.Network.TotalWeight;
            if (Math.Abs(m2) < double.Epsilon) return 0;

            var ki = nodeWeight;
            var kiIn = commWeight;
            var sumTot = this._tot[comm];
            var sumIn = this._inm[comm];

            return (sumIn + kiIn) / m2 - Math.Pow((sumTot + ki) / m2, 2) -
                   (sumIn / m2 - Math.Pow(sumTot / m2, 2) - Math.Pow(ki / m2, 2));
        }

        /// <summary>
        ///     Removes the node from its current community with which it has the given associated weight.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        /// <param name="comm">The community from which to remove the node.</param>
        /// <param name="weight">The weight of the node associated with the community.</param>
        private void Remove(uint node, uint comm, double weight)
        {
            this._tot[comm] -= this.Network.Weights[node];
            this._inm[comm] -= 2 * weight + this.GetSelfLoops(node);
            this.Communities[comm].Remove(node);
            this.NodesCommunities[node] = uint.MaxValue;
        }

        /// <summary>
        ///     Renumbers communities to avoid having IDs with no nodes associated, but does not change internal structure.
        /// </summary>
        /// <returns>The new communities for each node.</returns>
        private uint[] RenumberCommunities(out uint final)
        {
            var renumber = new uint[this.Size];
            for (var node = 0; node < this.Size; node++)
                renumber[this.NodesCommunities[node]]++;

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
            // first re-calculates loops
            this.UpdateLoops();

            //zero weights
            this._tot.Initialize(0d);
            this._numNeighComm = 0;
            this._neighCommWeight.Initialize(0);
            this._neighComm.Initialize(0u);

            // resets weights accounting for possible edge changes
            for (var i = 0u; i < this.Size; i++)
            {
                this._neighCommWeight[i] = -1;
                this._tot[this.NodesCommunities[i]] += this.Network.Weights[i];
            }
        }

        private void UpdateLoops()
        {
            if (this.Network == null) return;
            for (var v = 0u; v < this.Size; v++)
                this._inm[v] = this.GetSelfLoops(v);
        }

        #endregion
    }
}