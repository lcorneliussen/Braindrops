namespace Braindrops.Variance.Tests
{
    internal class Reading<T> : IReading<T>
    {
        private T _t;

        public Reading(T t)
        {
            _t = t;
        }

        public object Object
        {
            set { _t = (T) value; }
        }

        #region IReading<T> Members

        public T Get()
        {
            return _t;
        }

        #endregion
    }
}