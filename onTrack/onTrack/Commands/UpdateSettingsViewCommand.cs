using onTrack.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace onTrack.Commands
{
    public class UpdateSettingsViewCommand : ICommand
    {
        public SettingsViewModel SettingsViewModel;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter.ToString() == "NotificationSettings")
            {
                SettingsViewModel.SelectedViewModel = new NotificationSettingsViewModel();
            }
            else if (parameter.ToString() == "AutomationSettings")
            {
                SettingsViewModel.SelectedViewModel = new AutomationSettingsViewModel();
            }
            else if (parameter.ToString() == "AudioSettings")
            {
                SettingsViewModel.SelectedViewModel = new AudioSettingsViewModel();
            }
        }

        public UpdateSettingsViewCommand(SettingsViewModel settingsViewModel)
        {
            SettingsViewModel = settingsViewModel;
        }
    }
}