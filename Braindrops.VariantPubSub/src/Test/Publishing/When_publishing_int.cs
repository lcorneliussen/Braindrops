using NUnit.Framework;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    [TestFixture]
    public class When_publishing_int : arbitrary_subscriptions
    {
        protected override void setupExpectations()
        {
            _objectSubscriber.Expect(s => s.OnPublished(null))
                .Constraints(Is.Matching<object>(o => ((int) o) == 1));

            _intSubscriber.Expect(s => s.OnPublished(1));
        }

        protected override void Because()
        {
            _sut.Publish<int>(1);
        }
    }
}