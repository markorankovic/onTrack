using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        }

        private void Standard_Click(object sender, RoutedEventArgs e)
        {
            Timer.SetReinforcement(new StandardReinforcement());
        }

        private void TypeOutTheTask_Click(object sender, RoutedEventArgs e)
        {
            Timer.SetReinforcement(new TypeOutTheGoalReinforcement());
        }

        private void PressTheRightYes_Click(object sender, RoutedEventArgs e)
        {
            Timer.SetReinforcement(new PressTheRightYesReinforcement());
        }

        private void WhatYouGonnaDoNow_Click(object sender, RoutedEventArgs e)
        {
            Timer.SetReinforcement(new WhatYouGonnaDoNowReinforcement());
        }

        private void None_Click(object sender, RoutedEventArgs e)
        {
            Timer.SetReinforcement(new NoneReinforcement());
        }
    }
}
