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
    }
}