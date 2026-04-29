using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace onTrack.Views
{
    public partial class TimerView : UserControl
    {
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (Main.Playing)
            {
                Main.Stop();
                PlayButton.Content = "▶️ Start";
            }
            else
            {
                Main.Reset();
                PlayButton.Content = "⏹️ Stop";
            }
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
        }

        public TimerView()
        {
            InitializeComponent();
            DataContext = ((TaskTree)Application.Current.Resources["taskList"]);
            Trace.WriteLine(Resources["mouseover"]);
            PlayButton.Content = Main.Playing ? "⏹️ Stop" : "▶️ Start";
        }

        private void objective_TextChanged(object sender, TextChangedEventArgs e)
        {
            Main.SetObjective(objective.Text);
        }


        private void slTime_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            Trace.WriteLine("Tool tip opening");
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            objective.MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
            countdown.MoveFocus();
        }
    }
}