// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
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
//    Project: CommunitiesEvolution
//    Last updated: 06/18/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using Comuna;
using Comuna.D3;
using Newtonsoft.Json;

namespace CommunitiesEvolution
{
    internal class Program
    {
        #region Static Fields & Constants

        private const int NUM_NODES = 5;
        private const int NUM_PASSES = -1;
        private const double MIN_MODULARITY = 1E-6;
        private const string D3_JSON_FILE = "graphs.json";
        private const int MAX_UPDATES = 6;

        #endregion

        #region Private & Protected Methods

        private static void Main(string[] args)
        {
            var network = new Network();
            for (var i = 0u; i < NUM_NODES; i++)
                network.AddVertex(i);

            var communityAlg = new CommunityAlgorithm(network, NUM_PASSES, MIN_MODULARITY);
            using (var tracker = new CommunityTracker(communityAlg, D3_JSON_FILE, MAX_UPDATES, Formatting.Indented))
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

            Console.WriteLine("Done!");
            Console.ReadKey();
        }

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