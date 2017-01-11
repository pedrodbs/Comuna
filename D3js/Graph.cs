// ------------------------------------------
// Graph.cs, Learning.Tests.SocialImportanceSimulations
// 
// Created by Pedro Sequeira, 2015/06/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;

namespace CommunityGrapher.Domain.D3
{
    public class Graph
    {
        public Graph()
        {
            this.Nodes = new List<Node>();
            this.Links = new List<Link>();
        }

        [JsonProperty("n")]
        public List<Node> Nodes { get; private set; }

        [JsonProperty("l")]
        public List<Link> Links { get; private set; }
    }
}