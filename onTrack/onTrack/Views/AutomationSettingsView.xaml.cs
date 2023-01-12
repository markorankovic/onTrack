using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using WindowsInput;

namespace onTrack.Views
{
    public partial class AutomationSettingsView : UserControl
    {
        public AutomationSettingsView()
        {
            InitializeComponent();

            DataContext = this;

            enabled.IsChecked = Timer.autoPausePlay || Timer.autoFocus;
            autoPausePlay.IsChecked = Timer.autoPausePlay;
            autoFocus.IsChecked = Timer.autoFocus;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetTheOptionMarkInCheckBoxBlue();
        }

        private void SetToBlue(Path optionMark)
        {
            var brush = new SolidColorBrush();
            var color = ColorTranslator.FromHtml("#00bbff");
            brush.Color = Color.FromArgb(color.A, color.R, color.G, color.B);
            optionMark.Fill = brush;
        }

        private void SetTheOptionMarkInCheckBoxBlue()
        {
            var autoPausePlayOptionMark = (Path)autoPausePlay.Template.FindName("optionMark", autoPausePlay);
            var autoFocusOptionMark = (Path)autoPausePlay.Template.FindName("optionMark", autoFocus);
            var enabledOptionMark = (Path)autoPausePlay.Template.FindName("optionMark", enabled);
            SetToBlue(autoPausePlayOptionMark);
            SetToBlue(autoFocusOptionMark);
            SetToBlue(enabledOptionMark);
        }

        private void AutoPausePlay_Checked(object sender, RoutedEventArgs e)
        {
            if (autoPausePlay.IsChecked == true)
            {
                Timer.autoPausePlay = true;
            }
            else
            {
                Timer.autoPausePlay = false;
            }
        }

        private void autoPausePlay_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!autoPausePlay.IsEnabled)
            {
                autoPausePlay.IsChecked = false;
            }
        }

        private void autoFocus_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!autoFocus.IsEnabled)
            {
                autoFocus.IsChecked = false;
            }
        }

        private void autoFocus_Checked(object sender, RoutedEventArgs e)
        {
            if (autoFocus.IsChecked == true)
            {
                Timer.autoFocus = true;
            }
            else
            {
                Timer.autoFocus = false;
            }
        }

        private void enabled_Click(object sender, RoutedEventArgs e)
        {
            if (!enabled.IsChecked ?? false)
            {
                Timer.autoPausePlay = false;
                Timer.autoFocus = false;
            }
        }

        private enum Record
        {
            Pause,
            Play,
            Focus
        }

        Record? RecordingType = null;

        bool recording = false;

        private void MinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeRecordWindow(Window window)
        {
            window.WindowState = WindowState.Maximized;
            window.Show();
            window.Activate();
        }

        private void RecordClickLocation()
        {
            MinimizeWindow();

            Window window = new Window();
            window.WindowStyle = WindowStyle.None;
            window.AllowsTransparency = true;
            window.Opacity = 0.3;
            var brush = new SolidColorBrush(Colors.Black);
            window.Background = brush;

            int TranslateToScreenRes(double loc, double factor)
            {
                return (int)(factor * loc);
            }

            void Record(object sender, MouseButtonEventArgs e)
            {
                var pos = Mouse.GetPosition(window);
                Timer.RecordAutoFocusClick(
                    TranslateToScreenRes(pos.X, 1),
                    TranslateToScreenRes(pos.Y, 1)
                );
                window.Close();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }

            window.MouseDown += Record;

            MaximizeRecordWindow(window);

            Timer.SimulateNotification();
        }

        private void Pause_Record_Click(object sender, RoutedEventArgs e)
        {
            recording = !recording;
            if (recording && autoPausePlay.IsChecked.Value)
            {
                RecordingType = Record.Pause;
                Pause_Record.Content = "Stop";
                Pause_Record.Focus();
            } else
            {
                Pause_Record.Content = "Record";
            }
        }

        private void Play_Record_Click(object sender, RoutedEventArgs e)
        {
            recording = !recording;
            if (recording && autoPausePlay.IsChecked.Value)
            {
                RecordingType = Record.Play;
                Play_Record.Content = "Stop";
                Play_Record.Focus();
                RecordClickLocation();
            }
            else
            {
                Play_Record.Content = "Record";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            recording = !recording;
            if (recording && autoFocus.IsChecked.Value)
            {
                RecordingType = Record.Focus;
                Focus_Record.Content = "Stop";
                Focus_Record.Focus();
                RecordClickLocation();
            }
            else
            {
                Focus_Record.Content = "Record";
            }
        }

        private void Pause_Record_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            Timer.autoPauseKey = key;
        }

        private void Pause_Record_LostFocus(object sender, RoutedEventArgs e)
        {
            recording = false;
            Pause_Record.Content = "Record";
        }

        private void Play_Record_LostFocus(object sender, RoutedEventArgs e)
        {
            recording = false;
            Play_Record.Content = "Record";
        }

        private void Focus_Record_LostFocus(object sender, RoutedEventArgs e)
        {
            recording = false;
            Focus_Record.Content = "Record";
        }
    }

    public class IsCheckedToOpacity : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "1" : "0.5";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "1" ? true : false;
        }
    }
}