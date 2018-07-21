// ------------------------------------------
// <copyright file="Node.cs" company="Pedro Sequeira">
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

using Newtonsoft.Json;

namespace Comuna.D3
{
    /// <summary>
    ///     Represents a node structure used to save a node of a <see cref="Network" /> to a D3 json file.
    /// </summary>
    [JsonObject]
    public class Node
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Node" /> with the given ID.
        /// </summary>
        /// <param name="id">The id of the node.</param>
        /// <param name="community">The id of the community of this node</param>
        public Node(uint id, uint community = 0)
        {
            this.IdNum = id;
            this.Community = community;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets the ID of the community of this node.
        /// </summary>
        [JsonProperty(Constants.COMMUNITY_PROP)]
        public uint Community { get; set; }

        /// <summary>
        ///     Gets or sets the ID of this node.
        /// </summary>
        [JsonIgnore]
        public string ID { get => this.IdNum.ToString(); set => this.IdNum = uint.Parse(value); }

        /// <summary>
        ///     Gets or sets the ID of this node.
        /// </summary>
        [JsonProperty(Constants.ID_PROP)]
        public uint IdNum { get; set; }

        #endregion
    }
}