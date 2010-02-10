using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_event_of_derivedObj : arbitrary_subscriptions
    {
        private DerivedObj _data;
        private Event<DerivedObj> _event;

        protected override void setupExpectations()
        {
            _data = new DerivedObj();
            _event = new Event<DerivedObj>(_data);

            _objectSubscriber.Expect(s => s.OnPublished(_event));

            _eventWithObjectSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data));

            _eventWithBaseObjSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data));

            _eventWithDerivedObjSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data));
        }

        protected override void Because()
        {
            _sut.Publish<Event<DerivedObj>>(_event);
        }
    }
}