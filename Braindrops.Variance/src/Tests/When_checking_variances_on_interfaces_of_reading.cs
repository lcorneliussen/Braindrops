using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Variance.Tests
{
    [TestFixture]
    public class When_checking_variances_on_interfaces_of_reading
    {
        [Test]
        public void ReadReadingInt_FromReadingObject_NotOK()
        {
            Variance
                .IsVariant(typeof (IReading<IReading<object>>), typeof (IReading<IReading<int>>))
                .Should().Be.False();
        }

        [Test]
        public void ReadReadingObject_FromReadingInt_OK()
        {
            Variance
                .IsVariant(typeof (IReading<IReading<int>>), typeof (IReading<IReading<object>>))
                .Should().Be.True();
        }

        [Test]
        public void ReadReadingObject_FromReadingString_OK()
        {
            Variance
                .IsVariant(typeof (IReading<IReading<string>>), typeof (IReading<IReading<object>>))
                .Should().Be.True();
        }

        [Test]
        public void ReadReadingString_FromReadingObject_NotOK()
        {
            Variance
                .IsVariant(typeof (IReading<IReading<object>>), typeof (IReading<IReading<string>>))
                .Should().Be.False();
        }

        [Test]
        public void WriteReadingInt_OnReadingObject_OK()
        {
            Variance
                .IsVariant(typeof (IWriting<IReading<object>>), typeof (IWriting<IReading<int>>))
                .Should().Be.True();
        }

        [Test]
        public void WriteReadingObject_OnReadingInt_NotOK()
        {
            Variance
                .IsVariant(typeof (IWriting<IReading<int>>), typeof (IWriting<IReading<object>>))
                .Should().Be.False();
        }

        [Test]
        public void WriteReadingObject_OnReadingString_NotOK()
        {
            Variance
                .IsVariant(typeof (IWriting<IReading<string>>), typeof (IWriting<IReading<object>>))
                .Should().Be.False();
        }

        [Test]
        public void WriteReadingString_OnReadingObject_OK()
        {
            Variance
                .IsVariant(typeof (IWriting<IReading<object>>), typeof (IWriting<IReading<string>>))
                .Should().Be.True();
        }
    }
}