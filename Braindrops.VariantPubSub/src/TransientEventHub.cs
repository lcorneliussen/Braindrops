using System;
using System.Collections.Generic;
using System.Linq;
using Braindrops.Reflection;
using Braindrops.Variance;
using Minimod.PrettyTypeSignatures;

namespace Braindrops.VariantPubSub
{
    public class TransientEventHub : IEventHub
    {
        /*private static readonly ILog _log = LogManager.GetLogger(
            typeof(TransientEventHub));*/

        private readonly SubscriptionRegistry _registry = new SubscriptionRegistry();

        #region IEventHub Members

        public void Register(ISubscriber subscriber)
        {
            IEnumerable<Type> interfaces = subscriber.GetType().GetInterfaces()
                .Where(i => i.IsGenericTypeOf(typeof(ISubscriber<>)));

            foreach (Type interf in interfaces)
            {
                _registry.Register(
                                      new RegistryEntry
                                          {
                                              Guid = Guid.NewGuid(),
                                              Subscriber = subscriber,
                                              EventType = interf.GetGenericArgumentsFor(typeof(ISubscriber<>))[0]
                                          });
            }
        }

        public void Unregister(ISubscriber subscriber)
        {
            _registry.RemoveAll(subscriber);
        }

        public IEnumerable<ISubscriber<EventType>> GetSubscribers<EventType>()
        {
            return GetSubscribers<EventType>(typeof(EventType));
        }

        public IEnumerable<ISubscriber<EventType>> GetSubscribers<EventType>(Type expectedEventType)
        {
            return _registry.Resolve(expectedEventType).Select(r => r.AsSubscriberOf<EventType>(expectedEventType));
        }

        public void Publish(Type asEventType, object eventData)
        {
            if (asEventType == null) throw new ArgumentNullException("asEventType");
            if (eventData == null) throw new ArgumentNullException("eventData");

            if (!eventData.IsVariantTo(asEventType))
            {
                string message = string.Format(
                                                  "The instance eventData of type ({0}) is not variant to asEventType ({1})",
                                                  eventData.GetType().GetPrettyName(),
                                                  asEventType.GetPrettyName());

                throw new ArgumentException(message);
            }

            // Type actualEventType = eventData.GetType();
            ISubscriber<object>[] subscribers = GetSubscribers<object>(asEventType).ToArray();

            //_log.Debug(m => m("Publishing '{0}' to {1} subscribers.", eventData, subscribers.Length));

            foreach (var subscriber in subscribers)
            {
                //_log.Debug(m => m("Publishing '{0}' to subscriber {1}.", eventData, subscriber.DisplayName));

                try
                {
                    subscriber.OnPublished(eventData);
                }
                catch (Exception e)
                {
                    // Exception wird nach oben durchgereicht, damit alles zurückgedreht(rollback) wird
                    // und sie als Fehlermeldung im aufrufenden Task gelangt.
                    // TODO: Custom Exception
                    throw new Exception(string.Format("Error occured on publishing '{0}' to subscriber {1}.", eventData,
                                                      subscriber.DisplayName));
                }
            }
        }

        public void Publish<AsType>(object eventData)
        {
            Publish(typeof(AsType), eventData);
        }

        #endregion
    }
}