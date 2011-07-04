using Braindrops.Reflection;
using Braindrops.Testing;
using Minimod.PrettyTypeSignatures;
using Rhino.Mocks;

namespace Braindrops.VariantPubSub.Test.Publishing
{
    public abstract class arbitrary_subscriptions : InstanceContextSpecification<TransientEventHub>
    {
        protected ISubscriber<BaseObj> _baseObjSubscriber;
        protected ISubscriber<DerivedObj> _derivedObjSubscriber;

        protected ISubscriber<IEvent<BaseObj>> _eventWithBaseObjSubscriber;
        protected ISubscriber<IEvent<DerivedObj>> _eventWithDerivedObjSubscriber;
        protected ISubscriber<IEvent<object>> _eventWithObjectSubscriber;
        protected ISubscriber<int> _intSubscriber;
        protected ISubscriber<object> _objectSubscriber;

        protected override void GivenContext()
        {
            _objectSubscriber = StrictMock<ISubscriber<object>>();
            _intSubscriber = StrictMock<ISubscriber<int>>();
            _baseObjSubscriber = StrictMock<ISubscriber<BaseObj>>();
            _derivedObjSubscriber = StrictMock<ISubscriber<DerivedObj>>();

            _eventWithObjectSubscriber = StrictMock<ISubscriber<IEvent<object>>>();
            _eventWithBaseObjSubscriber = StrictMock<ISubscriber<IEvent<BaseObj>>>();
            _eventWithDerivedObjSubscriber = StrictMock<ISubscriber<IEvent<DerivedObj>>>();

            fakeName(_objectSubscriber);
            fakeName(_intSubscriber);
            fakeName(_baseObjSubscriber);
            fakeName(_derivedObjSubscriber);

            fakeName(_eventWithObjectSubscriber);
            fakeName(_eventWithBaseObjSubscriber);
            fakeName(_eventWithDerivedObjSubscriber);

            setupExpectations();
        }

        private void fakeName(ISubscriber sub)
        {
            sub.Expect(o => o.DisplayName).Repeat.Any()
                .Return(sub.GetType().GetPrettyName());
        }

        protected abstract void setupExpectations();

        protected override TransientEventHub GivenInstance()
        {
            var hub = new TransientEventHub();
            hub.Register(_objectSubscriber);
            hub.Register(_intSubscriber);
            hub.Register(_baseObjSubscriber);
            hub.Register(_derivedObjSubscriber);

            hub.Register(_eventWithObjectSubscriber);
            hub.Register(_eventWithBaseObjSubscriber);
            hub.Register(_eventWithDerivedObjSubscriber);
            return hub;
        }
    }
}