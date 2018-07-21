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
//    Project: Comuna
//    Last updated: 06/18/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QuickGraph;

namespace Comuna
{
    /// <summary>
    ///     Represents a network structure with a series of <see cref="Connection" /> between nodes, where each node has a
    ///     distinct <see cref="uint" /> identifier.
    /// </summary>
    public class Network : UndirectedGraph<uint, Connection>, IDisposable
    {
        #region Static Fields & Constants

        private const char CSV_SEP_CHAR = ',';

        #endregion

        #region Fields

        private readonly Dictionary<uint, double> _weights = new Dictionary<uint, double>();

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the total weight associated with all connections in this network.
        /// </summary>
        public double TotalWeight { get; private set; }

        /// <summary>
        ///     Gets the weights associated with each node in this network.
        /// </summary>
        public IReadOnlyDictionary<uint, double> Weights => this._weights;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override string ToString() =>
            $"Nodes: {this.VertexCount}, conn: {this.EdgeCount}, weight: {this.TotalWeight}";

        #endregion

        #region Public Methods

        /// <summary>
        ///     Creates a new <see cref="Network" /> by reading the edge information stored in the given CSV (comma-separated
        ///     values) file.
        ///     The format is 'source_node, target_node [, weight]'. If not provided, weight of the edge is considered to be 1.
        /// </summary>
        /// <param name="filePath">The path to the CSV file containing the network information.</param>
        /// <param name="sepChar">The character used to separate each field in the file.</param>
        /// <returns>A new network according to the information in the given file, or <c>null</c> if no information could be read.</returns>
        public static Network LoadFromCsv(string filePath, char sepChar = CSV_SEP_CHAR)
        {
            // checks file
            if (!File.Exists(filePath)) return null;

            var separator = new[] {sepChar};

            // reads network, one edge per line
            var network = new Network();
            using (var fs = new FileStream(filePath, FileMode.Open))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // checks empty / incorrect line
                    var fields = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (fields.Length < 2) continue;

                    // parses source, target and weight
                    if (!uint.TryParse(fields[0], out var source)) continue;
                    if (!uint.TryParse(fields[1], out var target)) continue;
                    if (fields.Length < 3 || !double.TryParse(fields[2], out var weight)) weight = 1d;

                    // checks vertexes
                    if (!network.ContainsVertex(source))
                        network.AddVertex(source);
                    if (!network.ContainsVertex(target))
                        network.AddVertex(target);

                    // adds edge
                    network.AddEdge(new Connection(source, target, weight));
                }
            }

            return network;
        }

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
            return true;
        }

        /// <summary>
        ///     Adds a new node to the network.
        /// </summary>
        /// <param name="node">The node (ID) to be added to the network.</param>
        /// <returns>A <see cref="bool" /> indicating whether the node was successfully added.</returns>
        public new bool AddVertex(uint node)
        {
            this._weights[node] = 0;
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
            return true;
        }

        /// <summary>
        ///     Removes the given node (ID) from the network and updates weights.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        /// <returns>A <see cref="bool" /> indicating whether the node was successfully removed.</returns>
        public new bool RemoveVertex(uint node)
        {
            foreach (var edge in this.AdjacentEdges(node).ToArray())
                this.RemoveEdge(edge);

            if (this.Weights.ContainsKey(node))
                this._weights.Remove(node);

            return base.RemoveVertex(node);
        }

        /// <summary>
        ///     Writes the network's edge information to a given CSV (comma-separated values) file.
        ///     The format is 'source_node, target_node, weight]'.
        /// </summary>
        /// <param name="filePath">The path to the CSV file in which to store the network information.</param>
        /// <param name="sepChar">The character used to separate each field in the file.</param>
        public void SaveToCsv(string filePath, char sepChar = CSV_SEP_CHAR)
        {
            // writes network, one edge per line
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
                foreach (var edge in this.Edges)
                    sw.WriteLine($"{edge.Source}{sepChar}{edge.Target}{sepChar}{edge.Weight}");
        }

        public void Dispose() => this.Clear();

        #endregion

        #region Private & Protected Methods

        private void AddWeight(uint vertex, double weight)
        {
            if (!this.Weights.ContainsKey(vertex))
                this._weights.Add(vertex, weight);
            else
                this._weights[vertex] += weight;
            this.TotalWeight += weight;
        }

        private void RemoveWeight(uint vertex, double weight)
        {
            if (!this.Weights.ContainsKey(vertex)) return;
            this._weights[vertex] -= weight;
            this.TotalWeight -= weight;
        }

        #endregion
    }
}