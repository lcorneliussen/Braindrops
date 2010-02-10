// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Random Braindrops by Lars Corneliussen" file="VarianceDirection.cs">
//   Copyright (c) Random Braindrops by Lars Corneliussen
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Braindrops.Variance
{
    /// <summary>
    /// Declares a generic type parameter as read-only or write-only. 
    /// Is used for generic type <see cref="Variance"/>.
    /// </summary>
    public enum VarianceDirection
    {
        /// <summary>
        /// Makes the generic argument contra-variant. Use for write-only arguments.
        /// </summary>
        In,

        /// <summary>
        /// Makes the generic argument co-variant. Use for read-only arguments.
        /// </summary>
        Out
    }
}