// ------------------------------------------
// <copyright file="CommunityTracker.cs" company="Pedro Sequeira">
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

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Comuna.D3
{
    /// <summary>
    ///     A class that enables tracking the evolution of the communities in a <see cref="Network" /> over time while writing
    ///     the changes to a D3 json file. The <see cref="Network" />'s connections and the <see cref="CommunityAlgorithm" />
    ///     are assumed to be updated outside of the tracker.
    /// </summary>
    public class CommunityTracker : IDisposable
    {
        #region Fields

        private readonly Formatting _formatting;
        private readonly JsonTextWriter _jsonWriter;
        private bool _disposed;
        private uint[] _prevCommunities;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="CommunityTracker" /> with the given algorithm and capacity.
        /// </summary>
        /// <param name="communityAlg">
        ///     The community algorithm containing the <see cref="Network" /> used to update the communities.
        /// </param>
        /// <param name="filePath">The path to the file in which to save the network graph.</param>
        /// <param name="maxUpdates">The maximum number of updates to the communities to be tracked.</param>
        /// <param name="formatting">The Json file formatting.</param>
        public CommunityTracker(
            CommunityAlgorithm communityAlg, string filePath, uint maxUpdates, Formatting formatting = Formatting.None)
        {
            this.CommunityAlg = communityAlg;
            this.FilePath = filePath;
            this._formatting = formatting;

            //writes header of json file
            this._jsonWriter =
                new JsonTextWriter(new StreamWriter(new FileStream(filePath, FileMode.Create), Encoding.UTF8))
                {
                    Formatting = formatting,
                    CloseOutput = true
                };

            this._jsonWriter.WriteStartObject();

            // writes num time-steps
            this._jsonWriter.WritePropertyName(Constants.NUM_TIME_STEPS_PROP);
            this._jsonWriter.WriteValue(maxUpdates);

            // opens graphs
            this._jsonWriter.WritePropertyName(Constants.GRAPH_LIST_PROP);
            this._jsonWriter.WriteStartArray();
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the community algorithm containing the <see cref="Network" /> used to update the communities.
        /// </summary>
        public CommunityAlgorithm CommunityAlg { get; }

        /// <summary>
        ///     Gets the path to the D3 json file where the network graphs are saved.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        ///     Gets the network graph whose communities are tracked by this instance.
        /// </summary>
        public Network Network => this.CommunityAlg.Network;

        #endregion

        #region Public Methods

        public void Update()
        {
            // creates a new graph structure with 
            var graph = new Graph();

            // checks and adds links from community graph
            foreach (var connection in this.CommunityAlg.Network.Edges)
                graph.Links.Add(
                    new Link {Source = connection.Source, Target = connection.Target, Value = connection.Weight});

            //checks updated community for each node and adds corresponding graph nodes
            for (var i = 0u; i < this.CommunityAlg.Network.VertexCount; i++)
            {
                var community = this.CommunityAlg.NodesCommunities[i];
                if (this._prevCommunities == null || !this._prevCommunities[i].Equals(community))
                    graph.Nodes.Add(new Node(i, community));
            }

            // stores copies of previous information
            this._prevCommunities = (uint[]) this.CommunityAlg.NodesCommunities.Clone();

            // writes graph to file
            this._jsonWriter.WriteRawValue(JsonConvert.SerializeObject(graph, this._formatting));
        }

        /// <summary>
        ///     Disposes of this tracker by closing the D3 json file.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed) throw new ObjectDisposedException("This tracker has already been disposed");
            this._jsonWriter.WriteEndArray();
            this._jsonWriter.WriteEndObject();
            this._jsonWriter.Close();
            this._prevCommunities = null;
            this._disposed = true;
        }

        #endregion
    }
}