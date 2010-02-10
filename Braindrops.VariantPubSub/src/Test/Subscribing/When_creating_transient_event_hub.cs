using Braindrops.Testing;
using NUnit.Framework;

namespace Braindrops.VariantPubSub.Test.Subscribing
{
    [TestFixture]
    public class When_creating_transient_event_hub : InstanceContextSpecification<TransientEventHub>
    {
        protected override void GivenContext()
        {
        }

        protected override TransientEventHub GivenInstance()
        {
            return new TransientEventHub();
        }

        protected override void Because()
        {
        }
    }
}