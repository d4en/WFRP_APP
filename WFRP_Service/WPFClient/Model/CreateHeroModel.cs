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
        public string HeroName
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
        string _eyeColor = null;
        public string EyeColor
        {
            get
            {
                return _eyeColor;
            }
            set
            {
                _eyeColor = value;

                OnPropertyChanged("CreateHeroModelEyeColor");
            }
        }
        string _sex = null;
        public string Sex
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
        string _race = null;
        public string Race
        {
            get
            {
                return _race;
            }
            set
            {
                _race = value;
                
                OnPropertyChanged("CreateHeroModelRace");
            }
        }
        string _age = null;
        public string Age
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
        public string Height
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
        public string Weight
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
        public string Origin
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
        public string Family
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
    }
}
