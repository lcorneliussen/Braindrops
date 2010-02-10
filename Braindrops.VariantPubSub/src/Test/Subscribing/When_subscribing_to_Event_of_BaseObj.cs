using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Subscribing
{
    [TestFixture]
    public class When_subscribing_to_Event_of_BaseObj : InstanceContextSpecification<TransientEventHub>
    {
        private ISubscriber<IEvent<BaseObj>> _subscriber;

        protected override void GivenContext()
        {
            _subscriber = Mock<ISubscriber<IEvent<BaseObj>>>();
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
        public void ShouldHaveOneSubscriberFor_EventOf_BaseObj()
        {
            _sut.GetSubscribers<IEvent<BaseObj>>()
                .ShouldNotBeNull()
                .ShouldNotBeEmpty();
        }

        [Test]
        public void ShouldHaveOneSubscriberFor_EventOf_DerivedObj()
        {
            _sut.GetSubscribers<IEvent<DerivedObj>>()
                .ShouldNotBeNull()
                .ShouldNotBeEmpty();
        }
    }
}