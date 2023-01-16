using onTrack.ViewModels;
using System;
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
            } else
            {
                MainViewModel.SelectedViewModel = new TaskListViewModel();
            }
        }

        public UpdateViewCommand(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }
    }
} 