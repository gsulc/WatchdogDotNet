using System.Timers;

namespace WatchdogDotNet
{
    public static class WatchdogExtensions
    {
        /// <summary>
        /// Stops and Starts the timer. Used to kick a watchdog to prevent the timer Elapsed event.
        /// </summary>
        public static void Restart(this Timer timer)
        {
            timer.Stop();
            timer.Start();
        }
    }
}
