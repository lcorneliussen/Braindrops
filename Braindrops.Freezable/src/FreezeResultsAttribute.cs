using System;

namespace Braindrops.Freezable
{
    /// <summary>
    /// Specifies if a property or method should freeze the results, when the owning object is frozen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class FreezeResultsAttribute : Attribute
    {
    }
}