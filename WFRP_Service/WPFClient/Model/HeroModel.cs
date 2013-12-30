using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPFClient.Model
{
    public class HeroModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        System.Windows.Visibility _heroWindowIsVisible = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility HeroModelHeroWindowIsVisible
        {
            get
            {
                return _heroWindowIsVisible;
            }
            set
            {
                _heroWindowIsVisible = value;

                OnPropertyChanged("HeroModelHeroWindowIsVisible");
            }
        }

        string _clientName = null;
        public string HeroModelClientName
        {
            get
            {
                return _clientName;
            }
            set
            {
                _clientName = value;

                OnPropertyChanged("HeroModelClientName");
            }
        }
    }
}
