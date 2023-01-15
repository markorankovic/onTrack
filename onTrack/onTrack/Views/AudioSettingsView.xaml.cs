using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace onTrack.Views
{
    public partial class AudioSettingsView : UserControl
    {
        public AudioSettingsView()
        {
            InitializeComponent();
            var alarmSoundRadioButtons = LogicalTreeHelper.GetChildren(alarmSound).OfType<RadioButton>();
            foreach (var rb in alarmSoundRadioButtons)
            {
                if (rb.Content.Equals(Timer.GetAlarmName()))
                {
                    rb.IsChecked = true;
                }
            }

            if (Timer.SoundPlaying)
            {
                test_button.Content = "Stop";
            }
            else
            {
                test_button.Content = "Test";
            }
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)e.OriginalSource).Content.Equals("Test"))
            {
                Timer.PlayAlarm();
                ((Button)e.OriginalSource).Content = "Stop";
            }
            else
            {
                Timer.StopAlarm();
                ((Button)e.OriginalSource).Content = "Test";
            }
        }

        private void AlarmSound_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Content)
            {
                case "Evacuation": Timer.SetAlarmName("Evacuation"); return;
                case "Wake Up": Timer.SetAlarmName("Wake Up"); return;
                case "Police": Timer.SetAlarmName("Police"); return;
                default: return;
            }
        }
    }
}
