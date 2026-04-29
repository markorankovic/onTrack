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
                if (rb.Content.Equals(Main.GetAlarmName()))
                {
                    rb.IsChecked = true;
                }
            }

            if (Main.SoundPlaying)
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
                Main.PlayAlarm();
                ((Button)e.OriginalSource).Content = "Stop";
            }
            else
            {
                Main.StopAlarm();
                ((Button)e.OriginalSource).Content = "Test";
            }
        }

        private void AlarmSound_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Content)
            {
                case "Evacuation": Main.SetAlarmName("Evacuation"); return;
                case "Wake Up": Main.SetAlarmName("Wake Up"); return;
                case "Police": Main.SetAlarmName("Police"); return;
                default: return;
            }
        }
    }
}
