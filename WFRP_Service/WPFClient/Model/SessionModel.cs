using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPFClient.Model
{
    public class SessionModel : INotifyPropertyChanged
    {

        public static int maxChatSize = 1000;

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        System.Windows.Visibility _sessionWindowIsVisible = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility SessionModelSessionWindowIsVisible
        {
            get
            {
                return _sessionWindowIsVisible;
            }
            set
            {
                _sessionWindowIsVisible = value;

                OnPropertyChanged("SessionModelSessionWindowIsVisible");
            }
        }

        List<string> _membersListBox = null;
        public List<string> SessionModelMembersListBox
        {
            get
            {
                return _membersListBox;
            }
            set
            {
                _membersListBox = value;

                OnPropertyChanged("SessionModelMembersListBox");
            }
        }

        string _selectedMember = null;
        public string SessionModelSelectedMember
        {
            get
            {
                return _selectedMember;
            }
            set
            {
                _selectedMember = value;

                OnPropertyChanged("SessionModelSelectedMember");
            }
        }

        string _chatTextBox = "";
        public string SessionModelChat
        {
            get
            {
                return _chatTextBox;
            }
            set
            {
                _chatTextBox = value;

                OnPropertyChanged("SessionModelChat");
            }
        }

        List<string> _chatList = new List<string>();
        public List<string> SessionModelChatList
        {
            get
            {
                return _chatList;
            }
            set
            {
                _chatList = value;

                OnPropertyChanged("SessionModelChatList");
            }
        }

        string _msgTextBox = "";
        public string SessionModelMsg
        {
            get
            {
                return _msgTextBox;
            }
            set
            {
                _msgTextBox = value;

                OnPropertyChanged("SessionModelMsg");
            }
        }

    }
}
