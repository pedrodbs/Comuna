// ------------------------------------------
// NetworkGraph.cs, PS.CommunityGrapher
// 
// Created by Pedro Sequeira, 2015/06/26
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace CommunityGrapher.Domain
{
    public class NetworkGraph : UndirectedGraph<uint, NetworkEdge>
    {
        public NetworkGraph()
        {
            this.Weights = new Dictionary<uint, double>();
        }

        public double TotalWeight { get; private set; }
        public Dictionary<uint, double> Weights { get; private set; }

        public static NetworkGraph LoadFile(string filename)
        {
            var graph = new NetworkGraph();
            //using (var xreader = XmlReader.Create(new StreamReader(filename)))
            //    graph.DeserializeFromGraphML(xreader,
            //        uint.Parse,
            //        (source, target, id) => new NetworkEdge(source, target));
            return graph;
        }

        public new bool AddVertex(uint v)
        {
            this.Weights[v] = 0;
            return base.AddVertex(v);
        }

        public new bool RemoveVertex(uint v)
        {
            if (this.Weights.ContainsKey(v))
                this.Weights.Remove(v);
            return base.RemoveVertex(v);
        }

        public new bool AddEdge(NetworkEdge e)
        {
            if (!base.AddEdge(e)) return false;

            this.AddWeight(e.Source, e.Weight);
            this.AddWeight(e.Target, e.Weight);
            this.TotalWeight += 2*e.Weight;

            return true;
        }

        public new bool RemoveEdge(NetworkEdge e)
        {
            if (!base.RemoveEdge(e)) return false;

            this.RemoveWeight(e.Source, e.Weight);
            this.RemoveWeight(e.Target, e.Weight);
            this.TotalWeight -= 2*e.Weight;

            return true;
        }

        private void AddWeight(uint vertex, double weight)
        {
            if (!this.Weights.ContainsKey(vertex))
                this.Weights.Add(vertex, weight);
            else
                this.Weights[vertex] += weight;
        }

        private void RemoveWeight(uint vertex, double weight)
        {
            if (this.Weights.ContainsKey(vertex))
                this.Weights[vertex] -= weight;
        }

        public void ClearEdges()
        {
            foreach (var edge in this.Edges.ToList())
                this.RemoveEdge(edge);
        }

        public void ClearVertices()
        {
            foreach (var vertex in this.Vertices.ToList())
                this.RemoveVertex(vertex);
        }

        public new void Clear()
        {
            this.ClearVertices();
            this.ClearEdges();
        }
    }
}