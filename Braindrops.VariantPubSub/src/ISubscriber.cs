using Braindrops.Variance;

namespace Braindrops.VariantPubSub
{
    public interface ISubscriber
    {
        string DisplayName { get; }
    }

    [Variance(VarianceDirection.In)]
    public interface ISubscriber<EventData> : ISubscriber
    {
        void OnPublished(EventData data);
    }
}