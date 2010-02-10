namespace Braindrops.Freezable
{
    /// <summary>
    /// Convenience methods for all <see cref="IFreezeme"/>.
    /// Maps directly to <see cref="Freezer"/>.
    /// </summary>
    public static class FreezableExtensions
    {
        public static bool CanFreeze(this IFreezeme freezable)
        {
            return Freezer.CanFreeze(freezable);
        }

        public static void Freeze(this IFreezeme freezable)
        {
            Freezer.Freeze(freezable);
        }

        public static bool IsFrozen(this IFreezeme freezable)
        {
            return Freezer.IsFrozen(freezable);
        }

        public static T MakeFreezable<T>(this T freezable)
            where T : IFreezeme
        {
            return (T) Freezer.MakeFreezable(freezable, typeof (T));
        }

        public static T CloneUnfrozen<T>(this T freezable)
            where T : IFreezeme
        {
            return (T) Freezer.CloneUnfrozen(freezable, typeof (T));
        }
    }
}