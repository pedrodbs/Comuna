// ------------------------------------------
// <copyright file="Connection.cs" company="Pedro Sequeira">
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
//    Project: CommunityGrapher
//    Last updated: 05/25/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using QuickGraph;

namespace CommunityGrapher
{
    /// <summary>
    ///     Represents an undirected edge of a <see cref="Network" />.
    /// </summary>
    public class Connection : IUndirectedEdge<uint>
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Connection" /> linking the given source and target with the associated weight.
        /// </summary>
        /// <param name="source">The source of the connection.</param>
        /// <param name="target">The target of the connection.</param>
        /// <param name="weight">The weight associated with the connection.</param>
        public Connection(uint source, uint target, double weight = 1)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the weight associated with this connection.
        /// </summary>
        public double Weight { get; }

        #endregion

        #region IUndirectedEdge<uint> Members

        /// <summary>
        ///     Gets the source node.
        /// </summary>
        public uint Source { get; }

        /// <summary>
        ///     Gets the target node.
        /// </summary>
        public uint Target { get; }

        #endregion
    }
}