using System.Threading;
using NUnit.Framework;

namespace Braindrops.Testing.Tests.TestStopwatchTests
{
    [TestFixture]
    public class TestAccuracy : BaseTest
    {
        [Test]
        public void some_cases()
        {
            TestStopwatch.MeasureLaps("nothing#1", 10, () => { });

            TestStopwatch.MeasureLaps("nothing#2", 10, () => { });

            TestStopwatch.MeasureLaps("10ms pause", 10, sw =>
                                                            {
                                                                using (sw.Pause())
                                                                    Thread.Sleep(10);
                                                            });

            TestStopwatch.MeasureLaps("10ms", 10, () => Thread.Sleep(10));

            TestStopwatch.MeasureLaps("10ms pause + 10ms", 10, sw =>
                                                                   {
                                                                       using (sw.Pause())
                                                                           Thread.Sleep(10);

                                                                       Thread.Sleep(10);
                                                                   });
        }
    }
}