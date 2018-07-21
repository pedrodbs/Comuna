// ------------------------------------------
// <copyright file="D3Tests.cs" company="Pedro Sequeira">
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
//    Project: Comuna.D3.Tests
//    Last updated: 05/26/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Comuna.D3.Tests
{
    [TestClass]
    public class D3Tests
    {
        #region Static Fields & Constants

        private const string FILE_NAME = "graph.json";
        private const string EVO_FILE_NAME = "evo-graph.json";
        private const int NUM_NODES = 10;
        private const int NUM_PASSES = -1;
        private const double MIN_MODULARITY = 1E-6;

        #endregion

        #region Public Methods

        [TestMethod]
        public void SaveFileTest()
        {
            // creates graph and adds nodes
            var network = new Network();
            for (var i = 0u; i < NUM_NODES; i++)
                network.AddVertex(i);

            // adds connections
            network.AddEdge(new Connection(0, 1));
            network.AddEdge(new Connection(0, 2));
            network.AddEdge(new Connection(0, 9));
            network.AddEdge(new Connection(2, 4));
            network.AddEdge(new Connection(2, 9));
            network.AddEdge(new Connection(4, 7));
            network.AddEdge(new Connection(7, 9));
            network.AddEdge(new Connection(8, 9));

            // creates algorithm and updates communities
            var communityAlg = new CommunityAlgorithm(network, -1, 0.000001);
            communityAlg.Update();
            communityAlg.DisplayCommunities();

            var fullPath = Path.Combine(Path.GetFullPath("."), FILE_NAME);
            File.Delete(fullPath);

            communityAlg.ToD3GraphFile(FILE_NAME, Formatting.Indented);

            Console.WriteLine(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"D3 json file should exist in {fullPath}.");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0, "Json file size should be > 0 bytes.");
#if !DEBUG
            File.Delete(fullPath);
#endif
        }

        [TestMethod]
        public void SaveGraphEvolutionFileTest()
        {
            var network = new Network();
            for (var i = 0u; i < NUM_NODES; i++)
                network.AddVertex(i);

            var fullPath = Path.Combine(Path.GetFullPath("."), EVO_FILE_NAME);
            File.Delete(fullPath);
            Console.WriteLine(fullPath);

            var communityAlg = new CommunityAlgorithm(network, NUM_PASSES, MIN_MODULARITY);
            using (var tracker = new CommunityTracker(communityAlg, fullPath, 6, Formatting.Indented))
            {
                var e01 = new Connection(1, 0);
                var e02 = new Connection(0, 2);
                var e12 = new Connection(2, 1);
                var e13 = new Connection(1, 3);
                var e34 = new Connection(4, 3);
                Update(communityAlg, tracker);

                Console.WriteLine("Inserting 0->1, 0->2 and 1->2");
                network.AddEdge(e01);
                network.AddEdge(e02);
                network.AddEdge(e12);
                Update(communityAlg, tracker);

                Console.WriteLine("Inserting 1->3 and 3->4");
                network.AddEdge(e13);
                network.AddEdge(e34);
                Update(communityAlg, tracker);

                Console.WriteLine("Removing 0->1, 0->2 and 1->2");
                network.RemoveEdge(e01);
                network.RemoveEdge(e02);
                network.RemoveEdge(e12);
                Update(communityAlg, tracker);

                Console.WriteLine("Removing 1->3 and 3->4");
                network.RemoveEdge(e13);
                network.RemoveEdge(e34);
                Update(communityAlg, tracker);

                Console.WriteLine("Inserting 0->1, 0->2 and 1->2");
                network.AddEdge(e01);
                network.AddEdge(e02);
                network.AddEdge(e12);
                Update(communityAlg, tracker);
            }

            Assert.IsTrue(File.Exists(fullPath), $"D3 json file should exist in {fullPath}.");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0, "Json file size should be > 0 bytes.");
#if !DEBUG
            File.Delete(fullPath);
#endif
        }

        #endregion

        #region Private & Protected Methods

        private static void Update(CommunityAlgorithm communityAlg, CommunityTracker tracker)
        {
            communityAlg.Update();
            communityAlg.DisplayNodesCommunities();
            Console.WriteLine("Modularity: {0}", communityAlg.GetModularity());

            tracker.Update();
        }

        #endregion
    }
}