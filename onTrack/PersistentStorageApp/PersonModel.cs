using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PersistentStorageApp
{
    public class PersonModel
    {
        string _name;
        public string name {
            get { return _name; }
            set {
                _name = value;
            }
        }

        ObservableCollection<Address> _addresses;
        public ObservableCollection<Address> addresses {
            get { return _addresses; }
            set {
                _addresses = value;
            }
        }

        public class Address {
            public string _line;
            public string line { set { _line = value; } get { return _line; } }
            public Address(string line)
            {
                this._line = line;
            }
        }

        public PersonModel(string name, ObservableCollection<Address> addresses)
        {
            this._name = name;
            this._addresses = addresses;
        }
    }
}