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

            TimeSpan newTime = new TimeSpan(0,0,0);

            this.timeProviderPlayer1 = new TimeProvider(this, buttonPlayer1, newTime);
            this.timeProviderPlayer2 = new TimeProvider(this, buttonPlayer2, newTime);
        }

        public async void onTimeProviderTick(TimeSpan timeSpanCurrent, object sender)
        {
            Button button = (Button)sender;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => {
                button.Content = TimeProvider.formatTime(timeSpanCurrent.Hours, timeSpanCurrent.Minutes, timeSpanCurrent.Seconds);
            });
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            buttonPlayer1.Content = "Player 1";
            buttonPlayer2.Content = "Player 2";
        }

        private void buttonPlayer1_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            toggleTimer((Button)sender);
        }

        private void buttonPlayer2_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            toggleTimer((Button)sender);
        }

        private void toggleTimer(Button sender)
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
    }
}
