using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Variance.Tests
{
    [TestFixture]
    public class When_using_variances_on_types_of_reading
    {
        private Reading<IReading<T>> readReading<T>(T obj)
        {
            return new Reading<IReading<T>>(new Reading<T>(obj));
        }

        private Writing<IReading<T>> writeReading<T>()
        {
            return new Writing<IReading<T>>();
        }

        [Test]
        public void ReadReadingObject_FromReadingInt_OK()
        {
            Reading<IReading<int>> x = readReading(2);
            var y = x.AsVariant<IReading<IReading<object>>>();
            ((int) y.Get().Get()).Should().Be.EqualTo(2);
        }

        [Test]
        public void ReadReadingObject_FromReadingString_OK()
        {
            Reading<IReading<string>> x = readReading("something");
            var y = x.AsVariant<IReading<IReading<object>>>();
            y.Get().Get().Should().Be.EqualTo("something");
        }

        [Test]
        public void WriteInt_OnObject_OK()
        {
            Writing<IReading<object>> x = writeReading<object>();
            var y = x.AsVariant<IWriting<IReading<int>>>();
            y.Set(new Reading<int>(2));
            x.Object.Should().Be.AssignableTo<IReading<object>>();
            ((int) ((IReading<object>) x.Object).Get()).Should().Be.EqualTo(2);
        }

        [Test]
        public void WriteString_OnObject_OK()
        {
            Writing<IReading<object>> x = writeReading<object>();
            var y = x.AsVariant<IWriting<IReading<string>>>();
            y.Set(new Reading<string>("something"));
            x.Object.Should().Be.AssignableTo<IReading<object>>();
            ((IReading<object>) x.Object).Get().Should().Be.EqualTo("something");
        }
    }
}