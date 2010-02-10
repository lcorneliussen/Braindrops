using System;
using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.Freezable.Tests
{
    [TestFixture]
    public class FreezeMethodTests : BaseTest
    {
        [Test, ExpectedException(typeof (Exception))]
        public void Test()
        {
            var freezable = new FreezeMethod().MakeFreezable<IFreezeMethod>();
            freezable.Freeze();
            freezable.MyMethod();
        }
    }

    public class FreezeMethod : IFreezeMethod
    {
        #region IFreezeMethod Members

        public void MyMethod()
        {
        }

        #endregion
    }

    public interface IFreezeMethod : IFreezeme
    {
        [Freezes]
        void MyMethod();
    }
}