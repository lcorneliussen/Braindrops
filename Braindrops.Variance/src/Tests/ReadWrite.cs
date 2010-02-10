namespace Braindrops.Variance.Tests
{
    internal class ReadWrite<TRead, TWrite> : IWriting<TWrite>, IReading<TRead>
    {
        private object _t;

        #region IReading<TRead> Members

        public TRead Get()
        {
            return (TRead) _t;
        }

        #endregion

        #region IWriting<TWrite> Members

        public object Object
        {
            get { return _t; }
            set { _t = value; }
        }

        public void Set(TWrite item)
        {
            _t = item;
        }

        #endregion
    }
}