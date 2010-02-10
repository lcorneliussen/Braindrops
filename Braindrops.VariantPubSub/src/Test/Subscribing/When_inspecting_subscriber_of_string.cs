using Braindrops.Testing;
using Braindrops.Variance;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Subscribing
{
    [TestFixture]
    public class When_inspecting_subscriber_of_string : InstanceContextSpecification<ISubscriber<string>>
    {
        private ISubscriber<string> _subscriber;

        protected override void Because()
        {
        }

        protected override void GivenContext()
        {
            _subscriber = Mock<ISubscriber<string>>();
        }

        protected override ISubscriber<string> GivenInstance()
        {
            return _subscriber;
        }

        [Test]
        public void ShouldBeVariantTo_SubscriberOfString()
        {
            _sut.IsVariantTo<ISubscriber<string>>().ShouldBeTrue();
        }

        [Test]
        public void ShouldNotBeVariantTo_SubscriberOfBaseObj()
        {
            _sut.IsVariantTo<ISubscriber<BaseObj>>().ShouldBeFalse();
        }

        [Test]
        public void ShouldNotBeVariantTo_SubscriberOfDerivedObj()
        {
            _sut.IsVariantTo<ISubscriber<BaseObj>>().ShouldBeFalse();
        }

        [Test]
        public void ShouldNotBeVariantTo_SubscriberOfInt()
        {
            _sut.IsVariantTo<ISubscriber<int>>().ShouldBeFalse();
        }

        [Test]
        public void ShouldNotBeVariantTo_SubscriberOfObject()
        {
            _sut.IsVariantTo<ISubscriber<object>>().ShouldBeFalse();
        }
    }
}