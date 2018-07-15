using System.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WatchdogDotNet;

namespace UnitTests
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void TestElapsedEventPrevented()
        {
            bool watchdogElapsed = false;
            var testComplete = new System.Threading.ManualResetEventSlim(false);

            using (Timer watchdog = new Timer(50))
            {
                using (Timer resetTimer = new Timer(10))
                {
                    using (Timer testLength = new Timer(500))
                    {
                        watchdog.Elapsed += (s, e) => { watchdogElapsed = true; };
                        resetTimer.Elapsed += (s, e) => { watchdog.Restart(); };
                        testLength.Elapsed += (s, e) => { testComplete.Set(); };

                        watchdog.Start();
                        resetTimer.Start();
                        testLength.Start();

                        testComplete.Wait();

                        watchdog.Stop();
                        resetTimer.Stop();
                        testLength.Stop();
                    }
                }
            }

            Assert.IsFalse(watchdogElapsed);
        }
    }
}
