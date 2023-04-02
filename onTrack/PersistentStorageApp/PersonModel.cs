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

        Address _address;
        public Address address {
            get { return _address; }
            set {
                _address = value;
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

        public PersonModel(string name, Address address)
        {
            this._name = name;
            this._address = address;
        }
    }
}