using System;
using Common.Logging;
using NUnit.Framework;
using Rhino.Mocks;

namespace Braindrops.Testing
{
    public class BaseTest : TestWithMocks
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof (BaseTest));

        protected Exception _exception;

        protected bool _mockSetupDone;

        [TestFixtureSetUp]
        protected virtual void Init()
        {
            _log.Info("setup test fixture");
        }

        [TestFixtureTearDown]
        protected virtual void Dispose()
        {
            _log.Info("teardown test fixture");
        }

        [SetUp]
        protected virtual void SetUp()
        {
            _mocks = new MockRepository();
            _log.Info("setup test method");
        }

        [TearDown]
        protected virtual void TearDown()
        {
            if (_mockSetupDone)
            {
                _mocks.VerifyAll();
            }

            _mocks = null;
            _log.Info("teardown test method");
        }

        [Test]
        public void ShouldHaveRunInitAndSetupWithoutException()
        {
            if (_exception != null)
            {
                throw _exception.PreserveErrorStackTrace();
            }
        }

        protected bool isEnvironmentVariableTrue(string variableName)
        {
            string val = Environment.GetEnvironmentVariable(variableName);
            return "true".Equals(val, StringComparison.OrdinalIgnoreCase);
        }

        // ===================================================
        protected static void printErrorMessage(Exception e)
        {
            _log.Info(e.Message);
        }

        protected static void printError(Exception e)
        {
            _log.Info(e);
        }

        protected static string datetimeToString(string format, DateTime? datetime)
        {
            if (datetime == null)
            {
                return "null";
            }

            return datetime.Value.ToString(format);
        }

        public void MockSetupDone()
        {
            _mocks.ReplayAll();
            _mockSetupDone = true;
        }
    }
}