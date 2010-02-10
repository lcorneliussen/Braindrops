using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Variance.Tests
{
    [TestFixture]
    public class When_checking_variances_on_interfaces_of_writing
    {
        [Test]
        public void ReadWritingInt_FromWritingObject_OK()
        {
            Variance
                .IsVariant(typeof (IReading<IWriting<object>>), typeof (IReading<IWriting<int>>))
                .Should().Be.True();
        }

        [Test]
        public void ReadWritingObject_FromWritingInt_NotOK()
        {
            Variance
                .IsVariant(typeof (IReading<IWriting<int>>), typeof (IReading<IWriting<object>>))
                .Should().Be.False();
        }

        [Test]
        public void ReadWritingObject_FromWritingString_NotOK()
        {
            Variance
                .IsVariant(typeof (IReading<IWriting<string>>), typeof (IReading<IWriting<object>>))
                .Should().Be.False();
        }

        [Test]
        public void ReadWritingString_FromWritingObject_OK()
        {
            Variance
                .IsVariant(typeof (IReading<IWriting<object>>), typeof (IReading<IWriting<string>>))
                .Should().Be.True();
        }

        [Test]
        public void WriteWritingInt_OnWritingObject_NotOK()
        {
            Variance
                .IsVariant(typeof (IWriting<IWriting<object>>), typeof (IWriting<IWriting<int>>))
                .Should().Be.False();
        }

        [Test]
        public void WriteWritingObject_OnWritingInt_OK()
        {
            Variance
                .IsVariant(typeof (IWriting<IWriting<int>>), typeof (IWriting<IWriting<object>>))
                .Should().Be.True();
        }

        [Test]
        public void WriteWritingObject_OnWritingString_OK()
        {
            Variance
                .IsVariant(typeof (IWriting<IWriting<string>>), typeof (IWriting<IWriting<object>>))
                .Should().Be.True();
        }

        [Test]
        public void WriteWritingString_OnWritingObject_NotOK()
        {
            Variance
                .IsVariant(typeof (IWriting<IWriting<object>>), typeof (IWriting<IWriting<string>>))
                .Should().Be.False();
        }
    }
}