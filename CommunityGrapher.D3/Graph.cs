// ------------------------------------------
// <copyright file="Graph.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace CommunityGrapher.D3
{
    /// <summary>
    ///     Represents a graph structure used to save a <see cref="Network" /> to a D3 json file.
    /// </summary>
    public class Graph
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets the list of <see cref="Link" /> in this graph.
        /// </summary>
        [JsonProperty("l")]
        public List<Link> Links { get; private set; } = new List<Link>();

        /// <summary>
        ///     Gets or sets the list of <see cref="Node" /> in this graph.
        /// </summary>
        [JsonProperty("n")]
        public List<Node> Nodes { get; private set; } = new List<Node>();

        #endregion
    }
}