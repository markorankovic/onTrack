using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PersistentStorageApp
{
    public partial class PersonView : UserControl
    {
        public PersonModel Person
        {
            get { return (PersonModel)GetValue(PersonProperty); }
            set { 
                SetValue(PersonProperty, value); 
            }
        }

        public static readonly DependencyProperty PersonProperty =
            DependencyProperty.Register(
                "Person",
                typeof(PersonModel),
                typeof(PersonView),
                new PropertyMetadata(
                    new PersonModel(
                        "Joe",
                        new PersonModel.Address("Joe's Pizza, NY")
                    )
                )
            );

        public PersonView()
        {
            InitializeComponent();
        }
    }
}