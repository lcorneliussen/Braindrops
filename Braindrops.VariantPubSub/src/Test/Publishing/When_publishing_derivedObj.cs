using NUnit.Framework;
using Rhino.Mocks;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_derivedObj : arbitrary_subscriptions
    {
        private DerivedObj _data;

        protected override void setupExpectations()
        {
            _data = new DerivedObj();
            _objectSubscriber.Expect(s => s.OnPublished(_data));
            _baseObjSubscriber.Expect(s => s.OnPublished(_data));
            _derivedObjSubscriber.Expect(s => s.OnPublished(_data));
        }

        protected override void Because()
        {
            _sut.Publish<DerivedObj>(_data);
        }
    }
}