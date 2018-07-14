using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WatchdogDotNet;

namespace UnitTests
{
    [TestClass]
    public class WatchdogTests
    {
        static readonly TimeSpan _standardInterval = TimeSpan.FromMilliseconds(50);
        const int _standardPaddingTimeInMilliseconds = 100;

        [TestMethod]
        public void TestTimeoutConstructor()
        {
            using (var watchdog = new WatchdogTimer(_standardInterval))
            {
                int a = 0;
                var mre = new ManualResetEventSlim(false);
                watchdog.Elapsed += (s, e) => { ++a; mre.Set(); };
                watchdog.Start();
                mre.Wait(_standardPaddingTimeInMilliseconds);
                Assert.AreEqual(1, a);
                Assert.IsFalse(watchdog.Enabled);
            }
        }

        [TestMethod]
        public void TestTimeoutSetProperty()
        {
            using (var watchdog = new WatchdogTimer())
            {
                int a = 0;
                watchdog.Interval = _standardInterval;
                var mre = new ManualResetEventSlim(false);
                watchdog.Elapsed += (s, e) => { ++a; mre.Set(); };
                watchdog.Start();
                mre.Wait(_standardPaddingTimeInMilliseconds);
                Assert.AreEqual(1, a);
                Assert.IsFalse(watchdog.Enabled);
            }
        }

        [TestMethod]
        public void TestDisabledAfterConstructing()
        {
            using (var watchdog = new WatchdogTimer())
            {
                Assert.IsFalse(watchdog.Enabled);
            }
            using (var watchdog = new WatchdogTimer(_standardInterval))
            {
                Assert.IsFalse(watchdog.Enabled);
            }
        }

        [TestMethod]
        public void TestStartAndStopSetEnabled()
        {
            using (var watchdog = new WatchdogTimer())
            {
                watchdog.Interval = _standardInterval;
                watchdog.Start();
                Assert.IsTrue(watchdog.Enabled);
                watchdog.Stop();
                Assert.IsFalse(watchdog.Enabled);
            }
            using (var watchdog = new WatchdogTimer(_standardInterval))
            {
                watchdog.Start();
                Assert.IsTrue(watchdog.Enabled);
                watchdog.Stop();
                Assert.IsFalse(watchdog.Enabled);
            }
        }

        [TestMethod]
        public void TestKickWatchdog()
        {
            int kicks = 0;
            bool hasWatchdogElapsed = false;
            ManualResetEventSlim doneMre = new ManualResetEventSlim(false);
            double halfTimeout = _standardInterval.TotalMilliseconds / 2;
            using (var halfTimer = new System.Timers.Timer(halfTimeout))
            {
                halfTimer.AutoReset = true;
                
                using (var watchdog = new WatchdogTimer())
                {
                    halfTimer.Elapsed += (s, e) =>
                    {
                        watchdog.Kick();
                        if (++kicks >= 5)
                            doneMre.Set();
                    };
                    watchdog.Elapsed += (s, e) =>
                    {
                        hasWatchdogElapsed = true;
                    };

                    watchdog.Start();
                    halfTimer.Start();
                    doneMre.Wait();
                    watchdog.Stop();
                    halfTimer.Stop();
                }
            }

            Assert.IsFalse(hasWatchdogElapsed);
        }
    }
}
