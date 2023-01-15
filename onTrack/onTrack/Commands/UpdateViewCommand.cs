using onTrack.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace onTrack.Commands
{
    public class UpdateViewCommand : ICommand
    {
        public MainViewModel MainViewModel;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter.ToString() == "Timer")
            {
                MainViewModel.SelectedViewModel = new TimerViewModel();
            } else if (parameter.ToString() == "Settings")
            {
                MainViewModel.SelectedViewModel = new SettingsViewModel();
            }
        }

        public UpdateViewCommand(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }
    }
} 