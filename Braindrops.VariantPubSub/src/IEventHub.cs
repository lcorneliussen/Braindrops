using System;
using System.Collections.Generic;

namespace Braindrops.VariantPubSub
{
    public interface IEventHub
    {
        void Register(ISubscriber subscriber);

        void Unregister(ISubscriber subscriber);

        IEnumerable<ISubscriber<EventType>> GetSubscribers<EventType>();

        IEnumerable<ISubscriber<EventType>> GetSubscribers<EventType>(Type expectedEventType);

        void Publish<AsType>(object eventData);

        void Publish(Type asEventType, object eventData);
    }
}