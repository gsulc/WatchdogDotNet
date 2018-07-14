using System;
using System.Timers;

namespace WatchdogDotNet
{
    public class WatchdogTimer : IDisposable
    {
        public delegate void TimerElapsedEventHandler(object sender, EventArgs e);
        public event TimerElapsedEventHandler Elapsed;

        private readonly Timer _timer;
        private TimeSpan _interval;

        public WatchdogTimer()
        {
            _timer = new Timer();
            _timer.AutoReset = false;
            _timer.Elapsed += OnTimerElapsed;
        }

        public WatchdogTimer(TimeSpan interval)
        {
            _interval = interval;
            _timer = new Timer(interval.TotalMilliseconds);
            _timer.AutoReset = false;
            _timer.Elapsed += OnTimerElapsed;
        }
        
        public TimeSpan Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                _timer.Interval = _interval.TotalMilliseconds;
            }
        }

        public bool Enabled
        {
            get => _timer.Enabled;
            set => _timer.Enabled = value;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Kick()
        {
            _timer.Stop();
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed(this, e);
        }

        #region IDisposable Support
        private bool _isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _timer.Dispose();
                }

                _isDisposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
