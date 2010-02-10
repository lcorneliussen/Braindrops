using System;
using System.Collections.Generic;
using System.Linq;

namespace Braindrops.VariantPubSub
{
    internal class SubscriptionRegistry
    {
        /*private static readonly ILog _log = LogManager.GetLogger(
            typeof(SubscriptionRegistry));*/

        private readonly IDictionary<Guid, RegistryEntry> _byGuid = new Dictionary<Guid, RegistryEntry>();

        private readonly IDictionary<ISubscriber, RegistryEntry> _bySubscriber =
            new Dictionary<ISubscriber, RegistryEntry>();

        private readonly IDictionary<Type, IList<RegistryEntry>> _byType = new Dictionary<Type, IList<RegistryEntry>>();
        private readonly object _registering = new object();

        public void Register(RegistryEntry entry)
        {
            /*_log.Debug(
                m => m(
                         "Registering subscriber {0} of type {1} to {2}.",
                         entry.Subscriber.DisplayName,
                         entry.Subscriber.GetType().GetDisplayName(),
                         entry.EventType.GetDisplayName()));*/

            lock (_registering)
            {
                _byGuid.Add(entry.Guid, entry);
                _bySubscriber.Add(entry.Subscriber, entry);
                indexByType(entry);
            }
        }

        public void RemoveAll(ISubscriber subscriber)
        {
            lock (_registering)
            {
                RegistryEntry entry;
                if (_bySubscriber.TryGetValue(subscriber, out entry))
                {
                    _byGuid.Remove(entry.Guid);
                    removeByType(entry);
                }
            }
        }

        private void removeByType(RegistryEntry entry)
        {
            Type type = entry.EventType;
            if (_byType.ContainsKey(type))
            {
                _byType[type].Remove(entry);
            }
        }

        private void indexByType(RegistryEntry entry)
        {
            Type type = entry.EventType;
            if (!_byType.ContainsKey(type))
            {
                _byType[type] = new List<RegistryEntry>();
            }

            _byType[type].Add(entry);
        }

        public RegistryEntry[] Resolve(Type actual)
        {
            IEnumerable<Type> keys = _byType.Keys.Where(k => matches(k, actual));

            return keys.SelectMany(k => _byType[k]).Distinct().ToArray();
        }

        private bool matches(Type registration, Type actual)
        {
            return Variance.Variance.IsVariant(actual, registration);
        }
    }
}