using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Variance.Tests
{
    [TestFixture]
    public class When_using_variances_on_types
    {
        [Test]
        public void ReadObject_FromInt_OK()
        {
            var x = new Reading<int>(2);
            var y = x.AsVariant<IReading<object>>();
            ((int) y.Get()).Should().Be.EqualTo(2);
        }

        [Test]
        public void ReadObject_FromString_OK()
        {
            var x = new Reading<string>("something");
            var y = x.AsVariant<IReading<object>>();
            y.Get().Should().Be.EqualTo("something");
        }

        [Test]
        public void WriteString_OnObject_OK()
        {
            var x = new Writing<object>();
            var y = x.AsVariant<IWriting<string>>();
            y.Set("something");
            x.Object.Should().Be.EqualTo("something");
        }
    }
}