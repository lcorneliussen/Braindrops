using Braindrops.Variance;

namespace Braindrops.VariantPubSub.Test
{
    [Variance(VarianceDirection.Out)]
    public interface IEvent<T>
    {
        T Data { get; }
    }

    public class Event<T> : IEvent<T>
    {
        public Event(T data)
        {
            Data = data;
        }

        #region IEvent<T> Members

        public T Data { get; private set; }

        #endregion
    }

    public static class PrivateEvent
    {
        public static IEvent<T> Create<T>(T data)
        {
            return new Event<T>(data);
        }

        #region Nested type: Event

        private class Event<T> : IEvent<T>
        {
            public Event(T data)
            {
                Data = data;
            }

            #region IEvent<T> Members

            public T Data { get; private set; }

            #endregion
        }

        #endregion
    }
}