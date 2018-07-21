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
//    Project: Comuna
//    Last updated: 06/14/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using QuickGraph;

namespace Comuna
{
    /// <summary>
    ///     Represents an undirected connection in an <see cref="Network" /> with an associated weight.
    /// </summary>
    public class Connection : IUndirectedEdge<uint>, IEquatable<Connection>
    {
        #region Static Fields & Constants

        private const double COMPARE_EPSILON = 1E-6;

        #endregion

        #region Fields

        private readonly int _hashCode;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Connection" /> linking the given source and target with the
        ///     associated weight.
        /// </summary>
        /// <param name="source">The source of the connection.</param>
        /// <param name="target">The target of the connection.</param>
        /// <param name="weight">The weight associated with the connection.</param>
        public Connection(uint source, uint target, double weight = 1)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
            this._hashCode = this.ProduceHashCode();
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the weight associated with this connection.
        /// </summary>
        public double Weight { get; }

        /// <summary>
        ///     Gets the source node.
        /// </summary>
        public uint Source { get; }

        /// <summary>
        ///     Gets the target node.
        /// </summary>
        public uint Target { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return !(obj is null) &&
                   (ReferenceEquals(this, obj) || obj.GetType() == this.GetType() &&
                    this.Equals((Connection) obj));
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
        public static bool operator ==(Connection left, Connection right) => Equals(left, right);

        /// <summary>
        ///     Tests whether the two connections are different (not equal).
        /// </summary>
        /// <param name="left">The first connection.</param>
        /// <param name="right">The second connection.</param>
        /// <returns>A <see cref="bool" /> indicating whether the two connections are different.</returns>
        public static bool operator !=(Connection left, Connection right) => !Equals(left, right);

        /// <summary>
        ///     Tests whether this connection is equal to another one.
        /// </summary>
        /// <param name="other">The other connection.</param>
        /// <returns>A <see cref="bool" /> indicating whether this connection is equal to the other one.</returns>
        public bool Equals(Connection other)
        {
            return !(other is null) &&
                   (ReferenceEquals(this, other) ||
                    this._hashCode == other._hashCode &&
                    Math.Abs(this.Weight - other.Weight) < COMPARE_EPSILON &&
                    (Equals(this.Source, other.Source) && Equals(this.Target, other.Target) ||
                     Equals(this.Source, other.Target) && Equals(this.Target, other.Source)));
        }

        #endregion

        #region Private & Protected Methods

        private int ProduceHashCode()
        {
            unchecked
            {
                var hashCode = this.Weight.GetHashCode();
                var source = this.Source < this.Target ? this.Source : this.Target;
                var target = this.Source < this.Target ? this.Target : this.Source;
                hashCode = (hashCode * 397) ^ source.GetHashCode();
                hashCode = (hashCode * 397) ^ target.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}