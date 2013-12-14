using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Service
{
    [DataContract]
    public class Client
    {
        private string _name;
        private string _password;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public bool Equals(Client other)
        {
            if (other.Name == this.Name && other.Password == this.Password)
                return true;
            else
                return false;

        }

        public override bool Equals(Object other)
        {
            return Equals(other as Client);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0) ^
                    (Password != null ? Password.GetHashCode() : 0);
        }

    }

    [DataContract(Name = "ServerMessageType")]
    public enum ServerMessageTypeEnum
    {
        [EnumMember]
        Connect,
        [EnumMember]
        DisconnectInfoClient,
        [EnumMember]
        Register,
        [EnumMember]
        Login,
        [EnumMember]
        StartSession
    }

    [DataContract]
    public class ServerMessage
    {
        private string _content;
        private bool _isStatusCorrect;
        private ServerMessageTypeEnum _type;

        [DataMember]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        [DataMember]
        public bool IsStatusCorrect
        {
            get { return _isStatusCorrect; }
            set { _isStatusCorrect = value; }
        }
        [DataMember]
        public ServerMessageTypeEnum Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }

    [DataContract]
    public class Identity
    {
        private string _accountID;

        [DataMember]
        public string AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
    }

    [DataContract]
    public class Session
    {
        private int _sessionID;
        private KeyValuePair<Client, IWFRPCallback> _MG;
        private Dictionary<Client, IWFRPCallback> _members;

        [DataMember]
        public int SessionID
        {
            get { return _sessionID; }
            set { _sessionID = value; }
        }

        [DataMember]
        public KeyValuePair<Client, IWFRPCallback> MG
        {
            get { return _MG; }
            set { _MG = value; }
        }

        [DataMember]
        public Dictionary<Client, IWFRPCallback> Members
        {
            get { return _members; }
            set { _members = value; }
        }
    }
}
