using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Reflection.Tests
{
    [TestFixture]
    public class GetMostSeniorInterfaceTests
    {
        [Test]
        public void InterclassA_for_InterfaceA()
        {
            typeof (InterfaceA).GetMostSpecificInterfaceIn(typeof (InterclassA)).Should().Be.EqualTo(typeof (InterfaceA));
        }

        [Test]
        public void InterclassB_for_InterfaceA()
        {
            typeof (InterfaceA).GetMostSpecificInterfaceIn(typeof (InterclassB)).Should().Be.EqualTo(typeof (InterfaceB));
        }

        [Test]
        public void InterclassB_for_InterfaceB()
        {
            typeof (InterfaceB).GetMostSpecificInterfaceIn(typeof (InterclassB)).Should().Be.EqualTo(typeof (InterfaceB));
        }

        [Test]
        public void InterclassC_for_InterfaceA()
        {
            typeof (InterfaceA).GetMostSpecificInterfaceIn(typeof (InterclassC)).Should().Be.EqualTo(typeof (InterfaceC));
        }

        [Test]
        public void InterclassC_for_InterfaceB()
        {
            typeof (InterfaceB).GetMostSpecificInterfaceIn(typeof (InterclassC)).Should().Be.EqualTo(typeof (InterfaceC));
        }

        [Test]
        public void InterclassC_for_InterfaceC()
        {
            typeof (InterfaceC).GetMostSpecificInterfaceIn(typeof (InterclassC)).Should().Be.EqualTo(typeof (InterfaceC));
        }
    }
}