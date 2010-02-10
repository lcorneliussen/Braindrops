namespace Braindrops.Variance.Tests
{
    internal class Writing<T> : IWriting<T>
    {
        private T _t;

        #region IWriting<T> Members

        public object Object
        {
            get { return _t; }
        }

        public void Set(T item)
        {
            _t = item;
        }

        #endregion
    }
}