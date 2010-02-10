using Rhino.Mocks;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    public class When_publishing_derivedObj_as_baseObj : arbitrary_subscriptions
    {
        private DerivedObj _data;

        protected override void setupExpectations()
        {
            _data = new DerivedObj();
            _objectSubscriber.Expect(s => s.OnPublished(_data));
            _baseObjSubscriber.Expect(s => s.OnPublished(_data));
        }

        protected override void Because()
        {
            _sut.Publish<BaseObj>(_data);
        }
    }
}