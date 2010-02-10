using Common.Logging;
using Rhino.Mocks;

namespace Braindrops.Testing
{
    public abstract class TestWithMocks
    {
        private static readonly ILog _log = LogManager.GetLogger(
                                                                    typeof (TestWithMocks));

        protected MockRepository _mocks;

        /// <summary>
        /// Mocks are what we are talking about here: objects pre-programmed with 
        /// expectations which form a specification of the calls they are expected to receive.
        /// http://martinfowler.com/articles/mocksArentStubs.html.
        /// </summary>
        protected InterfaceType Mock<InterfaceType>(params object[] argumentsForConstructor)
            where InterfaceType : class
        {
            return _mocks.DynamicMock<InterfaceType>(argumentsForConstructor);
        }

        /// <summary>
        /// Stubs provide canned answers to calls made during the test, usually 
        /// not responding at all to anything outside what's programmed in for the 
        /// test. Stubs may also record information about calls, such as an email 
        /// gateway stub that remembers the messages it 'sent', or maybe only how 
        /// many messages it 'sent'.
        /// http://martinfowler.com/articles/mocksArentStubs.html.
        /// </summary>
        protected InterfaceType Stub<InterfaceType>(params object[] argumentsForConstructor)
            where InterfaceType : class
        {
            return MockRepository.GenerateStub<InterfaceType>(argumentsForConstructor);
        }

        /// <summary>
        /// Mocks are what we are talking about here: objects pre-programmed with 
        /// expectations which form a specification of the calls they are expected to receive.
        /// http://martinfowler.com/articles/mocksArentStubs.html.
        /// </summary>
        protected InterfaceType StrictMock<InterfaceType>(params object[] argumentsForConstructor)
        {
            return _mocks.StrictMock<InterfaceType>(argumentsForConstructor);
        }

        /// <summary>
        /// Dummy objects are passed around but never actually used. 
        /// Usually they are just used to fill parameter lists.
        /// http://martinfowler.com/articles/mocksArentStubs.html.
        /// </summary>
        protected InterfaceType Dummy<InterfaceType>(params object[] argumentsForConstructor)
            where InterfaceType : class
        {
            return StrictMock<InterfaceType>(argumentsForConstructor);
        }

        /// <summary>
        /// </summary>
        protected ClassType Partial<ClassType>(params object[] argumentsForConstructor)
            where ClassType : class
        {
            return _mocks.PartialMock<ClassType>(argumentsForConstructor);
        }
    }
}