using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Variance.Tests
{
    [TestFixture]
    public class When_checking_variances_on_read_write
    {
        [Test]
        public void ReadInt_FromReadWriteObject_NotOK()
        {
            Variance
                .IsVariant(typeof (ReadWrite<object, object>), typeof (IReading<int>))
                .Should().Be.False();
        }

        [Test]
        public void ReadObject_FromReadWriteInt_OK()
        {
            Variance
                .IsVariant(typeof (ReadWrite<int, int>), typeof (IReading<object>))
                .Should().Be.True();
        }

        [Test]
        public void ReadObject_FromReadWriteString_OK()
        {
            Variance
                .IsVariant(typeof (ReadWrite<string, string>), typeof (IReading<object>))
                .Should().Be.True();
        }

        [Test]
        public void ReadString_FromReadWriteObject_NotOK()
        {
            Variance
                .IsVariant(typeof (ReadWrite<object, object>), typeof (IReading<string>))
                .Should().Be.False();
        }

        [Test]
        public void WriteInt_OnReadWriteObject_OK()
        {
            Variance
                .IsVariant(typeof (ReadWrite<object, object>), typeof (IWriting<int>))
                .Should().Be.True();
        }

        [Test]
        public void WriteObject_OnReadWriteInt_NotOK()
        {
            Variance
                .IsVariant(typeof (ReadWrite<int, int>), typeof (IWriting<object>))
                .Should().Be.False();
        }

        [Test]
        public void WriteObject_OnReadWriteString_NotOK()
        {
            Variance
                .IsVariant(typeof (ReadWrite<string, string>), typeof (IWriting<object>))
                .Should().Be.False();
        }

        [Test]
        public void WriteString_OnReadWriteObject_OK()
        {
            Variance
                .IsVariant(typeof (ReadWrite<object, object>), typeof (IWriting<string>))
                .Should().Be.True();
        }
    }
}