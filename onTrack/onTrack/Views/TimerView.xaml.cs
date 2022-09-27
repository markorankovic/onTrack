using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using onTrack.Reinforcements;
using Windows.Foundation.Collections;

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
            if (((System.Windows.Controls.Slider)e.OriginalSource).Maximum != 30) 
            { 
                return; 
            }
            Timer.SetDuration(slTime.Value);
        }

        private void slTime_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            Trace.WriteLine("Tool tip opening");
        }
    }
}