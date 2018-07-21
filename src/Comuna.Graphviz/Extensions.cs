// ------------------------------------------
// <copyright file="Extensions.cs" company="Pedro Sequeira">
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
//    Project: Comuna.Graphviz
//    Last updated: 06/18/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Diagnostics;
using System.Globalization;
using System.IO;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace Comuna.Graphviz
{
    /// <summary>
    ///     Provides extension methods to print <see cref="Network" /> objects with community information to image files using
    ///     Graphviz.
    /// </summary>
    public static class Extensions
    {
        #region Static Fields & Constants

        private const GraphvizImageType GRAPHVIZ_IMAGE_TYPE = GraphvizImageType.Pdf;
        private const int VERTEX_FONT_SIZE = 14;
        private const int EDGE_FONT_SIZE = 9;
        private const string FONT_NAME = "Candara"; //"Tahoma";
        private const GraphvizVertexShape VERTEX_SHAPE = GraphvizVertexShape.Circle;
        private const int STROKE_COLOR = 60;
        private const int EDGE_ALPHA = 70;
        private const int FONT_COLOR = 0;
        private const int GRAPHVIZ_TIMEOUT_MS = 2000;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Saves the given <see cref="Network" /> to an image file.
        /// </summary>
        /// <param name="communityAlg">The root node of the tree to be saved.</param>
        /// <param name="basePath">The path in which to save the given tree</param>
        /// <param name="fileName">The name of the image file to be saved (without extension)</param>
        /// <param name="showLabels">Whether to show nodes' labels.</param>
        /// <param name="imageType">The type of image file in which to save the tree.</param>
        /// <param name="paletteGenerator">The color palette generator used to represent the different communities.</param>
        /// <param name="timeout">The maximum time to wait for Graphviz to create the image file.</param>
        /// <returns>The path to the file where the tree image file was saved.</returns>
        public static string ToGraphvizFile(
            this CommunityAlgorithm communityAlg, string basePath, string fileName,
            bool showLabels = true, GraphvizImageType imageType = GRAPHVIZ_IMAGE_TYPE,
            PaletteGenerator paletteGenerator = null, int timeout = GRAPHVIZ_TIMEOUT_MS)
        {
            var filePath = Path.Combine(basePath, $"{fileName}.dot");
            if (File.Exists(filePath))
                File.Delete(filePath);

            var graph = GetGraph(communityAlg, showLabels, paletteGenerator);
            var viz = new GraphvizAlgorithm<Node, Edge>(graph) {ImageType = imageType};
            viz.FormatVertex += OnFormatVertex;
            viz.FormatEdge += OnFormatEdge;
            return viz.Generate(new FileDotEngine(timeout), filePath);
        }

        #endregion

        #region Private & Protected Methods

        private static UndirectedGraph<Node, Edge> GetGraph(
            CommunityAlgorithm communityAlg, bool showLabels, PaletteGenerator paletteGenerator)
        {
            // generates community color palette
            paletteGenerator = paletteGenerator ?? TolPalettes.CreateTolDivPalette;
            var colors = paletteGenerator(communityAlg.GetNumberCommunities());

            // creates a graphviz from the network and communities
            var graph = new UndirectedGraph<Node, Edge>();

            // adds nodes with community info
            var nodes = new Node[communityAlg.Size];
            foreach (var nodeID in communityAlg.Network.Vertices)
            {
                var node = new Node(nodeID, communityAlg.NodesCommunities[nodeID])
                           {Color = colors[communityAlg.NodesCommunities[nodeID]], ShowLabel = showLabels};
                nodes[nodeID] = node;
                graph.AddVertex(node);
            }

            // adds edges from connections
            foreach (var conn in communityAlg.Network.Edges)
                graph.AddEdge(new Edge(nodes[conn.Source], nodes[conn.Target], conn.Weight) {ShowLabel = showLabels});
            return graph;
        }

        private static void OnFormatEdge(object sender, FormatEdgeEventArgs<Node, Edge> e)
        {
            // transparent edges
            var formatter = e.EdgeFormatter;
            formatter.StrokeGraphvizColor = new GraphvizColor(EDGE_ALPHA, 0, 0, 0);
            formatter.Font = new GraphvizFont(FONT_NAME, EDGE_FONT_SIZE);
            formatter.FontGraphvizColor = new GraphvizColor(EDGE_ALPHA, FONT_COLOR, FONT_COLOR, FONT_COLOR);
            formatter.Label.Value = e.Edge.ShowLabel
                ? e.Edge.Weight.ToString("0.##", CultureInfo.InvariantCulture)
                : string.Empty;
        }

        private static void OnFormatVertex(object sender, FormatVertexEventArgs<Node> e)
        {
            var formatter = e.VertexFormatter;
            formatter.Shape = VERTEX_SHAPE;
            formatter.Style = GraphvizVertexStyle.Filled;
            formatter.Font = new GraphvizFont(FONT_NAME, VERTEX_FONT_SIZE);
            formatter.FontColor = new GraphvizColor(255, FONT_COLOR, FONT_COLOR, FONT_COLOR);
            formatter.Label = e.Vertex.ShowLabel ? e.Vertex.IdNum.ToString() : string.Empty;
            formatter.StrokeColor = new GraphvizColor(255, STROKE_COLOR, STROKE_COLOR, STROKE_COLOR);
            formatter.FillColor = e.Vertex.Color;
        }

        #endregion

        #region Nested type: FileDotEngine

        internal sealed class FileDotEngine : IDotEngine
        {
            #region Fields

            private readonly int _timeout;

            #endregion

            #region Constructors

            public FileDotEngine(int timeout)
            {
                this._timeout = timeout;
            }

            #endregion

            #region Public Methods

            public string Run(GraphvizImageType imageType, string dot, string outputFileName)
            {
                // writes .dot file with the tree and converts it to an image
                File.WriteAllText(outputFileName, dot);
                var args = $"\"{outputFileName}\" -T{imageType.GetOutputFormatStr()} -O";
                var processInfo = new ProcessStartInfo("dot", args)
                                  {
                                      UseShellExecute = false,
                                      RedirectStandardOutput = true,
                                      CreateNoWindow = true
                                  };
                var process = Process.Start(processInfo);
                process?.WaitForExit(this._timeout);
                return outputFileName;
            }

            #endregion
        }

        #endregion
    }
}