using System;
using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.Freezable.Tests
{
    [TestFixture]
    public class CloneTest : BaseTest
    {
        [Test]
        public void Frozen_Clone_ShouldWorkAndBeUnfrozen()
        {
            var single = new SingleProperty().MakeFreezable<ISingleProperty>();

            single.Access = true;
            single.Freeze();

            ISingleProperty clone = single.CloneUnfrozen();
            clone.Access.ShouldBeTrue();
            clone.IsFrozen().ShouldBeFalse();
        }

        [Test]
        public void MadeFreezable_Clone_ShouldWork()
        {
            var single = new SingleProperty().MakeFreezable<ISingleProperty>();

            single.Access = true;

            ISingleProperty clone = single.CloneUnfrozen();
            clone.Access.ShouldBeTrue();
            clone.CanFreeze().ShouldBeTrue();
        }

        [Test]
        public void Unprepared_Clone_ShouldWork()
        {
            var single = new SingleProperty();

            single.Access = true;

            var clone = single.CloneUnfrozen<ISingleProperty>();
            clone.Access.ShouldBeTrue();
            clone.CanFreeze().ShouldBeTrue();
        }
    }

    [Serializable]
    public class SingleProperty : ISingleProperty
    {
        #region ISingleProperty Members

        public bool Access { get; set; }

        #endregion
    }
}