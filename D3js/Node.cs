// ------------------------------------------
// Node.cs, PS.CommunityGrapher
// 
// Created by Pedro Sequeira, 2015/06/26
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Newtonsoft.Json;

namespace CommunityGrapher.Domain.D3
{
    public class Node
    {
        public Node(uint id)
        {
            this.IdNum = id;
        }

        [JsonProperty("c")]
        public uint Community { get; set; }

        [JsonIgnore]
        public string ID
        {
            get { return this.IdNum.ToString(); }
            set { this.IdNum = uint.Parse(value); }
        }

        [JsonProperty("i")]
        public uint IdNum { get; set; }

        [JsonProperty("h", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string HexColor { get; set; }
    }
}