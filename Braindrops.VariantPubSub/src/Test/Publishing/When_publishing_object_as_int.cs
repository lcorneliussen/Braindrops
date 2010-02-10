using System;
using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_object_as_int : arbitrary_subscriptions
    {
        private Exception _exception;

        protected override void setupExpectations()
        {
        }

        protected override void Because()
        {
            _exception = Try.Catch(() => _sut.Publish<int>(new object()));
        }

        [Test]
        public void ExceptionType_ShouldBeAArgumentException()
        {
            _exception.GetType().ShouldEqual<ArgumentException>();
        }

        [Test]
        public void ShouldThrowAnException()
        {
            _exception.ShouldNotBeNull();
        }
    }
}