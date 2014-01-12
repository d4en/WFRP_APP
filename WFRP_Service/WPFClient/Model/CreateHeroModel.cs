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
    
    }
}
