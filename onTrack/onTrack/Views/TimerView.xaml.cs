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
            Timer.Reset();
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        public TimerView()
        {
            InitializeComponent();
            Trace.WriteLine(Resources["mouseover"]);
        }

        private void objective_TextChanged(object sender, TextChangedEventArgs e)
        {
            Timer.SetObjective(objective.Text);
        }

        private void slTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (((Slider)e.OriginalSource).Maximum != 30) 
            { 
                return; 
            }
            Timer.SetDuration(slTime.Value);
        }

        private void slTime_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            Trace.WriteLine("Tool tip opening");
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            objective.MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }
    }
}