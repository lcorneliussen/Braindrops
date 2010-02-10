// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Random Braindrops by Lars Corneliussen" file="VarianceAttribute.cs">
//   Copyright (c) Random Braindrops by Lars Corneliussen
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;

namespace Braindrops.Variance
{
    /// <summary>
    /// Configures generic intefaces for <see cref="Variance"/>.
    /// For each generic type on the target interface, the attribut must 
    /// define a direction specifying either co- or contra-variance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class VarianceAttribute : Attribute
    {
        public VarianceAttribute(params VarianceDirection[] genericTypeUsages)
        {
            GenericTypeUsages = genericTypeUsages;
        }

        public VarianceDirection[] GenericTypeUsages { get; private set; }
    }
}