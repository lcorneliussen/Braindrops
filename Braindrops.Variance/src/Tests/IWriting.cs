namespace Braindrops.Variance.Tests
{
    [Variance(VarianceDirection.In)]
    public interface IWriting<T>
    {
        object Object { get; }
        void Set(T item);
    }
}