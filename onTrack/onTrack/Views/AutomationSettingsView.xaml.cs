using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

            autoPausePlay.IsChecked = Timer.autoPausePlay;
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
