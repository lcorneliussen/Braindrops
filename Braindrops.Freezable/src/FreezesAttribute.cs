using System;

namespace Braindrops.Freezable
{
    /// <summary>
    /// Specifies if a property or method should freeze, when the owning object is frozen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class FreezesAttribute : Attribute
    {
    }
}