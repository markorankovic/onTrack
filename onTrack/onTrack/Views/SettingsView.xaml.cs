using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Toolkit.Uwp.Notifications;
using onTrack.Reinforcements;
using Windows.Foundation.Collections;

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
                case "Press the right yes": Timer.SetReinforcement(new PressTheRightYesReinforcement()); return;
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
    }
}
