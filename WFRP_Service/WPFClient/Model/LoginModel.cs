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

        System.Windows.Visibility _loginWindowIsVisible = System.Windows.Visibility.Visible;
        public System.Windows.Visibility LoginModelLoginWindowIsVisible
        {
            get
            {
                return _loginWindowIsVisible;
            }
            set
            {
                _loginWindowIsVisible = value;

                OnPropertyChanged("LoginModelLoginWindowIsVisible");
            }
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

        bool _LoginExpander = true;
        public bool LoginModelExpander
        {
            get
            {
                return _LoginExpander;
            }
            set
            {
                _LoginExpander = value;

                OnPropertyChanged("LoginModelExpander");
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

        bool _registerExpander = true;
        public bool LoginModelRegExpander
        {
            get
            {
                return _registerExpander;
            }
            set
            {
                _registerExpander = value;

                OnPropertyChanged("LoginModelRegExpander");
            }
        }

        string _registerName = null;
        public string LoginModelRegUserName
        {
            get
            {
                return _registerName;
            }
            set
            {
                _registerName = value;

                OnPropertyChanged("LoginModelRegUserName");
            }
        }

        string _userPassword = null;
        public string LoginModelRegNewPsw
        {
            get
            {
                return _userPassword;
            }
            set
            {
                _userPassword = value;

                OnPropertyChanged("LoginModelRegNewPsw");
            }
        }

        string _userRePassword = null;
        public string LoginModelRegNewRePsw
        {
            get
            {
                return _userRePassword;
            }
            set
            {
                _userRePassword = value;

                OnPropertyChanged("LoginModelRegNewRePsw");
            }
        }

        string _registerStatus = null;
        public string LoginModelRegStatus
        {
            get
            {
                return _registerStatus;
            }
            set
            {
                _registerStatus = value;

                OnPropertyChanged("LoginModelRegStatus");
            }
        }

        bool _registerButtonIsEnabled = false;
        public bool LoginModelRegisterButtonIsEnabled
        {
            get
            {
                return _registerButtonIsEnabled;
            }
            set
            {
                _registerButtonIsEnabled = value;

                OnPropertyChanged("LoginModelRegisterButtonIsEnabled");
            }
        }
    }
}
