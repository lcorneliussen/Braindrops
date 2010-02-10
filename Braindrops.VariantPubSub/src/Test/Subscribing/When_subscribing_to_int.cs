using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Subscribing
{
    [TestFixture]
    public class When_subscribing_to_int : InstanceContextSpecification<TransientEventHub>
    {
        private ISubscriber<int> _subscriber;

        protected override void GivenContext()
        {
            _subscriber = Mock<ISubscriber<int>>();
        }

        protected override TransientEventHub GivenInstance()
        {
            return new TransientEventHub();
        }

        protected override void Because()
        {
            _sut.Register(_subscriber);
        }

        [Test]
        public void ShouldHaveOneSubscriberForInt()
        {
            _sut.GetSubscribers<int>()
                .ShouldNotBeNull()
                .ShouldNotBeEmpty();
        }
    }
}