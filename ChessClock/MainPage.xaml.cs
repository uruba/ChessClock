using System;
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
        TimeProvider timeProviderPlayer1, timeProviderPlayer2;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
      
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
            resetTimer();
        }

        private void buttonPlayer_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            toggleTimer(sender);
        }

        private void AppBarStop_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            stopTimer();
        }

        private void AppBarReset_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            resetTimer();
        }

        private void toggleTimer(object sender)
        {
            bool isTimerPlayer1Running = timeProviderPlayer1.isTimerRunning();
            bool isTimerPlayer2Running = timeProviderPlayer2.isTimerRunning();

            if (isTimerPlayer1Running || isTimerPlayer2Running)
            {
                if (
                    (isTimerPlayer1Running && sender.Equals(buttonPlayer1)) 
                    || 
                    (isTimerPlayer2Running && sender.Equals(buttonPlayer2))
                    ) {
                    timeProviderPlayer1.toggleTimer();
                    timeProviderPlayer2.toggleTimer();
                }

                return;
            }
            

            if (sender.Equals(buttonPlayer1))
            {
                timeProviderPlayer1.startTimer();
            }
            else if (sender.Equals(buttonPlayer2))
            {
                timeProviderPlayer2.startTimer();
            }
        }

        private void stopTimer()
        {
            if (timeProviderPlayer1 != null)
            {
                timeProviderPlayer1.stopTimer();
            }
            
            if (timeProviderPlayer2 != null)
            {
                timeProviderPlayer2.stopTimer();
            } 
        }

        private void resetTimer()
        {
            stopTimer();

            TimeSpan newTime = new TimeSpan(0, 0, 0);

            timeProviderPlayer1 = new TimeProvider(this, buttonPlayer1, newTime);
            timeProviderPlayer2 = new TimeProvider(this, buttonPlayer2, newTime);

            buttonPlayer1.Content = TimeProvider.formatTime(newTime);
            buttonPlayer2.Content = TimeProvider.formatTime(newTime);           
        }
    }
}
