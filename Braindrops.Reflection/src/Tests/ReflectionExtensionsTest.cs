using System;
using System.Reflection;
using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Reflection.Tests
{
    [TestFixture]
    public class GetBaseDefinitionTests
    {
        [Test]
        public void ShouldErrorOnFindingBaseForInterfaceY()
        {
            new Action(() => typeof (InterfaceY).GetTypeOrInterfaceBaseDefinition()).Should().Throw
                <NotSupportedException>();
        }

        [Test]
        public void ShouldErrorOnFindingBaseForInterfaceZ()
        {
            new Action(() => typeof (InterfaceZ).GetTypeOrInterfaceBaseDefinition()).Should().Throw
                <NotSupportedException>();
        }

        [Test]
        public void ShouldFindBaseAForInterfaceB()
        {
            typeof (InterfaceB).GetTypeOrInterfaceBaseDefinition().Should().Be.EqualTo(typeof (InterfaceA));
        }

        [Test]
        public void ShouldFindBaseCForInterfaceB()
        {
            typeof (InterfaceC).GetTypeOrInterfaceBaseDefinition().Should().Be.EqualTo(typeof (InterfaceB));
        }

        [Test]
        public void ShouldFindBaseDForInterfaceC()
        {
            typeof (InterfaceD).GetTypeOrInterfaceBaseDefinition().Should().Be.EqualTo(typeof (InterfaceC));
        }

        [Test]
        public void ShouldFindNoBaseForInterfaceA()
        {
            typeof (InterfaceA).GetTypeOrInterfaceBaseDefinition().Should().Be.Null();
        }

        [Test]
        public void ShouldFindPropertyBaseDefinition()
        {
            Type t1 = typeof (ClassA);
            PropertyInfo p1 = t1.GetProperty("SomeProperty");
            p1.Name.Should().Be.EqualTo("SomeProperty");
            var p1Info = (SomeAttribute) p1.GetCustomAttributes(typeof (SomeAttribute), false)[0];
            p1Info.Value.Equals("PropertyBase");

            Type t2 = typeof (ClassB);
            PropertyInfo p2 = t2.GetProperty("SomeProperty");
            p2.Name.Should().Be.EqualTo("SomeProperty");
            var p2Info = (SomeAttribute) p2.GetCustomAttributes(typeof (SomeAttribute), false)[0];
            p2Info.Value.Equals("PropertyOverridden");

            PropertyInfo p1retrieved = p2.GetTypeOrInterfaceBaseDefinition();
            p1retrieved.Should().Be.EqualTo(p1);
        }
    }

    public class ClassX<T>
    {
        public bool Method(T t, bool hello)
        {
            return false;
        }

        public bool Method2<T1>(T t, T1 hello)
        {
            return false;
        }

        #region Nested type: ClassY

        public class ClassY<T1, T2>
        {
            public void Method(T1 t1, bool hello)
            {
            }

            public void Method2<T3>(T1 t1, T3 hello)
            {
            }
        }

        #endregion

        #region Nested type: ClassZ

        internal class ClassZ
        {
            public int Method()
            {
                return 0;
            }

            public T1 Method2<T1>()
            {
                return default(T1);
            }
        }

        #endregion
    }

    public class ClassA
    {
        [Some("Property")]
        public virtual bool SomeProperty { get; set; }
    }

    public class SomeAttribute : Attribute
    {
        public SomeAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }

    public class ClassB : ClassA
    {
        [Some("PropertyOverridden")]
        public override bool SomeProperty
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }

    public interface InterfaceA
    {
    }

    public class InterclassA : InterfaceA
    {
    }

    public interface InterfaceB : InterfaceA
    {
    }

    public class InterclassB : InterclassA, InterfaceB
    {
    }

    public interface InterfaceC : InterfaceB
    {
    }

    public class InterclassC : InterclassB, InterfaceC
    {
    }

    public interface InterfaceD : InterfaceC
    {
    }

    public interface InterfaceX
    {
    }

    public interface InterfaceY : InterfaceA, InterfaceX
    {
    }

    public interface InterfaceZ : InterfaceB, InterfaceX
    {
    }
}