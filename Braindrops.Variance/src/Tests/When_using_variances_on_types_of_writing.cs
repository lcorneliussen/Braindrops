using NUnit.Framework;
using SharpTestsEx;

namespace Braindrops.Variance.Tests
{
    [TestFixture]
    public class When_using_variances_on_types_of_writing
    {
        private Reading<IWriting<T>> readWriting<T>()
        {
            return new Reading<IWriting<T>>(new Writing<T>());
        }

        [Test]
        public void ReadWritingInt_FromWritingObj_OK()
        {
            Reading<IWriting<object>> x = readWriting<object>();
            var y = x.AsVariant<IReading<IWriting<int>>>();
            y.Get().Set(2);

            ((int) y.Get().Object).Should().Be.EqualTo(2);
        }

        [Test]
        public void ReadWritingObject_FromWritingString_OK()
        {
            Reading<IWriting<object>> x = readWriting<object>();
            var y = x.AsVariant<IReading<IWriting<string>>>();
            y.Get().Set("hello");

            y.Get().Object.Should().Be.EqualTo("hello");
        }

        [Test]
        public void WriteObjectWriter_ViaIntWriter()
        {
            var x = new Writing<IWriting<int>>();

            var writingObject = new Writing<object>();
            var writingViaInt = writingObject.AsVariant<IWriting<int>>();

            x.Set(writingViaInt);

            writingObject.Set(2);

            x.Object.Should().Be.AssignableTo<IWriting<int>>();

            ((int) ((IWriting<int>) x.Object).Object).Should().Be.EqualTo(2);
        }

        [Test]
        public void WriteObjectWriter_ViaStringWriter()
        {
            var x = new Writing<IWriting<string>>();

            var writingObject = new Writing<object>();
            var writingViaString = writingObject.AsVariant<IWriting<string>>();

            x.Set(writingViaString);

            writingObject.Set("something");

            x.Object.Should().Be.AssignableTo<IWriting<string>>();
            ((IWriting<string>) x.Object).Object.Should().Be.EqualTo("something");
        }
    }
}