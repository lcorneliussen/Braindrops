using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_event_of_derivedObj_as_event_of_baseObject : arbitrary_subscriptions
    {
        private DerivedObj _data;
        private Event<DerivedObj> _event;

        protected override void setupExpectations()
        {
            _data = new DerivedObj();
            _event = new Event<DerivedObj>(_data);

            _objectSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data))
                .Message("_objectSubscriber");

            _eventWithObjectSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data))
                .Message("_eventWithObjectSubscriber");

            _eventWithBaseObjSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data))
                .Message("_eventWithBaseObjSubscriber");
        }

        protected override void Because()
        {
            _sut.Publish<IEvent<BaseObj>>(_event);
        }
    }
}