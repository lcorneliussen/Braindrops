using NUnit.Framework;
using Rhino.Mocks;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_baseObj : arbitrary_subscriptions
    {
        private BaseObj _data;

        protected override void setupExpectations()
        {
            _data = new BaseObj();
            _objectSubscriber.Expect(s => s.OnPublished(_data));
            _baseObjSubscriber.Expect(s => s.OnPublished(_data));
        }

        protected override void Because()
        {
            _sut.Publish<BaseObj>(_data);
        }
    }
}