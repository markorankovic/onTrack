using onTrack.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Windows.System;
using static System.Net.WebRequestMethods;

namespace onTrack
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            EvaluateNav();
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

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            #if DEBUG
                Launcher.LaunchUriAsync(new System.Uri("https://github.com/markorankovic/onTrack/tree/develop"));
            #else
                 Launcher.LaunchUriAsync(new System.Uri("https://github.com/markorankovic/onTrack"));
            #endif
        }

        private void EvaluateNav()
        {
            if (((MainViewModel) DataContext).SelectedViewModel is TimerViewModel)
            {
                timerNav.Opacity = 1;
                settingsNav.Opacity = 0.5;
                taskListNav.Opacity = 0.5;
            } else if (((MainViewModel)DataContext).SelectedViewModel is SettingsViewModel)
            {
                timerNav.Opacity = 0.5;
                settingsNav.Opacity = 1;
                taskListNav.Opacity = 0.5;
            } else
            {
                timerNav.Opacity = 0.5;
                settingsNav.Opacity = 0.5;
                taskListNav.Opacity = 1;
            }
        }

        private void TimerNav_Click(object sender, RoutedEventArgs e)
        {
            timerNav.Opacity = 1;
            taskListNav.Opacity = 0.5;
            settingsNav.Opacity = 0.5;
        }

        private void TaskListNav_Click(object sender, RoutedEventArgs e)
        {
            taskListNav.Opacity = 1;
            timerNav.Opacity = 0.5;
            settingsNav.Opacity = 0.5;
        }

        private void SettingsNav_Click(object sender, RoutedEventArgs e)
        {
            taskListNav.Opacity = 0.5;
            timerNav.Opacity = 0.5;
            settingsNav.Opacity = 1;
        }
    }
}