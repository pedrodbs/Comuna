// ------------------------------------------
// <copyright file="GraphvizTests.cs" company="Pedro Sequeira">
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
//    Project: Comuna.Graphviz.Tests
//    Last updated: 06/15/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph.Graphviz.Dot;

namespace Comuna.Graphviz.Tests
{
    [TestClass]
    public class GraphvizTests
    {
        #region Static Fields & Constants

        private const string FILE_NAME = "graph";
        private const int NUM_NODES = 10;
        private const int WAIT_TIMEOUT = 1000;

        private static readonly GraphvizImageType[] ImageTypes =
        {
            GraphvizImageType.Bmp,
            GraphvizImageType.Png,
            GraphvizImageType.Pdf,
            GraphvizImageType.Svg,
            GraphvizImageType.Svg,
            GraphvizImageType.Jpg
        };

        #endregion

        #region Public Methods

        [TestMethod]
        public void EqualEdgesTest()
        {
            var edge1 = new Edge(new Node(1), new Node(2));
            var edge2 = new Edge(new Node(2), new Node(1));
            Assert.AreEqual(edge1, edge2, $"Connection {edge1} should be equal to connection {edge2}.");
            Assert.IsTrue(edge1 == edge2, $"Connection {edge1} should be equal to connection {edge2}.");
            Assert.IsFalse(edge1 != edge2, $"Connection {edge1} should be equal to connection {edge2}.");
        }

        [TestMethod]
        public void EqualNodesTest()
        {
            var node1 = new Node(1);
            var node2 = new Node(1, 2);
            Assert.AreEqual(node1, node2, $"Connection {node1} should be equal to connection {node2}.");
            Assert.IsTrue(node1 == node2, $"Connection {node1} should be equal to connection {node2}.");
            Assert.IsFalse(node1 != node2, $"Connection {node1} should be equal to connection {node2}.");
            Assert.AreNotEqual(node1.Community, node2.Community);
        }

        [TestMethod]
        public void SaveFileTolDivTest()
        {
            SaveFileTest(TolPalettes.CreateTolDivPalette, "div");
        }

        [TestMethod]
        public void SaveFileTolRainbowTest()
        {
            SaveFileTest(TolPalettes.CreateTolRainbowPalette, "rainbow");
        }

        #endregion

        #region Private & Protected Methods

        private static void SaveFileTest(PaletteGenerator paletteGenerator, string name)
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

            var fullPath = Path.GetFullPath(".");
            foreach (var imageType in ImageTypes)
            {
                var fileName = $"{FILE_NAME}-{name}-{imageType}";
                var dotPath = Path.Combine(fullPath, $"{fileName}.dot");
                var imgPath = $"{dotPath}.{imageType.ToString().ToLower()}";
                File.Delete(dotPath);
                File.Delete(imgPath);

                var filePath = communityAlg.ToGraphvizFile(
                    fullPath, fileName, true, imageType, paletteGenerator, WAIT_TIMEOUT);

                Console.WriteLine(dotPath);
                Assert.IsTrue(File.Exists(dotPath), $"Dot file should exist in {dotPath}.");
                Assert.AreEqual(filePath, dotPath, $"Dot file should be exist in {imgPath}");

                Console.WriteLine(imgPath);
                Assert.IsTrue(File.Exists(imgPath), $"Image file should exist in {imgPath}.");
                Assert.IsTrue(new FileInfo(imgPath).Length > 0, "Image size should be > 0 bytes.");

#if !DEBUG
                File.Delete(dotPath);
                File.Delete(imgPath);
#endif
            }
        }

        #endregion
    }
}