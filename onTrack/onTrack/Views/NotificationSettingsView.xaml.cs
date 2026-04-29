using onTrack.Reinforcements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace onTrack.Views
{
    /// <summary>
    /// Interaction logic for NotificationSettingsView.xaml
    /// </summary>
    public partial class NotificationSettingsView : UserControl
    {
        public NotificationSettingsView()
        {
            InitializeComponent();

            var reinforcementRadioButtons = LogicalTreeHelper.GetChildren(reinforcements).OfType<RadioButton>();
            foreach (var rb in reinforcementRadioButtons)
            {
                if (("onTrack.Reinforcements." + rb.Name).Equals(Main.GetReinforcement().GetType().ToString()))
                {
                    rb.IsChecked = true;
                }
            }
        }

        private void Reinforcement_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Content)
            {
                case "Standard": Main.SetReinforcement(new StandardReinforcement()); return;
                case "Type out the task": Main.SetReinforcement(new TypeOutTheGoalReinforcement()); return;
                case "Press the right goal": Main.SetReinforcement(new PressTheRightGoalReinforcement()); return;
                case "What you gonna do now": Main.SetReinforcement(new WhatYouGonnaDoNowReinforcement()); return;
                case "Random": Main.SetReinforcement(new RandomReinforcement()); return;
                case "None": Main.SetReinforcement(new NoneReinforcement()); return;
                default: return;
            }
        }
    }
}
