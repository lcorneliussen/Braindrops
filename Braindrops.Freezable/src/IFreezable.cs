using System;

namespace Braindrops.Freezable
{
    /// <summary>
    /// Let you freeze a object for any write actions through properties
    /// or methods marked with <see cref="FreezesAttribute"/>.
    /// If frozen, all values returned by any property or method of
    /// Freezable that follow IFreezable, IFreezeme or are marked with
    /// <see cref="FreezeResultsAttribute"/> should also be frozen.
    /// </summary>
    public interface IFreezable
    {
        bool IsFrozen { get; }
        object BaseObject { get; }
        void Freeze();

        T CloneUnfrozen<T>();

        object CloneUnfrozen(Type targetType);
    }
}