using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace Braindrops.Testing.Tests
{
    [TestFixture]
    public class StopAndGoThreadTest
    {
        [Test, ExpectedException(typeof (Exception))]
        public void FailSomeWhere()
        {
            Console.WriteLine("started");
            using (var thread = new StopAndGoThread())
            {
                Console.WriteLine("thread.Run");

                thread.Run(() => { throw new Exception("X"); });
            }
        }

        [Test]
        public void TestCreateAndDispose()
        {
            Console.WriteLine("started");
            new StopAndGoThread().Dispose();
            Console.WriteLine("done");
        }

        [Test]
        public void TestRunOneThing()
        {
            Console.WriteLine("started");
            using (var thread = new StopAndGoThread())
            {
                Console.WriteLine("thread.Run");
                bool ran = false;
                thread.Run(() =>
                               {
                                   ran = true;
                                   Console.WriteLine("running");
                               });
                Assert.IsTrue(ran);
                Console.WriteLine("thread.Dispose()");
            }
            Console.WriteLine("done");
        }

        [Test]
        public void TestRunTwoThings()
        {
            Console.WriteLine("started");
            using (var thread = new StopAndGoThread())
            {
                Console.WriteLine("thread.Run");
                bool ran = false;
                thread.Run(() =>
                               {
                                   ran = true;
                                   Console.WriteLine("running");
                               });

                thread.Run(() =>
                               {
                                   ran = true;
                                   Console.WriteLine("running second time");
                               });

                Assert.IsTrue(ran);
                Console.WriteLine("thread.Dispose()");
            }
            Console.WriteLine("done");
        }

        [Test]
        public void TestRunTwoThingsAsync()
        {
            Console.WriteLine("started");
            bool ranSecond = false;
            bool ranFirst = false;

            using (var thread = new StopAndGoThread())
            {
                Console.WriteLine("thread.Run");
                var st = new Stopwatch();
                st.Start();
                thread.RunAsync(() =>
                                    {
                                        Console.WriteLine("running");
                                        Thread.Sleep(50);
                                        Console.WriteLine("done 1 " + st.ElapsedMilliseconds + "ms");
                                        ranFirst = true;
                                    });

                Console.WriteLine("queue 2 " + st.ElapsedMilliseconds + "ms");

                thread.RunAsync(() =>
                                    {
                                        Console.WriteLine("running second time");
                                        Console.WriteLine("done 2 " + st.ElapsedMilliseconds + "ms");
                                        ranSecond = true;
                                    });

                Console.WriteLine("thread.Dispose()");
            }
            Assert.IsTrue(ranFirst);
            Assert.IsTrue(ranSecond);
            Console.WriteLine("done");
        }
    }
}