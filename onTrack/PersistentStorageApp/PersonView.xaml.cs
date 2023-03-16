using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace PersistentStorageApp
{
    public partial class PersonView : UserControl
    {
        public PersonView()
        {
            InitializeComponent();

            var addresses = new ObservableCollection<PersonModel.Address>();
            addresses.Add(new PersonModel.Address("Maddison Square"));
            addresses.Add(new PersonModel.Address("Old Trafford"));

            DataContext = new PersonModel("John", addresses);
        }
    }
}