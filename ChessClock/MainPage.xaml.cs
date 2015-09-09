using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace ChessClock
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, TimeProviderListener
    {
        private List<TimeProvider> timeProviders;
        private Boolean isTimerRunning = false;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            timeProviders = new List<TimeProvider>();
            timeProviders.Add(new TimeProvider(this, buttonPlayer1, TimeProvider.newTime));
            timeProviders.Add(new TimeProvider(this, buttonPlayer2, TimeProvider.newTime));

            resetTimer();
        }

        public async void onTimeProviderTick(TimeSpan timeSpanCurrent, object sender)
        {
            Button button = (Button)sender;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => {
                button.Content = TimeProvider.formatTime(timeSpanCurrent);
            });
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void buttonPlayer_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            foreach (TimeProvider timeProvider in timeProviders)
            {
                // toggle timers if the sender belongs to the TimeProvider object currently iterated on
                // and at the same time either the time provider's timer is running (currently active button)
                // or the general isTimerRunning variable is false (it's the game's start)
                if (sender.Equals(timeProvider.getSender()) && (timeProvider.isTimerRunning() || !isTimerRunning))
                {
                    toggleTimersFromButton(sender);
                }
            }        
        }

        private void AppBarStop_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            stopTimer();
        }

        private void AppBarReset_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            resetTimer();
        }

        private void toggleTimersFromButton(object sender)
        {
            foreach (TimeProvider timeProvider in timeProviders)
            {
                if (isTimerRunning || (!isTimerRunning && sender.Equals(timeProvider.getSender())))
                {
                    timeProvider.toggleTimer();
                }
                else if (!isTimerRunning)
                {
                    ((Button)timeProvider.getSender()).IsEnabled = false;
                }         
            }

            isTimerRunning = true;
            AppBarReset.Visibility = Visibility.Visible;
            AppBarStop.Visibility = Visibility.Visible;
        }

        private void stopTimer()
        {
            foreach (TimeProvider timeProvider in timeProviders)
            {
                timeProvider.stopTimer();
            }

            isTimerRunning = false;
            AppBarStop.Visibility = Visibility.Collapsed;
        }

        private void resetTimer()
        {
            stopTimer();

            foreach (TimeProvider timeProvider in timeProviders)
            {
                timeProvider.resetTimer();
                Button button = (Button)timeProvider.getSender();
                button.Content = TimeProvider.formatTime(TimeProvider.newTime);
                button.IsEnabled = true;
            }

            AppBarReset.Visibility = Visibility.Collapsed;        
        }
    }
}
