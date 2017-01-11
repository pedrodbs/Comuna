// ------------------------------------------
// Link.cs, Learning.Tests.SocialImportanceSimulations
// 
// Created by Pedro Sequeira, 2015/06/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Newtonsoft.Json;

namespace CommunityGrapher.Domain.D3
{
    public class Link
    {
		private double _v;

        [JsonProperty("s")]
        public uint Source { get; set; }

        [JsonProperty("t")]
        public uint Target { get; set; }

        [JsonProperty ("v")]
		public double Value {
			get { return this._v; }
			set { this._v = System.Math.Round(value,2); }
		}
    }
}
