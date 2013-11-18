using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFClient.Model
{
    public class LoginModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        string _status = null;
        public string LoginModelStatus
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;

                OnPropertyChanged("LoginModelStatus");
            }
        }

        string _msg = null;
        public string LoginModelMsg
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;

                OnPropertyChanged("LoginModelMsg");
            }
        }

        string _userName = null;
        public string LoginModelUserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;

                OnPropertyChanged("LoginModelUserName");
            }
        }

        string _serverIP = null;
        public string LoginModelServerIP
        {
            get
            {
                return _serverIP;
            }
            set
            {
                _serverIP = value;

                OnPropertyChanged("LoginModelServerIP");
            }
        }

        bool _connectButtonIsEnabled = false;
        public bool LoginModelConnectButtonIsEnabled
        {
            get
            {
                return _connectButtonIsEnabled;
            }
            set
            {
                _connectButtonIsEnabled = value;

                OnPropertyChanged("LoginModelConnectButtonIsEnabled");
            }
        }

        bool _disconnectButtonIsEnabled = false;
        public bool LoginModelDisconnectButtonIsEnabled
        {
            get
            {
                return _disconnectButtonIsEnabled;
            }
            set
            {
                _disconnectButtonIsEnabled = value;

                OnPropertyChanged("LoginModelDisconnectButtonIsEnabled");
            }
        }

        string _pswd = null;
        public string LoginModelPswd
        {
            get
            {
                return _pswd;
            }
            set
            {
                _pswd = value;

                OnPropertyChanged("LoginModelPswd");
            }
        }

        string _ID = null;
        public string LoginModelID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;

                OnPropertyChanged("LoginModelID");
            }
        }

    }
}
