using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_derivedObj_in_event_of_baseObj : arbitrary_subscriptions
    {
        private DerivedObj _data;
        private Event<BaseObj> _event;

        protected override void setupExpectations()
        {
            _data = new DerivedObj();
            _event = new Event<BaseObj>(_data);

            _objectSubscriber.Expect(s => s.OnPublished(_event));

            _eventWithObjectSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data))
                .Message("_eventWithObjectSubscriber");

            _eventWithBaseObjSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data))
                .Message("_eventWithBaseObjSubscriber");
        }

        protected override void Because()
        {
            _sut.Publish<Event<BaseObj>>(_event);
        }
    }
}