// ------------------------------------------
// <copyright file="ElementsTests.cs" company="Pedro Sequeira">
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
//    Project: Comuna.Tests
//    Last updated: 06/18/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Comuna.Tests
{
    [TestClass]
    public class ElementsTests
    {
        #region Static Fields & Constants

        private const string CSV_FILE = "network.csv";

        #endregion

        #region Public Methods

        [TestMethod]
        public void ClearConnectionsTest()
        {
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);
            for (var i = 0u; i < num - 1; i++)
                network.AddEdge(new Connection(i, i + 1));

            network.ClearConnections();
            Console.WriteLine(network);
            Assert.AreEqual(0, network.EdgeCount, "Network should not contain edges.");
            Assert.AreEqual(0, network.TotalWeight, double.Epsilon, "Network total weight should be 0.");
        }

        [TestMethod]
        public void ClearNetworkTest()
        {
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);
            for (var i = 0u; i < num - 1; i++)
                network.AddEdge(new Connection(i, i + 1));

            network.Clear();
            Console.WriteLine(network);
            Assert.AreEqual(0, network.EdgeCount, "Network should not contain edges.");
            Assert.AreEqual(0, network.VertexCount, "Network should not contain nodes.");
            Assert.AreEqual(0, network.TotalWeight, double.Epsilon, "Network total weight should be 0.");
        }

        [TestMethod]
        public void ClearNodesTest()
        {
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);
            for (var i = 0u; i < num - 1; i++)
                network.AddEdge(new Connection(i, i + 1));

            network.ClearNodes();
            Console.WriteLine(network);
            Assert.AreEqual(0, network.VertexCount, "Network should not contain nodes.");
            Assert.AreEqual(0, network.TotalWeight, double.Epsilon, "Network total weight should be 0.");
        }

        [TestMethod]
        public void EqualConnectionsTest()
        {
            var conn1 = new Connection(1, 2);
            var conn2 = new Connection(2, 1);
            Assert.AreEqual(conn1, conn2, $"Connection {conn1} should be equal to connection {conn2}.");
            Assert.IsTrue(conn1 == conn2, $"Connection {conn1} should be equal to connection {conn2}.");
            Assert.IsFalse(conn1 != conn2, $"Connection {conn1} should be equal to connection {conn2}.");
        }

        [TestMethod]
        public void LoadCsvFileTest()
        {
            // creates and saves network
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);
            for (var i = 0u; i < num; i++)
            for (var j = i + 1; j < num; j++)
                network.AddEdge(new Connection(i, j));

            // save file
            var fullPath = Path.Combine(Path.GetFullPath("."), CSV_FILE);

            // loads network from file
            if (!File.Exists(fullPath)) return;
            network = Network.LoadFromCsv(fullPath);
            Assert.IsNotNull(network, "Network should not be null. Error occurred while loading file.");
            Console.WriteLine(network);

            // checks vertexes and edges
            Assert.AreEqual(num, network.VertexCount, $"Network should have {num} nodes.");
            for (var i = 0u; i < num; i++)
            for (var j = i + 1; j < num; j++)
            {
                Assert.IsTrue(network.TryGetEdge(i, j, out var connection), $"Network should have connection {i}-{j}.");
                Console.WriteLine(connection);
                Assert.AreEqual(1d, connection.Weight, double.Epsilon,
                    "Network connections should have a weight of 1.");
            }
        }

        [TestMethod]
        public void NetworkConnectionTest()
        {
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);
            for (var i = 0u; i < num - 1; i++)
            {
                var connection = new Connection(i, i + 1);
                network.AddEdge(connection);
                Console.WriteLine(connection);
                Assert.IsTrue(network.ContainsEdge(i, i + 1), $"Network should contain connection: {i}-{i + 1}.");
            }

            Console.WriteLine(network);
            Assert.AreEqual(num - 1, network.EdgeCount, $"Num. network connections should be {num - 1}.");
        }

        [TestMethod]
        public void NetworkNodeTest()
        {
            const int numNodes = 10;
            var network = new Network();
            for (var i = 0u; i < numNodes; i++)
            {
                network.AddVertex(i);
                Assert.IsTrue(network.ContainsVertex(i), $"Network should contain node: {i}.");
            }

            Console.WriteLine(network);
            Assert.AreEqual(numNodes, network.VertexCount, $"Num. network nodes should be {numNodes}.");
        }

        [TestMethod]
        public void NetworkWeightTest()
        {
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
            {
                network.AddVertex(i);
                Assert.AreEqual(0, network.Weights[i], double.Epsilon, $"Node {i} weight should be: 0.");
            }

            Console.WriteLine(network);
            Assert.AreEqual(0, network.TotalWeight, double.Epsilon, "Network total weight should be 0.");

            for (var i = 0u; i < num - 1; i++)
            {
                var connection = new Connection(i, i + 1);
                network.AddEdge(connection);
                Console.WriteLine(connection);
                var expected = i == 0 ? 1d : 2d;
                Console.WriteLine($"Node {i} weight: {network.Weights[i]}.");
                Assert.AreEqual(expected, network.Weights[i], double.Epsilon,
                    $"Node {i} weight should be: {expected}.");

                expected = (i + 1d) * 2d;
                Console.WriteLine($"Total weight: {network.TotalWeight}.");
                Assert.AreEqual(expected, network.TotalWeight, double.Epsilon,
                    $"Network total weight should be: {expected}.");
            }
        }

        [TestMethod]
        public void RemoveConnectionsTest()
        {
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);
            for (var i = 0u; i < num - 1; i++)
            {
                network.AddEdge(new Connection(i, i + 1));
                network.RemoveEdge(new Connection(i, i + 1));
                Assert.IsFalse(network.ContainsEdge(i, i + 1), $"Network should not contain connection: {i}-{i + 1}.");
            }

            Console.WriteLine(network);
            Assert.AreEqual(0, network.EdgeCount, "Network should not contain edges.");
            Assert.AreEqual(0, network.TotalWeight, double.Epsilon, "Network total weight should be 0.");
        }

        [TestMethod]
        public void RemoveNodesTest()
        {
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
            {
                network.AddVertex(i);
                network.RemoveVertex(i);
                Assert.IsFalse(network.ContainsVertex(i), $"Network should not contain node: {i}.");
            }

            Console.WriteLine(network);
            Assert.AreEqual(0, network.EdgeCount, "Network should not contain edges.");
            Assert.AreEqual(0, network.TotalWeight, double.Epsilon, "Network total weight should be 0.");
        }

        [TestMethod]
        public void SaveCsvFileTest()
        {
            // creates network nodes
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);

            // creates edges (fully-connected)
            for (var i = 0u; i < num; i++)
            for (var j = i + 1; j < num; j++)
                network.AddEdge(new Connection(i, j));
            Console.WriteLine(network);

            // save file
            var fullPath = Path.Combine(Path.GetFullPath("."), CSV_FILE);
            File.Delete(fullPath);
            network.SaveToCsv(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"CSV file should exist in {fullPath}.");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0, "CSV file size should be > 0 bytes.");
#if !DEBUG
            File.Delete(fullPath);
#endif
        }

        #endregion
    }
}