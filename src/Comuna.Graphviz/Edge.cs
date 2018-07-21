// ------------------------------------------
// <copyright file="Edge.cs" company="Pedro Sequeira">
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
//    Project: Comuna.Graphviz
//    Last updated: 06/15/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using QuickGraph;

namespace Comuna.Graphviz
{
    /// <summary>
    ///     Represents an edge structure used to save a connection of a <see cref="Network" /> to a Graphviz dot file.
    /// </summary>
    public class Edge : IUndirectedEdge<Node>
    {
        #region Fields

        private readonly int _hashCode;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Edge" /> linking the given source and target with the
        ///     associated weight.
        /// </summary>
        /// <param name="source">The source of the connection.</param>
        /// <param name="target">The target of the connection.</param>
        /// <param name="weight">The weight associated with the connection.</param>
        public Edge(Node source, Node target, double weight = 1)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
            this._hashCode = this.ProduceHashCode();
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets a value indicating whether to show the edge's label.
        /// </summary>
        public bool ShowLabel { get; set; }

        /// <summary>
        ///     Gets the weight associated with this connection.
        /// </summary>
        public double Weight { get; }

        /// <summary>
        ///     Gets the source node.
        /// </summary>
        public Node Source { get; }

        /// <summary>
        ///     Gets the target node.
        /// </summary>
        public Node Target { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return !(obj is null) &&
                   (ReferenceEquals(this, obj) || obj.GetType() == this.GetType() &&
                    this.Equals((Edge) obj));
        }

        /// <inheritdoc />
        public override int GetHashCode() => this._hashCode;

        /// <inheritdoc />
        public override string ToString() => $"{this.Source}-{this.Target} ({this.Weight:0.00})";

        #endregion

        #region Public Methods

        /// <summary>
        ///     Tests whether the two connections are equal.
        /// </summary>
        /// <param name="left">The first connection.</param>
        /// <param name="right">The second connection.</param>
        /// <returns>A <see cref="bool" /> indicating whether the two connections are equal.</returns>
        public static bool operator ==(Edge left, Edge right) => Equals(left, right);

        /// <summary>
        ///     Tests whether the two connections are different (not equal).
        /// </summary>
        /// <param name="left">The first connection.</param>
        /// <param name="right">The second connection.</param>
        /// <returns>A <see cref="bool" /> indicating whether the two connections are different.</returns>
        public static bool operator !=(Edge left, Edge right) => !Equals(left, right);

        /// <summary>
        ///     Tests whether this connection is equal to another one.
        /// </summary>
        /// <param name="other">The other connection.</param>
        /// <returns>A <see cref="bool" /> indicating whether this connection is equal to the other one.</returns>
        public bool Equals(Edge other)
        {
            return !(other is null) &&
                   (ReferenceEquals(this, other) ||
                    this._hashCode == other._hashCode &&
                    (Equals(this.Source, other.Source) && Equals(this.Target, other.Target) ||
                     Equals(this.Source, other.Target) && Equals(this.Target, other.Source)));
        }

        #endregion

        #region Private & Protected Methods

        private int ProduceHashCode()
        {
            unchecked
            {
                var source = this.Source.IdNum < this.Target.IdNum ? this.Source : this.Target;
                var target = this.Source.IdNum < this.Target.IdNum ? this.Target : this.Source;
                return (source.GetHashCode() * 397) ^ target.GetHashCode();
            }
        }

        #endregion
    }
}