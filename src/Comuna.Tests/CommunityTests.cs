// ------------------------------------------
// <copyright file="CommunityTests.cs" company="Pedro Sequeira">
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
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Comuna.Tests
{
    [TestClass]
    public class CommunityTests
    {
        #region Public Methods

        [TestMethod]
        public void CommunityGraphTest()
        {
            // creates network nodes
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);
            Console.WriteLine(network);

            // creates and updates community algorithm
            var commAlgorithm = new CommunityAlgorithm(network);
            commAlgorithm.Update();
            commAlgorithm.DisplayNodesCommunities();
            commAlgorithm.DisplayCommunities();

            // gets community graph
            var communityGraph = commAlgorithm.GetCommunityNetwork();
            Console.WriteLine(communityGraph);

            Assert.AreEqual(0, communityGraph.EdgeCount, "Community graph should not contain edges.");
            Assert.AreEqual(num, communityGraph.VertexCount, $"Num. communities should be {num}.");
            Assert.AreEqual(0, communityGraph.TotalWeight, double.Epsilon, "Community graph total weight should be 0.");
        }

        [TestMethod]
        public void DisposeTest()
        {
            // creates network nodes
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);

            // creates edges
            for (var i = 0u; i < num; i++)
            for (var j = i + 1; j < num; j++)
                network.AddEdge(new Connection(i, j));

            // creates and updates community algorithm
            var commAlgorithm = new CommunityAlgorithm(network);
            commAlgorithm.Update();

            // disposes algorithm
            commAlgorithm.Dispose();

            Assert.IsNull(commAlgorithm.Communities, "Community algorithm should not have any communities.");
            Assert.IsNull(commAlgorithm.NodesCommunities, "Community algorithm should not have any communities.");

            commAlgorithm.Dispose();
        }

        [TestMethod]
        public void FullyConnectedTest()
        {
            // creates network nodes
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);

            // creates edges (full)
            for (var i = 0u; i < num; i++)
            for (var j = i + 1; j < num; j++)
                network.AddEdge(new Connection(i, j));
            Console.WriteLine(network);

            // creates and updates community algorithm
            var commAlgorithm = new CommunityAlgorithm(network);
            commAlgorithm.Update();
            commAlgorithm.DisplayNodesCommunities();
            commAlgorithm.DisplayCommunities();

            for (var i = 0u; i < num; i++)
                Assert.AreEqual(0u, commAlgorithm.NodesCommunities[i], $"Node {i} should be in community 0.");
            Assert.AreEqual(1, commAlgorithm.GetNumberCommunities(), "Num. communities should be 1.");
            Assert.AreEqual(0, commAlgorithm.GetModularity(), double.Epsilon, "Community modularity should be 0.");
        }

        [TestMethod]
        public void NetworkPassesTest()
        {
            // creates network nodes
            const int numNodes = 16;
            var network = new Network();
            for (var i = 0u; i < numNodes; i++)
                network.AddVertex(i);

            // creates edges (according to paper)
            network.AddEdge(new Connection(0, 2));
            network.AddEdge(new Connection(0, 4));
            network.AddEdge(new Connection(0, 3));
            network.AddEdge(new Connection(0, 5));
            network.AddEdge(new Connection(1, 2));
            network.AddEdge(new Connection(1, 4));
            network.AddEdge(new Connection(1, 7));
            network.AddEdge(new Connection(2, 4));
            network.AddEdge(new Connection(2, 5));
            network.AddEdge(new Connection(2, 6));
            network.AddEdge(new Connection(3, 7));
            network.AddEdge(new Connection(4, 10));
            network.AddEdge(new Connection(5, 7));
            network.AddEdge(new Connection(5, 11));
            network.AddEdge(new Connection(6, 7));
            network.AddEdge(new Connection(6, 11));
            network.AddEdge(new Connection(8, 9));
            network.AddEdge(new Connection(8, 10));
            network.AddEdge(new Connection(8, 11));
            network.AddEdge(new Connection(8, 14));
            network.AddEdge(new Connection(8, 15));
            network.AddEdge(new Connection(9, 12));
            network.AddEdge(new Connection(9, 14));
            network.AddEdge(new Connection(10, 11));
            network.AddEdge(new Connection(10, 12));
            network.AddEdge(new Connection(10, 13));
            network.AddEdge(new Connection(10, 14));
            network.AddEdge(new Connection(11, 13));

            // creates and updates community algorithm
            var communityAlg = new CommunityAlgorithm(network, -1, 0);
            communityAlg.Update();
            communityAlg.DisplayCommunities();

            //checks communities
            Assert.AreEqual(4, communityAlg.GetNumberCommunities(), "Network should have 4 communities.");
            var communities = new[]
                              {
                                  new HashSet<uint> {0, 1, 2, 4, 5},
                                  new HashSet<uint> {3, 6, 7},
                                  new HashSet<uint> {11, 13},
                                  new HashSet<uint> {8, 9, 10, 12, 14, 15}
                              };
            for (var i = 0; i < 4; i++)
            {
                var community = communityAlg.Communities[i];
                Console.WriteLine(ArrayUtil.ToString(community.ToArray()));
                Assert.IsTrue(communities.Any(comm => comm.SetEquals(community)), $"Community {i} is unexpected.");
            }

            // gets and checks community graph (1st pass)
            communityAlg.DisplayCommunityGraph();
            communityAlg.DisplayPartition();
            network = communityAlg.GetCommunityNetwork();
            Assert.AreEqual(4, network.VertexCount, "Community network should have 4 nodes.");

            // checks connections
            var exptConns = new HashSet<Connection>
                            {
                                new Connection(0, 0, 16),
                                new Connection(0, 1, 1),
                                new Connection(0, 2, 3),
                                new Connection(1, 1, 14),
                                new Connection(1, 2, 1),
                                new Connection(1, 3, 4),
                                new Connection(2, 2, 2),
                                new Connection(2, 3, 1),
                                new Connection(3, 3, 4)
                            };
            var netConns = new HashSet<Connection>(network.Edges);
            foreach (var exptConn in exptConns)
            {
                Console.WriteLine(exptConn);
                Assert.IsTrue(netConns.Contains(exptConn), $"Community network should contain connection: {exptConn}.");
            }

            //// creates and updates community algorithm
            //communityAlg = new CommunityAlgorithm(network, -1, 0);
            //communityAlg.Update();
            //communityAlg.DisplayCommunities();
        }

        [TestMethod]
        public void NoConnectionsTest()
        {
            // creates network nodes
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);
            Console.WriteLine(network);

            // creates and updates community algorithm
            var commAlgorithm = new CommunityAlgorithm(network);
            commAlgorithm.Update();
            commAlgorithm.DisplayNodesCommunities();
            commAlgorithm.DisplayCommunities();

            Assert.AreEqual(0, network.EdgeCount, "Network should not contain edges.");
            for (var i = 0u; i < num; i++)
                Assert.AreEqual(i, commAlgorithm.NodesCommunities[i], $"Node {i} should be in community {i}.");
            Assert.AreEqual(num, commAlgorithm.GetNumberCommunities(), $"Num. communities should be {num}.");
            Assert.AreEqual(0, commAlgorithm.GetModularity(), double.Epsilon, "Community modularity should be 0.");
        }

        [TestMethod]
        public void RenumberTest()
        {
            // creates network nodes
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);

            // creates edges (random)
            var random = new Random();
            for (var i = 0u; i < num; i++)
            for (var j = i + 1; j < num; j++)
                if (random.NextDouble() > 0.6)
                    network.AddEdge(new Connection(i, j));
            Console.WriteLine(network);

            // creates and updates community algorithm
            var commAlgorithm = new CommunityAlgorithm(network);
            commAlgorithm.Update(false);

            // copies communities
            Console.WriteLine("Before renumbering:");
            commAlgorithm.DisplayCommunities();
            var beforeComms = new HashSet<uint>[num];
            for (var i = 0; i < num; i++)
                beforeComms[i] = new HashSet<uint>(commAlgorithm.Communities[i]);
            var beforeNumComms = commAlgorithm.GetNumberCommunities();

            // renumbers communities
            Console.WriteLine("After renumbering:");
            commAlgorithm.RenumberCommunities();
            commAlgorithm.DisplayCommunities();

            Assert.AreEqual(beforeNumComms, commAlgorithm.GetNumberCommunities(),
                $"Number of communities after renumbering should be the same: {beforeNumComms}");
            for (var i = 0u; i < num - 1; i++)
            {
                if (commAlgorithm.Communities[i].Count == 0)
                    Assert.IsTrue(commAlgorithm.Communities[i + 1].Count == 0,
                        $"Community {i + 1} should have 0 nodes since community {i} has 0.");
                if (i < beforeNumComms && beforeComms[i].Count > 0)
                    Assert.IsTrue(beforeComms[i].SetEquals(commAlgorithm.Communities[i]),
                        $"Renumbering should have maintained community {i}.");
            }
        }

        [TestMethod]
        public void ZeroWeightsTest()
        {
            // creates network nodes
            const int num = 10;
            var network = new Network();
            for (var i = 0u; i < num; i++)
                network.AddVertex(i);

            // creates edges (full with 0 weight)
            for (var i = 0u; i < num; i++)
            for (var j = i + 1; j < num; j++)
                network.AddEdge(new Connection(i, j, 0));
            Console.WriteLine(network);

            // creates and updates community algorithm
            var commAlgorithm = new CommunityAlgorithm(network);
            commAlgorithm.Update();
            commAlgorithm.DisplayNodesCommunities();
            commAlgorithm.DisplayCommunities();

            for (var i = 0u; i < num; i++)
                Assert.AreEqual(i, commAlgorithm.NodesCommunities[i], $"Node {i} should be in community {i}.");
            Assert.AreEqual(num, commAlgorithm.GetNumberCommunities(), $"Num. communities should be {num}.");
            Assert.AreEqual(0, commAlgorithm.GetModularity(), double.Epsilon, "Community modularity should be 0.");
        }

        #endregion
    }
}