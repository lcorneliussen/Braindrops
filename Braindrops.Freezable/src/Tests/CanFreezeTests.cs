using System;
using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.Freezable.Tests
{
    [TestFixture]
    public class CanFreezeTests : BaseTest
    {
        [Test]
        public void CanFreeze_MakeFreezable_ShouldBeTrue()
        {
            var freezable = (IFreezeme) new Freezable();
            IFreezeme reallyFreezable = freezable.MakeFreezable();
            reallyFreezable.CanFreeze().ShouldBeTrue();
        }

        [Test]
        public void CanFreeze_Unprepared_ShouldBeFalse()
        {
            var freezable = (IFreezeme) new Freezable();
            freezable.CanFreeze().ShouldBeFalse();
        }

        [Test]
        public void Freeze_MakeFreezable_ShouldBeFrozen()
        {
            var freezable = (IFreezeme) new Freezable();
            IFreezeme reallyFreezable = freezable.MakeFreezable();
            reallyFreezable.Freeze();
            reallyFreezable.IsFrozen().ShouldBeTrue();
        }

        [Test]
        public void Freeze_MakeFreezable_ShouldSuceed()
        {
            var freezable = (IFreezeme) new Freezable();
            IFreezeme reallyFreezable = freezable.MakeFreezable();
            reallyFreezable.Freeze();
        }

        [Test, ExpectedException(typeof (Exception))]
        public void Freeze_Unprepared_ShouldFail()
        {
            var freezable = (IFreezeme) new Freezable();
            freezable.Freeze();
        }

        [Test]
        public void IsFrozen_Unprepared_ShouldBeFalse()
        {
            var freezable = (IFreezeme) new Freezable();
            freezable.IsFrozen().ShouldBeFalse();
        }
    }
}