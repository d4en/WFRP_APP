using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFClient.Model
{
    public class OptionsModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        System.Windows.Visibility _optionsWindowIsVisible = System.Windows.Visibility.Visible;
        public System.Windows.Visibility OptionsModelOptionsWindowIsVisible
        {
            get
            {
                return _optionsWindowIsVisible;
            }
            set
            {
                _optionsWindowIsVisible = value;

                OnPropertyChanged("OptionsModelOptionsWindowIsVisible");
            }
        }

        bool _disconnectButtonIsEnabled = false;
        public bool OptionsModelDisconnectButtonIsEnabled
        {
            get
            {
                return _disconnectButtonIsEnabled;
            }
            set
            {
                _disconnectButtonIsEnabled = value;

                OnPropertyChanged("OptionsModelDisconnectButtonIsEnabled");
            }
        }

        string _status = null;
        public string OptionsModelStatus
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;

                OnPropertyChanged("OptionsModelStatus");
            }
        }

        string _msg = null;
        public string OptionsModelMsg
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;

                OnPropertyChanged("OptionsModelMsg");
            }
        }

        string _ID = null;
        public string OptionsModelID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;

                OnPropertyChanged("OptionsModelID");
            }
        }

        List<string> _clientListBox = null;
        public List<string> OptionsModelClientListBox
        {
            get
            {
                return _clientListBox;
            }
            set
            {
                _clientListBox = value;

                OnPropertyChanged("OptionsModelClientListBox");
            }
        }

        List<string> _clientListBoxSelectedItems = new List<string>();
        public List<string> OptionsModelClientListBoxSelectedItems
        {
            get
            {
                return _clientListBoxSelectedItems;
            }
            set
            {
                _clientListBoxSelectedItems = value;

                OnPropertyChanged("OptionsModelClientListBoxSelectedItems");
            }
        }

        bool _startButtonIsEnabled = false;
        public bool OptionsModelStartButtonIsEnabled
        {
            get
            {
                return _startButtonIsEnabled;
            }
            set
            {
                _startButtonIsEnabled = value;

                OnPropertyChanged("OptionsModelStartButtonIsEnabled");
            }
        }

    }
}
