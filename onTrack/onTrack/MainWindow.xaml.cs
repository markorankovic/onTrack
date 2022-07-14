using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.Notifications;

namespace onTrack
{
    public partial class MainWindow : Window
    {
        static Timer timer;
        SoundPlayer soundPlayer;

        static string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string sFile = Path.Combine(sCurrentDirectory, @"..\..\..\alert.wav");
        static string sFilePath = Path.GetFullPath(sFile);

        string alertSoundPath = sFilePath;

        public static RoutedCommand MyCommand = new RoutedCommand();

        public MainWindow()
        {
            soundPlayer = new(alertSoundPath);

            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                Dispatcher.Invoke(() =>
                {
                    Reset();
                });
            };

            InitializeComponent();
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
            new ToastContentBuilder()
                .AddText("Are you focusing?")
                .AddText("Objective: " + objective.Text)
                .AddButton(new ToastButton()
                    .SetContent("Yes")
                    .AddArgument("action", "wakeup")
                    .SetBackgroundActivation()
                )
                .Show();
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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}