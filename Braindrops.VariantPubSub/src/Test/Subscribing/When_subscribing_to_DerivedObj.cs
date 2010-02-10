using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Subscribing
{
    [TestFixture]
    public class When_subscribing_to_DerivedObj : InstanceContextSpecification<TransientEventHub>
    {
        private ISubscriber<DerivedObj> _subscriber;

        protected override void GivenContext()
        {
            _subscriber = Mock<ISubscriber<DerivedObj>>();
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
        public void ShouldHaveNoSubscriberForBaseObj()
        {
            _sut.GetSubscribers<BaseObj>()
                .ShouldNotBeNull()
                .ShouldBeEmpty();
        }

        [Test]
        public void ShouldHaveOneSubscriberForDerivedObj()
        {
            _sut.GetSubscribers<DerivedObj>()
                .ShouldNotBeNull()
                .ShouldNotBeEmpty();
        }
    }
}