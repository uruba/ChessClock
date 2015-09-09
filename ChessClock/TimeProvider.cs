using System;
using System.Diagnostics;
using System.Threading;
using Windows.UI.Xaml.Controls;

namespace ChessClock
{
    public sealed class TimeProvider
    {
        public static readonly TimeSpan newTime = new TimeSpan(0, 0, 0);

        private TimeSpan timeSpanTarget;
        private Stopwatch timer;
        private Timer cycleTimer;

        private readonly TimeProviderListener listener;
        private readonly object sender;
        private readonly TimerCallback timerCallback;
        private readonly static int timerTickLength = 10;
        private readonly TimeSpan timeSpanIncrement = new TimeSpan(0, 0, 0, 0, timerTickLength);      

        public TimeProvider(TimeProviderListener listener, object sender)
        {
            this.timer = new Stopwatch();
            this.listener = listener;
            this.sender = sender;
            timerCallback = new TimerCallback(timerTick);
        }

        public TimeProvider(TimeProviderListener listener, object sender, TimeSpan timeSpan) : this(listener, sender)
        {
            setTime(timeSpan);
        }

        public void setTime(TimeSpan timeSpan)
        {
            this.timeSpanTarget = timeSpan;
        }

        public object getSender()
        {
            return sender;
        }

        public void startTimer()
        {
            cycleTimer = new Timer(this.timerTick, null, 0, 10);
            timer.Start();
        }

        public void stopTimer()
        {
            if (cycleTimer != null)
            {
                cycleTimer.Dispose();
            }       
            timer.Stop();
        }

        public void toggleTimer()
        {
            if (isTimerRunning())
            {
                stopTimer();
                ((Button)sender).IsEnabled = false;
            }
            else
            {
                startTimer();
                ((Button)sender).IsEnabled = true;
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

        public static String formatTime(TimeSpan timeSpan)
        {
            return formatTime(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
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
