using System;
using Braindrops.Variance;

namespace Braindrops.VariantPubSub
{
    internal class RegistryEntry
    {
        public Guid Guid { get; set; }

        public Type EventType { get; set; }

        public ISubscriber Subscriber { get; set; }

        public ISubscriber<EventType> AsSubscriberOf<EventType>()
        {
            return Subscriber.AsVariant<ISubscriber<EventType>>();
        }

        public ISubscriber<EventType> AsSubscriberOf<EventType>(Type expectedEventType)
        {
            if (typeof (EventType) == expectedEventType)
            {
                return Subscriber.AsVariant<ISubscriber<EventType>>();
            }

            if (!expectedEventType.IsPublic)
            {
                throw new ArgumentException("The expected Event type must be public.", "expectedEventType");
            }

            return Subscriber
                .AsVariant(typeof (ISubscriber<>).MakeGenericType(expectedEventType)) // restrict
                .AsVariant<ISubscriber<EventType>>(false); // weaken unsafel
        }
    }
}