using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace PersistentStorageApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var address = new PersonModel.Address("Maddison Square");
            var Person = new PersonModel("John", address);

            DataContext = Person;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}