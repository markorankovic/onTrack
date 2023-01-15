using onTrack.Commands;
using System.Windows.Input;
namespace onTrack.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        internal BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel { get { return _selectedViewModel; } set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); } }

        public ICommand UpdateViewCommand { get; set; }

        public SettingsViewModel()
        {
            UpdateViewCommand = new UpdateSettingsViewCommand(this);
            _selectedViewModel = new NotificationSettingsViewModel();
        }
    }
}