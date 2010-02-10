using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_event_of_baseObj : arbitrary_subscriptions
    {
        private Event<BaseObj> _event;
        private BaseObj _data;

        protected override void setupExpectations()
        {
            _data = new BaseObj();
            _event = new Event<BaseObj>(_data);

            _objectSubscriber.Expect(s => s.OnPublished(_event));

            _eventWithObjectSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data));

            _eventWithBaseObjSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Property.Value("Data", _data));
        }

        protected override void Because()
        {
            _sut.Publish<Event<BaseObj>>(_event);
        }
    }
}