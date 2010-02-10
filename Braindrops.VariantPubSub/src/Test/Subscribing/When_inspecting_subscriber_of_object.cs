using Braindrops.Testing;
using Braindrops.Variance;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Subscribing
{
    [TestFixture]
    public class When_inspecting_subscriber_of_object : InstanceContextSpecification<ISubscriber<object>>
    {
        private ISubscriber<object> _subscriber;

        protected override void GivenContext()
        {
            _subscriber = Mock<ISubscriber<object>>();
        }

        protected override ISubscriber<object> GivenInstance()
        {
            return _subscriber;
        }

        protected override void Because()
        {
        }

        [Test]
        public void ShouldBeVariantTo_SubscriberOfBaseObj()
        {
            _sut.IsVariantTo<ISubscriber<BaseObj>>().ShouldBeTrue();
        }

        [Test]
        public void ShouldBeVariantTo_SubscriberOfDerivedObj()
        {
            _sut.IsVariantTo<ISubscriber<BaseObj>>().ShouldBeTrue();
        }

        [Test]
        public void ShouldBeVariantTo_SubscriberOfInt()
        {
            _sut.IsVariantTo<ISubscriber<int>>().ShouldBeTrue();
        }

        [Test]
        public void ShouldBeVariantTo_SubscriberOfObject()
        {
            _sut.IsVariantTo<ISubscriber<object>>().ShouldBeTrue();
        }

        [Test]
        public void ShouldBeVariantTo_SubscriberOfString()
        {
            _sut.IsVariantTo<ISubscriber<string>>().ShouldBeTrue();
        }
    }
}