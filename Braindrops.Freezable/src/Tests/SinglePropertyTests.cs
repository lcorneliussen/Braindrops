using System;
using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.Freezable.Tests
{
    [TestFixture]
    public class SinglePropertyTests : BaseTest
    {
        private ISingleProperty prepare()
        {
            var singleDirect = Stub<ISingleProperty>();

            MockSetupDone();

            return singleDirect.MakeFreezable();
        }

        [Test]
        public void Object_MakeFreezable_ShouldWork()
        {
            prepare();
        }

        [Test]
        public void Prepared_FreezeGet_ShouldWork()
        {
            ISingleProperty single = prepare();
            single.Freeze();
            single.Access.ShouldBeFalse();
        }

        [Test]
        [ExpectedException(typeof (Exception))]
        public void Prepared_FreezeSet_ShouldFail()
        {
            ISingleProperty single = prepare();
            single.Freeze();
            single.Access = true;
        }

        [Test]
        public void Prepared_Freeze_ShouldWork()
        {
            ISingleProperty single = prepare();
            single.Freeze();
            single.IsFrozen().ShouldBeTrue();
        }

        [Test]
        public void Prepared_SetFreezeGet_ShouldWork()
        {
            ISingleProperty single = prepare();
            single.Access = true;
            single.Freeze();
            single.Access.ShouldBeTrue();
        }

        [Test]
        public void Prepared_SetGet_ShouldWork()
        {
            ISingleProperty single = prepare();
            single.Access = true;
            single.Access.ShouldBeTrue();
        }
    }

    public class Freezable : IFreezeme
    {
    }

    public interface ISingleProperty : IFreezeme
    {
        bool Access { get; set; }
    }
}