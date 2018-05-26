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
//    Project: CommunityGrapher.D3
//    Last updated: 05/25/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CommunityGrapher.D3
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
        /// <param name="prevCommunities">An optional argument containing the communities of each node in a previous iteration.</param>
        /// <param name="formatting">The Json file formatting.</param>
        public static void SaveD3GraphFile(
            this CommunityAlgorithm communityAlg, string filePath,
            uint[] prevCommunities = null, Formatting formatting = Formatting.None)
        {
            // creates graph
            var graph = GetGraph(communityAlg, prevCommunities);

            // todo change this to support evolution of graphs (needs structure)
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            using (var tw = new JsonTextWriter(sw) {Formatting = formatting})
                new JsonSerializer().Serialize(tw, graph);
        }

        #endregion

        #region Private & Protected Methods

        private static Graph GetGraph(CommunityAlgorithm communityAlg, uint[] prevCommunities)
        {
            var graph = new Graph();
            var network = communityAlg.Network;

            //adds links from community graph
            foreach (var edge in network.Edges)
                graph.Links.Add(new Link {Source = edge.Source, Target = edge.Target, Value = edge.Weight});

            //checks updated community for each node and adds corresponding graph nodes
            for (var i = 0u; i < network.VertexCount; i++)
            {
                var community = communityAlg.NodeCommunity[i];
                if (prevCommunities == null || !prevCommunities[i].Equals(community))
                    graph.Nodes.Add(new Node(i)
                                    {
                                        Community = community,

                                        //todo remove this
                                        HexColor = string.Empty
                                    });
            }

            return graph;
        }

        #endregion
    }
}