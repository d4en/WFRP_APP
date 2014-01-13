using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPFClient.Model
{
    public class CreateHeroModel: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        System.Windows.Visibility _createheroWindowIsVisible = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility CreateHeroModelCreateHeroWindowIsVisible
        {
            get
            {
                return _createheroWindowIsVisible;
            }
            set
            {
                _createheroWindowIsVisible = value;

                OnPropertyChanged("CreateHeroModelCreateHeroWindowIsVisible");
            }
        }
        string _heroName = null;
        public string CreateHeroModelHeroName
        {
            get
            {
                return _heroName;
            }
            set
            {
                _heroName = value;

                OnPropertyChanged("CreateHeroModelHeroName");
            }
        }
        string _sex = null;
        public string CreateHeroModelSex
        {
            get
            {
                return _sex;
            }
            set
            {
                _sex = value;

                OnPropertyChanged("CreateHeroModelSex");
            }
        }
        List<string> _source = new List<string> { "człowiek", "elf", "krasnolud", "niziołek" };
        string _selectedItem = null;
        public List<string> Source { get { return _source; } }
        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
            }
        }
        List<string> _eyeColor = new List<String>();
        public List<string> HeroModelEyeColor
        {
            get
            {
                return _eyeColor;
            }
            set
            {
                _eyeColor = value;
                OnPropertyChanged("HeroModelEyeColor");
            }
        }
        public List<string> _occupation = new List<string>();
        public List<string> CreateHeroModelOccupation
        {
            get
            {
                return _occupation;
            }
            set
            {
                _occupation = value;

                OnPropertyChanged("CreateHeroModelOccupation");
            }
        }
        public string _occupationItem = null;
        public string CreateHeroModelOccupationItem
        {
            get
            {
                return _occupationItem;
            }
            set
            {
                _occupationItem = value;

                OnPropertyChanged("CreateHeroModelOccupationItem");
            }
        }
        public string _occupationText = null;
        public string CreateHeroModelOccupationText
        {
            get
            {
                return _occupationText;
            }
            set
            {
                _occupationText = value;

                OnPropertyChanged("CreateHeroModelOccupationText");
            }
        }
        string _age = null;
        public string CreateHeroModelAge
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;

                OnPropertyChanged("CreateHeroModelAge");
            }
        }
        string _height = null;
        public string CreateHeroModelHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;

                OnPropertyChanged("CreateHeroModelHeight");
            }
        }
        string _weight = null;
        public string CreateHeroModelWeight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;

                OnPropertyChanged("CreateHeroModelWeight");
            }
        }
        string _origin = null;
        public string CreateHeroModelOrigin
        {
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;

                OnPropertyChanged("CreateHeroModelOrigin");
            }
        }
        string _family = null;
        public string CreateHeroModelFamily
        {
            get
            {
                return _family;
            }
            set
            {
                _family = value;

                OnPropertyChanged("CreateHeroModelFamily");
            }
        }
        bool _submitButtonIsEnabled = true;
        public bool CreateHeroModelSubmitIsEnabled
        {
            get
            {
                return _submitButtonIsEnabled;
            }
            set
            {
                _submitButtonIsEnabled = value;

                OnPropertyChanged("CreateHeroModelSubmitIsEnabled");
            }
        }
        
    }
}
