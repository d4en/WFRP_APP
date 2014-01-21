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

    [DataContract]
    public class HeroBasicInfo
    {
        private int _accountID;
        private string _heroName;
        private string _race;
        private string _sex;
        private string _eyeColor;
        private string _age;
        private string _height;
        private string _weight;
        private string _origin;
        private string _family;
        private string _socialPosition;
        private string _HHWB;
        private string _whyTravel;
        private string _friends;
        private string _enemies;
        private string _likes;
        private string _dontLikes;
        private string _personality;
        private string _motivation;
        private string _whoHeServes;

        [DataMember]
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        [DataMember]
        public string HeroName
        {
            get { return _heroName; }
            set { _heroName = value; }
        }

        [DataMember]
        public string Race
        {
            get { return _race; }
            set { _race = value; }
        }

        [DataMember]
        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        [DataMember]
        public string EyeColor
        {
            get { return _eyeColor; }
            set { _eyeColor = value; }
        }

        [DataMember]
        public string Age
        {
            get { return _age; }
            set { _age = value; }
        }

        [DataMember]
        public string Height
        {
            get { return _height; }
            set { _height = value; }
        }

        [DataMember]
        public string Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        [DataMember]
        public string Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        [DataMember]
        public string Family
        {
            get { return _family; }
            set { _family = value; }
        }

        [DataMember]
        public string SocialPosition
        {
            get { return _socialPosition; }
            set { _socialPosition = value; }
        }

        [DataMember]
        public string HHWB
        {
            get { return _HHWB; }
            set { _HHWB = value; }
        }

        [DataMember]
        public string WhyTravel
        {
            get { return _whyTravel; }
            set { _whyTravel = value; }
        }

        [DataMember]
        public string Friends
        {
            get { return _friends; }
            set { _friends = value; }
        }

        [DataMember]
        public string Enemies
        {
            get { return _enemies; }
            set { _enemies = value; }
        }

        [DataMember]
        public string Likes
        {
            get { return _likes; }
            set { _likes = value; }
        }

        [DataMember]
        public string DontLikes
        {
            get { return _dontLikes; }
            set { _dontLikes = value; }
        }

        [DataMember]
        public string Personality
        {
            get { return _personality; }
            set { _personality = value; }
        }

        [DataMember]
        public string Motivation
        {
            get { return _motivation; }
            set { _motivation = value; }
        }

        [DataMember]
        public string WhoHeServes
        {
            get { return _whoHeServes; }
            set { _whoHeServes = value; }
        }
    
    }

    [DataContract]
    public class OccupationAndRaceInfo
    {
        private string _occupationSkills;
        private string _occupationAbilities;
        private string _raceSkills;
        private string _raceAbilities;

        [DataMember]
        public string OccupationSkills
        {
            get { return _occupationSkills; }
            set { _occupationSkills = value; }
        }

        [DataMember]
        public string OccupationAbilities
        {
            get { return _occupationAbilities; }
            set { _occupationAbilities = value; }
        }

        [DataMember]
        public string RaceSkills
        {
            get { return _raceSkills; }
            set { _raceSkills = value; }
        }

        [DataMember]
        public string RaceAbilities
        {
            get { return _raceAbilities; }
            set { _raceAbilities = value; }
        }
    }

    [DataContract]
    public class HeroRaceAndOccupation
    {
        private string _race;
        private string _occupation;

        [DataMember]
        public string Race
        {
            get { return _race; }
            set { _race = value; }
        }

        [DataMember]
        public string Occupation
        {
            get { return _occupation; }
            set { _occupation = value; }
        }
    }
}
