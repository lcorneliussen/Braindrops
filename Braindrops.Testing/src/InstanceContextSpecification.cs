using System;

namespace Braindrops.Testing
{
    public abstract class InstanceContextSpecification<SystemUnderTest> : StaticContextSpecification
    {
        protected SystemUnderTest _sut;

        /// <summary>
        /// Create system under test.
        /// </summary>
        /// <returns></returns>
        protected abstract SystemUnderTest GivenInstance();

        protected abstract override void GivenContext();

        protected override void InitializeSystemUnderTest()
        {
            _sut = GivenInstance();
        }

        protected override void CleanUpContext()
        {
            try
            {
                if (_sut != null && _sut is IDisposable)
                    ((IDisposable) _sut).Dispose();
            }
            finally
            {
                base.CleanUpContext();
            }
        }
    }
}