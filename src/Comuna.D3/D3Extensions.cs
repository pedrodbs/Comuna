// ------------------------------------------
// <copyright file="D3Extensions.cs" company="Pedro Sequeira">
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
//    Project: Comuna.D3
//    Last updated: 05/26/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Comuna.D3
{
    /// <summary>
    ///     Contains extensions for <see cref="CommunityAlgorithm" /> objects to enable export to D3.js graph files.
    /// </summary>
    public static class D3Extensions
    {
        #region Public Methods

        /// <summary>
        ///     Saves the network of the given <see cref="CommunityAlgorithm" /> to a d3.js graph file.
        /// </summary>
        /// <param name="communityAlg">The network to be saved to a graph file.</param>
        /// <param name="filePath">The path to the file in which to save the network graph.</param>
        /// <param name="formatting">The Json file formatting.</param>
        public static void ToD3GraphFile(
            this CommunityAlgorithm communityAlg, string filePath, Formatting formatting = Formatting.None)
        {
            // creates writers
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            using (var tw = new JsonTextWriter(sw) {Formatting = formatting})
            {
                tw.WriteStartObject();

                // writes num time-steps
                tw.WritePropertyName(Constants.NUM_TIME_STEPS_PROP);
                tw.WriteValue(1);

                // opens graphs
                tw.WritePropertyName(Constants.GRAPH_LIST_PROP);
                tw.WriteStartArray();

                // creates a new graph structure with the network and communities info 
                var graph = new Graph();

                // adds links from network
                foreach (var edge in communityAlg.Network.Edges)
                    graph.Links.Add(new Link {Source = edge.Source, Target = edge.Target, Value = edge.Weight});

                // adds nodes with community info
                for (var i = 0u; i < communityAlg.Network.VertexCount; i++)
                    graph.Nodes.Add(new Node(i, communityAlg.NodesCommunities[i]));

                // writes graph and closes json
                tw.WriteRawValue(JsonConvert.SerializeObject(graph, formatting));
                tw.WriteEndArray();
                tw.WriteEndObject();
            }
        }

        #endregion
    }
}