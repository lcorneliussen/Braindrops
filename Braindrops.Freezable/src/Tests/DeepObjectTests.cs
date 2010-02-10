using System;
using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.Freezable.Tests
{
    [TestFixture]
    public class DeepObjectTests : BaseTest
    {
        [Test]
        public void DeepClone()
        {
            var deep = new FreezeInside("root")
                           {
                               Single = new FreezeInside("single"),
                               Multiple = new[] {new FreezeInside("item1"), new FreezeInside("item2")}
                           };

            var frozen = deep.MakeFreezable<IFreezeInside>();
            frozen.Freeze();

            IFreezeInside clone = frozen.CloneUnfrozen();

            clone.Single.IsFrozen().ShouldBeFalse();

            clone.Multiple[0].IsFrozen().ShouldBeFalse();
            clone.Multiple[1].IsFrozen().ShouldBeFalse();
        }

        [Test]
        public void DeepFreeze()
        {
            var deep = new FreezeInside("root")
                           {
                               Single = new FreezeInside("single"),
                               Multiple = new[] {new FreezeInside("item1"), new FreezeInside("item2")}
                           };

            var frozen = deep.MakeFreezable<IFreezeInside>();
            frozen.Freeze();

            frozen.Single.IsFrozen().ShouldBeTrue();

            frozen.Multiple[0].IsFrozen().ShouldBeTrue();
            frozen.Multiple[1].IsFrozen().ShouldBeTrue();
        }
    }

    [Serializable]
    public class FreezeInside : IFreezeInside
    {
        public FreezeInside(string name)
        {
            Name = name;
        }

        #region IFreezeInside Members

        public string Name { get; set; }
        public IFreezeInside Single { get; set; }
        public IFreezeInside[] Multiple { get; set; }

        #endregion
    }

    public interface IFreezeInside : IFreezeme
    {
        string Name { get; set; }

        IFreezeInside Single { get; set; }

        IFreezeInside[] Multiple { get; set; }
    }
}