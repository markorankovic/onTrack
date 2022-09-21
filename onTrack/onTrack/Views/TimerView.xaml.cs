using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Foundation.Collections;

namespace onTrack.Views
{
    public interface Reinforcement
    {
        public ToastContentBuilder CreateToast(string goal) { return null; }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs) { return false; }
    }

    public class TypeOutTheGoalReinforcement: Reinforcement
    {
        string Goal = null;
        public ToastContentBuilder CreateToast(string goal)
        {
            this.Goal = goal;
            return new ToastContentBuilder()
                .AddText("Are you focusing?")
                .AddText("Objective: " + goal)
                .AddInputTextBox("tbReply", "Type out the goal here")
                .AddButton(new ToastButton()
                    .SetContent("Submit")
                    .AddArgument("action", "wakeup")
                    .SetBackgroundActivation()
                );
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs) 
        {
            string typedOutGoal = toastArgs.UserInput["tbReply"].ToString();
            if (typedOutGoal == Goal)
            {
                return true;
            }
            return false;
        }
    }

    public class StandardReinforcement : Reinforcement
    {
        string Goal = null;
        public ToastContentBuilder CreateToast(string goal)
        {
            this.Goal = goal;
            return new ToastContentBuilder()
                .AddText("Are you focusing?")
                .AddText("Objective: " + goal)
                .AddButton(new ToastButton()
                    .SetContent("Yes")
                    .AddArgument("action", "wakeup")
                    .SetBackgroundActivation()
                );
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            return true;
        }
    }

    public partial class TimerView : UserControl {
        static Timer timer;
        SoundPlayer soundPlayer;

        Reinforcement CurrentReinforcement = new StandardReinforcement();

        static string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string sFile = Path.Combine(sCurrentDirectory, @"..\..\..\alert.wav");
        static string sFilePath = Path.GetFullPath(sFile);

        string alertSoundPath = sFilePath;

        public static RoutedCommand MyCommand = new RoutedCommand();

        public TimerView()
        {
            InitializeComponent();

            soundPlayer = new(alertSoundPath);

            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (CurrentReinforcement.IsValidResponse(toastArgs))
                    {
                        Reset();
                    }
                });
            };
        }

        private void WakeUser()
        {
            soundPlayer.Play();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void ResetTimer()
        {
            float duration = (float)slTime.Value * 60 * 1000;
            Trace.WriteLine("Duration: " + duration);
            timer?.Stop();
            timer = new(duration);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void Reset()
        {
            Dispatcher.Invoke(() => {
                soundPlayer.Stop();
                ResetTimer();
            });
        }

        private void AlertUser()
        {
            CurrentReinforcement.CreateToast(objective.Text).Show();
        }

        private void OnTimedEvent(System.Object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                AlertUser();
                timer = new(10 * 1000);
                timer.Elapsed += OnToastPassed;
                timer.AutoReset = false;
                timer.Enabled = true;
            });
        }

        private void OnToastPassed(System.Object source, ElapsedEventArgs e)
        {
            WakeUser();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            soundPlayer.Stop();
            timer?.Stop();
        }
    }
}