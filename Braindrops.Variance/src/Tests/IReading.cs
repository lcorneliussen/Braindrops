namespace Braindrops.Variance.Tests
{
    [Variance(VarianceDirection.Out)]
    public interface IReading<T>
    {
        T Get();
    }
}