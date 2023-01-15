using onTrack.ViewModels;
using System.Windows.Controls;

namespace onTrack.Views
{
    public partial class SettingsView : UserControl { 
        public SettingsView()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
            notifications_button.Opacity = 1;
            automation_button.Opacity = 0.5;
            audio_button.Opacity = 0.5;
        }

        private void Notifications_Selected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((SettingsViewModel) DataContext).UpdateViewCommand.Execute("NotificationSettings");
            notifications_button.Opacity = 1;
            automation_button.Opacity = 0.5;
            audio_button.Opacity = 0.5;
        }

        private void Automation_Selected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((SettingsViewModel)DataContext).UpdateViewCommand.Execute("AutomationSettings");
            notifications_button.Opacity = 0.5;
            automation_button.Opacity = 1;
            audio_button.Opacity = 0.5;
        }

        private void Audio_Selected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((SettingsViewModel)DataContext).UpdateViewCommand.Execute("AudioSettings");
            notifications_button.Opacity = 0.5;
            automation_button.Opacity = 0.5;
            audio_button.Opacity = 1;
        }
    }
}