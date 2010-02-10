using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Subscribing
{
    [TestFixture]
    public class When_subscribing_to_Event_of_DerivedObj : InstanceContextSpecification<TransientEventHub>
    {
        private ISubscriber<IEvent<DerivedObj>> _subscriber;

        protected override void GivenContext()
        {
            _subscriber = Mock<ISubscriber<IEvent<DerivedObj>>>();
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
        public void ShouldHaveNoSubscriberFor_EventOf_BaseObj()
        {
            _sut.GetSubscribers<IEvent<BaseObj>>()
                .ShouldNotBeNull()
                .ShouldBeEmpty();
        }

        [Test]
        public void ShouldHaveNoSubscriberFor_EventOf_object()
        {
            _sut.GetSubscribers<IEvent<object>>()
                .ShouldNotBeNull()
                .ShouldBeEmpty();
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