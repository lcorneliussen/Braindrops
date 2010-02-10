using System;
using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_event_of_baseObj_as_event_of_derivedObject : arbitrary_subscriptions
    {
        private BaseObj _data;
        private Event<BaseObj> _event;
        private Exception _exception;

        protected override void setupExpectations()
        {
            _data = new BaseObj();
            _event = new Event<BaseObj>(_data);
        }

        protected override void Because()
        {
            _exception = Try.Catch(() => _sut.Publish<IEvent<DerivedObj>>(_event));
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