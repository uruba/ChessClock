using System;
using System.Diagnostics;
using System.Threading;

namespace ChessClock
{
    public sealed class TimeProvider
    {
        private TimeSpan timeSpanTarget;
        private Stopwatch timer;

        private readonly TimeProviderListener listener;
        private readonly object sender;
        private readonly TimerCallback timerCallback;
        private readonly static int timerTickLength = 10;
        private readonly TimeSpan timeSpanIncrement = new TimeSpan(0, 0, 0, 0, timerTickLength);
        private readonly Timer cycleTimer;

        public TimeProvider(TimeProviderListener listener, object sender)
        {
            this.timer = new Stopwatch();
            this.listener = listener;
            this.sender = sender;
            timerCallback = new TimerCallback(timerTick);
            cycleTimer = new Timer(this.timerTick, null, 0, 10);
        }

        public TimeProvider(TimeProviderListener listener, object sender, TimeSpan timeSpan) : this(listener, sender)
        {
            setTime(timeSpan);
        }

        public void setTime(TimeSpan timeSpan)
        {
            this.timeSpanTarget = timeSpan;
        }

        public void startTimer()
        {
            timer.Start();
        }

        public void stopTimer()
        {
            timer.Stop();
        }

        public void toggleTimer()
        {
            if (isTimerRunning())
            {
                stopTimer();
            }
            else
            {
                startTimer();
            }
        }

        public void resetTimer()
        {
            timer.Reset();
        }

        public bool isTimerRunning()
        {
            return timer.IsRunning;
        }

        public static String formatTime(int hours, int minutes, int seconds)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        private void timerTick(object state)
        {
            listener.onTimeProviderTick(timer.Elapsed, sender);
        }
    }

    public interface TimeProviderListener
    {
        void onTimeProviderTick(TimeSpan timeSpanCurrent, object sender);
    }
}
