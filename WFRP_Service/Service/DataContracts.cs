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
        private KeyValuePair<Client, IWFRPCallback> _MG = new KeyValuePair<Client, IWFRPCallback>();
        private Dictionary<Client, IWFRPCallback> _members = new Dictionary<Client, IWFRPCallback>();

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

    [DataContract]
    public class Message
    {
        private string _content;
        private string _sender;
        private string _receiver;

        [DataMember]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        [DataMember]
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        [DataMember]
        public string Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

    }

    [DataContract]
    public class FileMessage
    {
        private string _fileName;
        private byte[] _data;

        [DataMember]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        [DataMember]
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }

    [DataContract]
    public class Hero
    {
        private string _clientName;

        [DataMember]
        public string ClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }
    }

    [DataContract]
    public class HeroDetails_Eyes
    {
        private string _HeroEyes;

        [DataMember]
        public string HeroEyes
        {
            get { return _HeroEyes; }
            set { _HeroEyes = value; }
        }
    }

    [DataContract]
    public class AllHeroOccupations
    {
        List<string> _HeroOccupations;

        [DataMember]
        public List<string> HeroOccupations
        {
            get { return _HeroOccupations; }
            set { _HeroOccupations = value; }
        }
    }

    [DataContract]
    public class OccupationInfo
    {
        private string _info;

        [DataMember]
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }
    }
}
