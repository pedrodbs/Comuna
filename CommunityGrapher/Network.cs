// ------------------------------------------
// <copyright file="Network.cs" company="Pedro Sequeira">
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
//    Project: CommunityGrapher
//    Last updated: 05/25/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace CommunityGrapher
{
    /// <summary>
    ///     Represents a network structure with a series of <see cref="Connection" /> between nodes, where each node has a
    ///     distinct <see cref="uint" /> identifier.
    /// </summary>
    public class Network : UndirectedGraph<uint, Connection>
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the total weight associated with all connections in this network.
        /// </summary>
        public double TotalWeight { get; private set; }

        /// <summary>
        ///     Gets the weights associated with each node in this network.
        /// </summary>
        public Dictionary<uint, double> Weights { get; } = new Dictionary<uint, double>();

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds a new <see cref="Connection" /> to the network.
        /// </summary>
        /// <param name="connection">The connection to be added.</param>
        /// <returns>A <see cref="bool" /> indicating whether the connection was successfully added.</returns>
        public new bool AddEdge(Connection connection)
        {
            if (!base.AddEdge(connection)) return false;

            this.AddWeight(connection.Source, connection.Weight);
            this.AddWeight(connection.Target, connection.Weight);
            this.TotalWeight += 2 * connection.Weight;

            return true;
        }

        /// <summary>
        ///     Adds a new node to the network.
        /// </summary>
        /// <param name="node">The node (ID) to be added to the network.</param>
        /// <returns>A <see cref="bool" /> indicating whether the node was successfully added.</returns>
        public new bool AddVertex(uint node)
        {
            this.Weights[node] = 0;
            return base.AddVertex(node);
        }

        /// <summary>
        ///     Removes all nodes and connections from this network.
        /// </summary>
        public new void Clear()
        {
            this.ClearNodes();
            this.ClearConnections();
        }

        /// <summary>
        ///     Removes all connections from this network.
        /// </summary>
        public void ClearConnections()
        {
            foreach (var edge in this.Edges.ToList())
                this.RemoveEdge(edge);
        }

        /// <summary>
        ///     Removes all nodes from this network.
        /// </summary>
        public void ClearNodes()
        {
            foreach (var vertex in this.Vertices.ToList())
                this.RemoveVertex(vertex);
        }

        /// <summary>
        ///     Removes the given <see cref="Connection" /> from the network and updates weights.
        /// </summary>
        /// <param name="connection">The connection to be removed.</param>
        /// <returns>A <see cref="bool" /> indicating whether the connection was successfully removed.</returns>
        public new bool RemoveEdge(Connection connection)
        {
            if (!base.RemoveEdge(connection)) return false;

            this.RemoveWeight(connection.Source, connection.Weight);
            this.RemoveWeight(connection.Target, connection.Weight);
            this.TotalWeight -= 2 * connection.Weight;

            return true;
        }

        /// <summary>
        ///     Removes the given node (ID) from the network and updates weights.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        /// <returns>A <see cref="bool" /> indicating whether the node was successfully removed.</returns>
        public new bool RemoveVertex(uint node)
        {
            if (this.Weights.ContainsKey(node))
                this.Weights.Remove(node);
            return base.RemoveVertex(node);
        }

        #endregion

        #region Private & Protected Methods

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

        #endregion
    }
}