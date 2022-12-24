using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using onTrack.Reinforcements;
using Windows.UI;
using Color = System.Windows.Media.Color;

namespace onTrack.Views
{
    public partial class SettingsView : UserControl { 
        public SettingsView()
        {
            InitializeComponent();

            var reinforcementRadioButtons = LogicalTreeHelper.GetChildren(reinforcements).OfType<RadioButton>();
            foreach(var rb in reinforcementRadioButtons)
            {
                if (("onTrack.Reinforcements." + rb.Name).Equals(Timer.GetReinforcement().GetType().ToString()))
                {
                    rb.IsChecked = true;
                }
            }

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
            } else
            {
                test_button.Content = "Test";
            }

            autoPausePlay.IsChecked = Timer.autoPausePlay;
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)e.OriginalSource).Content.Equals("Test"))
            {
                Timer.PlayAlarm();
                ((Button)e.OriginalSource).Content = "Stop";
            } else
            {
                Timer.StopAlarm();
                ((Button)e.OriginalSource).Content = "Test";
            }
        }

        private void Reinforcement_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch(radioButton.Content)
            {
                case "Standard": Timer.SetReinforcement(new StandardReinforcement()); return;
                case "Type out the task": Timer.SetReinforcement(new TypeOutTheGoalReinforcement()); return;
                case "Press the right goal": Timer.SetReinforcement(new PressTheRightGoalReinforcement()); return;
                case "What you gonna do now": Timer.SetReinforcement(new WhatYouGonnaDoNowReinforcement()); return;
                case "Random": Timer.SetReinforcement(new RandomReinforcement()); return;
                case "None": Timer.SetReinforcement(new NoneReinforcement()); return;
                default: return;
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

        private void SetTheOptionMarkInCheckBoxBlue()
        {
            var optionMark = (Path)autoPausePlay.Template.FindName("optionMark", autoPausePlay);
            var brush = new SolidColorBrush();
            var color = ColorTranslator.FromHtml("#00bbff");
            brush.Color = Color.FromArgb(color.A, color.R, color.G, color.B);
            optionMark.Fill = brush;
        }

        private void AutoPausePlay_Checked(object sender, RoutedEventArgs e)
        {
            if (autoPausePlay.IsChecked == true)
            {
                Timer.autoPausePlay = true;
            } else
            {
                Timer.autoPausePlay = false;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetTheOptionMarkInCheckBoxBlue();
        }
    }
}
