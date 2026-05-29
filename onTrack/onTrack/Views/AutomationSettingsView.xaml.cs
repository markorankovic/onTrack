using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;

namespace onTrack.Views
{
    public partial class AutomationSettingsView : UserControl
    {
        public AutomationSettingsView()
        {
            InitializeComponent();

            DataContext = this;

            enabled.IsChecked = Main.AutoPausePlay || Main.AutoFocus || Main.AFKMode;
            afkMode.IsChecked = Main.AFKMode;
            autoPausePlay.IsChecked = Main.AutoPausePlay;
            autoFocus.IsChecked = Main.AutoFocus;

            Pause_Record.Content = Main.AutoPauseKey != null ? "Record Again" : "Record";
            Play_Record.Content = Main.AutoPlayClickLocation != null ? "Record Again" : "Record";
            Focus_Record.Content = Main.AutoFocusClickLocation != null ? "Record Again" : "Record";

            Pause_Button_Label.Content = Main.AutoPauseKey != null ? "Pause: " + Main.AutoPauseKey.ToString() : "Pause Button";
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
            var afkModeOptionMark = (Path)afkMode.Template.FindName("optionMark", afkMode);
            var enabledOptionMark = (Path)autoPausePlay.Template.FindName("optionMark", enabled);
            SetToBlue(autoPausePlayOptionMark);
            SetToBlue(autoFocusOptionMark);
            SetToBlue(afkModeOptionMark);
            SetToBlue(enabledOptionMark);
        }

        private void AutoPausePlay_Checked(object sender, RoutedEventArgs e)
        {
            if (autoPausePlay.IsChecked == true)
            {
                Main.AutoPausePlay = true;
            }
            else
            {
                Main.AutoPausePlay = false;
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
                Main.AutoFocus = true;
            }
            else
            {
                Main.AutoFocus = false;
            }
        }

        private void afkMode_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!afkMode.IsEnabled)
            {
                afkMode.IsChecked = false;
            }
        }

        private void AFKMode_Checked(object sender, RoutedEventArgs e)
        {
            if (afkMode.IsChecked == true)
            {
                Main.AFKMode = true;
            }
            else
            {
                Main.AFKMode = false;
            }
        }

        private void enabled_Click(object sender, RoutedEventArgs e)
        {
            if (!enabled.IsChecked ?? false)
            {
                Main.AFKMode = false;
                Main.AutoPausePlay = false;
                Main.AutoFocus = false;
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

        private void RecordClickLocation(bool autoFocus = true)
        {
            MinimizeWindow();

            Window window = new Window();
            window.WindowStyle = WindowStyle.None;
            window.AllowsTransparency = true;
            window.Opacity = 0.3;
            var brush = new SolidColorBrush(Colors.Black);
            window.Background = brush;
            window.KeyDown += Window_KeyDown;

            void Window_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Escape)
                {
                    CloseThenOpenMainWindow();
                }
            }

            void CloseThenOpenMainWindow()
            {
                window.Close();
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }

            void Record(object sender, MouseButtonEventArgs e)
            {
                var pos = Mouse.GetPosition(window);
                Main.RecordClick(
                    (int) pos.X,
                    (int) pos.Y,
                    autoFocus
                );
                if (autoFocus)
                {
                    Focus_Record.Content = Main.AutoFocusClickLocation != null ? "Record Again" : "Record";
                }
                else
                {
                    Play_Record.Content = Main.AutoPlayClickLocation != null ? "Record Again" : "Record";
                }
                CloseThenOpenMainWindow();
            }

            window.MouseDown += Record;

            MaximizeRecordWindow(window);

            if (autoFocus)
                Main.SimulateNotification();
        }

        private void Play_Record_Click(object sender, RoutedEventArgs e)
        {
            if (autoPausePlay.IsChecked.Value)
            {
                RecordingType = Record.Play;
                Play_Record.Focus();
                RecordClickLocation(false);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (autoFocus.IsChecked.Value)
            {
                RecordingType = Record.Focus;
                Focus_Record.Focus();
                RecordClickLocation();
                Focus_Record.Content = Main.AutoFocusClickLocation != null ? "Record Again" : "Record";
            }
        }

        private void Pause_Record_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            Main.AutoPauseKey = key;
            Pause_Record.Content = "Record Again";
            Pause_Button_Label.Content = Main.AutoPauseKey != null ? "Pause: " + Main.AutoPauseKey.ToString() : "Pause Button";
        }

        private void Pause_Record_LostFocus(object sender, RoutedEventArgs e)
        {
            recording = false;
            Pause_Record.Content = Main.AutoPauseKey != null ? "Record Again" : "Record";
        }

        private void Play_Record_LostFocus(object sender, RoutedEventArgs e)
        {
            recording = false;
        }

        private void Focus_Record_LostFocus(object sender, RoutedEventArgs e)
        {
            recording = false;
        }

        private void Pause_Record_MouseDown(object sender, MouseButtonEventArgs e)
        {
            recording = Pause_Record.Content.Equals("Stop") ? false : true;
            if (recording && autoPausePlay.IsChecked.Value)
            {
                RecordingType = Record.Pause;
                Pause_Record.Content = "Stop";
                Pause_Record.Focus();
            }
            else
            {
                Pause_Record.Content = Main.AutoPauseKey != null ? "Record Again" : "Record";
            }
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