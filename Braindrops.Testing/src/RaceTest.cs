using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Braindrops.Testing
{
    [TestFixture]
    public class RaceTest
    {
        private IEnumerable<RaceControl<string>> _testSimple(Race<string> race)
        {
            Console.WriteLine("operation called on thread {0}, name '{1}'", Thread.CurrentThread.ManagedThreadId,
                              Thread.CurrentThread.Name);
            int nId = Thread.CurrentThread.ManagedThreadId;

            yield return race.RunAs("x");
            Console.WriteLine("Should be in x");
            Console.WriteLine("operation called on thread {0}, name '{1}'", Thread.CurrentThread.ManagedThreadId,
                              Thread.CurrentThread.Name);

            int xId = Thread.CurrentThread.ManagedThreadId;
            Assert.AreNotEqual(nId, xId);

            yield return race.RunAs("y");
            Console.WriteLine("Should be in y");
            Console.WriteLine("operation called on thread {0}, name '{1}'", Thread.CurrentThread.ManagedThreadId,
                              Thread.CurrentThread.Name);

            int yId = Thread.CurrentThread.ManagedThreadId;
            Assert.AreNotEqual(nId, yId);
            Assert.AreNotEqual(xId, nId);

            yield return race.RunAs("x");
            Console.WriteLine("Should be in x again");
            Console.WriteLine("operation called on thread {0}, name '{1}'", Thread.CurrentThread.ManagedThreadId,
                              Thread.CurrentThread.Name);
            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, xId);
        }

        [ThreadStatic] private static int x;

        private IEnumerable<RaceControl<Formula1Teams>> _testThreadStatic(Race<Formula1Teams> race)
        {
            yield return race.RunAs(Formula1Teams.Ferrari);

            x.ShouldEqual(0);
            x++;
            x.ShouldEqual(1).Log();

            yield return race.RunAs(Formula1Teams.BmwSauber);

            x.ShouldEqual(0);
            x++;
            x.ShouldEqual(1).Log();

            yield return race.RunAs(Formula1Teams.Ferrari);

            x.ShouldEqual(1);
            x++;
            x++;
            x.ShouldEqual(3).Log();

            yield return race.RunAs(Formula1Teams.BmwSauber);

            x.ShouldEqual(1).Log();
        }

        private IEnumerable<RaceControl<string>> _testAsync(Race<string> race)
        {
            5.Log("Wait anywhere");
            Thread.Sleep(5);

            yield return race.RunAsync("a", () =>
                                                {
                                                    5.Log("Wait in a?");
                                                    Thread.Sleep(10);
                                                });

            yield return race.RunAs("b");
            10.Log("Wait in b?");
            Thread.Sleep(5);
        }

        [Test]
        public void TestAsync()
        {
            using (var multi = new Race<string>())
            {
                multi.Execute(_testAsync);
            }
        }

        [Test]
        public void TestFoo()
        {
            using (var multi = new Race<string>())
            {
                multi.Execute(_testSimple);
            }
        }

        [Test]
        public void TestThreadStatic()
        {
            using (var race = new Race<Formula1Teams>())
            {
                race.Execute(_testThreadStatic);
            }
        }
    }

    public enum Formula1Teams
    {
        Ferrari,
        Williams,
        BmwSauber
    }
}