using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Variance.Tests
{
    [TestFixture]
    public class When_checking_variances_on_interfaces
    {
        [Test]
        public void ReadInt_FromObject_NotOK()
        {
            Variance
                .IsVariant(typeof (IReading<object>), typeof (IReading<int>))
                .Should().Be.False();
        }

        [Test]
        public void ReadObject_FromInt_OK()
        {
            Variance
                .IsVariant(typeof (IReading<int>), typeof (IReading<object>))
                .Should().Be.True();
        }

        [Test]
        public void ReadObject_FromString_OK()
        {
            Variance
                .IsVariant(typeof (IReading<string>), typeof (IReading<object>))
                .Should().Be.True();
        }

        [Test]
        public void ReadString_FromObject_NotOK()
        {
            Variance
                .IsVariant(typeof (IReading<object>), typeof (IReading<string>))
                .Should().Be.False();
        }

        [Test]
        public void WriteInt_OnObject_OK()
        {
            Variance
                .IsVariant(typeof (IWriting<object>), typeof (IWriting<int>))
                .Should().Be.True();
        }

        [Test]
        public void WriteObject_OnInt_NotOK()
        {
            Variance
                .IsVariant(typeof (IWriting<int>), typeof (IWriting<object>))
                .Should().Be.False();
        }

        [Test]
        public void WriteObject_OnString_NotOK()
        {
            Variance
                .IsVariant(typeof (IWriting<string>), typeof (IWriting<object>))
                .Should().Be.False();
        }

        [Test]
        public void WriteString_OnObject_OK()
        {
            Variance
                .IsVariant(typeof (IWriting<object>), typeof (IWriting<string>))
                .Should().Be.True();
        }
    }
}