using QuickGraph;

namespace CommunityGrapher.Domain
{
    public class NetworkEdge : IUndirectedEdge<uint>
    {
        public NetworkEdge(uint source, uint target, double weight = 1)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
        }

        public double Weight { get; private set; }

        #region IUndirectedEdge<uint> Members

        public uint Source { get; private set; }
        public uint Target { get; private set; }

        #endregion
    }
}