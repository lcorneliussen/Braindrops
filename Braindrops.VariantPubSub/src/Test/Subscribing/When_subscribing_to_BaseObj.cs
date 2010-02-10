using System.Linq;
using Braindrops.Testing;
using Braindrops.Variance;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Subscribing
{
    [TestFixture]
    public class When_subscribing_to_BaseObj : InstanceContextSpecification<TransientEventHub>
    {
        private ISubscriber<BaseObj> _subscriber;

        protected override void GivenContext()
        {
            _subscriber = Mock<ISubscriber<BaseObj>>();
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
        public void ShouldHaveOneSubscriberForBaseObj()
        {
            _sut.GetSubscribers<BaseObj>()
                .ShouldNotBeNull()
                .ShouldNotBeEmpty()
                .Single()
                .IsVariantTo<ISubscriber<DerivedObj>>().ShouldBeTrue();
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