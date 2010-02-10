using System;
using Common.Logging;
using NUnit.Framework;
using Rhino.Mocks;

namespace Braindrops.Testing
{
    public abstract class StaticContextSpecification : TestWithMocks
    {
        public const string TEST_THROW_EXCEPTION_IN_SETUP = "TEST_THROW_EXCEPTION_IN_SETUP";

        protected static readonly ILog _log = LogManager.GetLogger(
                                                                      typeof (StaticContextSpecification));

        private Exception _exceptionInSetup;
        private bool _throwExceptionInSetup;

        public virtual bool ShareContextForObservations
        {
            get { return true; }
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            if (ShareContextForObservations)
            {
                InitializeContext();
            }
        }

        private bool InitializeContext()
        {
            _log.Debug("Initializing context for " + GetType().GetTypeDisplayName());

            _exceptionInSetup = null;
            _throwExceptionInSetup = IsEnvironmentVariableTrue(TEST_THROW_EXCEPTION_IN_SETUP);

            try
            {
                _mocks = new MockRepository();
                using (_mocks.Record())
                {
                    GivenContext();
                    InitializeSystemUnderTest();
                }
            }
            catch (Exception e)
            {
                // Caution: exception messages are not shown on the build server nunit output
                // show the exception in the System.out.log
                _exceptionInSetup = new Exception("Error in InitializeContext().", e);
                _log.Error("Initialization Exception: ", e);
                try
                {
                    InitializeContextFailed(_exceptionInSetup);
                }
                catch
                {
                }
                if (_throwExceptionInSetup)
                {
                    throw _exceptionInSetup.PreserveErrorStackTrace();
                }

                return false;
            }

            try
            {
                _mocks.ReplayAll();
                Because();
            }
            catch (Exception e)
            {
                _exceptionInSetup = new Exception("Error in Because().", e);
                _log.Error("Because Exception: ", e);
                try
                {
                    BecauseFailed(_exceptionInSetup);
                }
                catch
                {
                }
                if (_throwExceptionInSetup)
                {
                    throw _exceptionInSetup.PreserveErrorStackTrace();
                }

                return false;
            }

            return true;
        }

        [SetUp]
        public void Setup()
        {
            if (!ShareContextForObservations)
            {
                InitializeContext();
            }

            try
            {
                BeforeEachObservation();
            }
            catch (Exception e)
            {
                _exceptionInSetup = new Exception("Error in BeforeEachObservation().", e);
                _log.Error("BeforeEachObservation Exception: ", e);
                try
                {
                    BeforeEachObservationFailed(_exceptionInSetup);
                }
                catch
                {
                }
                if (_throwExceptionInSetup)
                {
                    throw _exceptionInSetup;
                }
            }
        }

        protected virtual void InitializeContextFailed(Exception exception)
        {
        }

        protected virtual void BecauseFailed(Exception exception)
        {
            if (_throwExceptionInSetup)
                CleanUpContext();
        }

        protected virtual void BeforeEachObservationFailed(Exception exception)
        {
            if (_throwExceptionInSetup)
                CleanUpContext();
        }


        [TearDown]
        public void TearDown()
        {
            AfterEachObservation();

            if (!ShareContextForObservations)
            {
                _mocks.VerifyAll();
                CleanUpContext();
            }
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            if (ShareContextForObservations)
            {
                _mocks.VerifyAll();
                CleanUpContext();
            }
        }

        protected abstract void Because();

        protected virtual void GivenContext()
        {
        }

        protected virtual void InitializeSystemUnderTest()
        {
        }

        protected virtual void BeforeEachObservation()
        {
        }

        protected virtual void AfterEachObservation()
        {
        }

        protected virtual void CleanUpContext()
        {
        }

        [Test]
        public void ShouldHaveRanSetupWithoutException()
        {
            if (_exceptionInSetup != null)
            {
                throw _exceptionInSetup.PreserveErrorStackTrace();
            }
        }

        protected bool IsEnvironmentVariableTrue(string variableName)
        {
            string val = Environment.GetEnvironmentVariable(variableName);
            return "true".Equals(val, StringComparison.OrdinalIgnoreCase);
        }
    }
}