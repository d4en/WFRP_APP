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
        StartSession,
        [EnumMember]
        AddedSnA,
        [EnumMember]
        AddedStartStats
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

    [DataContract]
    public class HeroAbilitiesAndSkills
    {
        private string _occupation;
        private string _IDAcc;
        private List<string> _skills;
        private List<string> _abilities;

        [DataMember]
        public string Occupation
        {
            get { return _occupation; }
            set { _occupation = value; }
        }

        [DataMember]
        public string IDAcc
        {
            get { return _IDAcc; }
            set { _IDAcc = value; }
        }

        [DataMember]
        public List<string> Skills
        {
            get { return _skills; }
            set { _skills = value; }
        }

        [DataMember]
        public List<string> Abilities
        {
            get { return _abilities; }
            set { _abilities = value; }
        }
    }

    [DataContract]
    public class AbilityNames
    {
        private List<string> _names;

        [DataMember]
        public List<string> Names
        {
            get { return _names; }
            set { _names = value; }
        }
    }

    [DataContract]
    public class SkillNames
    {
        private List<string> _names;

        [DataMember]
        public List<string> Names
        {
            get { return _names; }
            set { _names = value; }
        }
    }

    [DataContract]
    public class FullAbilityInfo
    {
        private string _name;
        private string _description;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }

    [DataContract]
    public class FullSkillInfo
    {
        private string _name;
        private string _description;
        private string _simAtt;
        private string _att;
        private string _type;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string SimAtt
        {
            get { return _simAtt; }
            set { _simAtt = value; }
        }

        [DataMember]
        public string Att
        {
            get { return _att; }
            set { _att = value; }
        }

        [DataMember]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }

    [DataContract]
    public class StartStats
    {
        private string _id;
        private string _ww;
        private string _us;
        private string _krz;
        private string _odp;
        private string _Zr;
        private string _Int;
        private string _Sw;
        private string _ogd;
        private string _Atk;
        private string _Zyw;
        private string _Sil;
        private string _Wt;
        private string _Sz;
        private string _Mag;
        private string _PO;
        private string _PP;

        [DataMember]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public string WW
        {
            get { return _ww; }
            set { _ww = value; }
        }

        [DataMember]
        public string US
        {
            get { return _us; }
            set { _us = value; }
        }

        [DataMember]
        public string Krz
        {
            get { return _krz; }
            set { _krz = value; }
        }

        [DataMember]
        public string Odp
        {
            get { return _odp; }
            set { _odp = value; }
        }

        [DataMember]
        public string Zr
        {
            get { return _Zr; }
            set { _Zr = value; }
        }

        [DataMember]
        public string Int
        {
            get { return _Int; }
            set { _Int = value; }
        }

        [DataMember]
        public string Sw
        {
            get { return _Sw; }
            set { _Sw = value; }
        }

        [DataMember]
        public string Ogd
        {
            get { return _ogd; }
            set { _ogd = value; }
        }

        [DataMember]
        public string Atk
        {
            get { return _Atk; }
            set { _Atk = value; }
        }

        [DataMember]
        public string Zyw
        {
            get { return _Zyw; }
            set { _Zyw = value; }
        }

        [DataMember]
        public string Sil
        {
            get { return _Sil; }
            set { _Sil = value; }
        }

        [DataMember]
        public string Wt
        {
            get { return _Wt; }
            set { _Wt = value; }
        }

        [DataMember]
        public string Sz
        {
            get { return _Sz; }
            set { _Sz = value; }
        }

        [DataMember]
        public string Mag
        {
            get { return _Mag; }
            set { _Mag = value; }
        }

        [DataMember]
        public string PO
        {
            get { return _PO; }
            set { _PO = value; }
        }

        [DataMember]
        public string PP
        {
            get { return _PP; }
            set { _PP = value; }
        }

    }

    [DataContract]
    public class HeroFullChart
    {
        private string _ww;
        private string _us;
        private string _krz;
        private string _odp;
        private string _Zr;
        private string _Int;
        private string _Sw;
        private string _ogd;
        private string _Atk;
        private string _Zyw;
        private string _Sil;
        private string _Wt;
        private string _Sz;
        private string _Mag;
        private string _PO;
        private string _PP;
        private List<string> _SkillNames;
        private List<string> _AbNames;
        private string _occupation;
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

        [DataMember]
        public string Occupation
        {
            get { return _occupation; }
            set { _occupation = value; }
        }

        [DataMember]
        public List<string> AbNames
        {
            get { return _AbNames; }
            set { _AbNames = value; }
        }

        [DataMember]
        public List<string> SkillNames
        {
            get { return _SkillNames; }
            set { _SkillNames = value; }
        }

        [DataMember]
        public string WW
        {
            get { return _ww; }
            set { _ww = value; }
        }

        [DataMember]
        public string US
        {
            get { return _us; }
            set { _us = value; }
        }

        [DataMember]
        public string Krz
        {
            get { return _krz; }
            set { _krz = value; }
        }

        [DataMember]
        public string Odp
        {
            get { return _odp; }
            set { _odp = value; }
        }

        [DataMember]
        public string Zr
        {
            get { return _Zr; }
            set { _Zr = value; }
        }

        [DataMember]
        public string Int
        {
            get { return _Int; }
            set { _Int = value; }
        }

        [DataMember]
        public string Sw
        {
            get { return _Sw; }
            set { _Sw = value; }
        }

        [DataMember]
        public string Ogd
        {
            get { return _ogd; }
            set { _ogd = value; }
        }

        [DataMember]
        public string Atk
        {
            get { return _Atk; }
            set { _Atk = value; }
        }

        [DataMember]
        public string Zyw
        {
            get { return _Zyw; }
            set { _Zyw = value; }
        }

        [DataMember]
        public string Sil
        {
            get { return _Sil; }
            set { _Sil = value; }
        }

        [DataMember]
        public string Wt
        {
            get { return _Wt; }
            set { _Wt = value; }
        }

        [DataMember]
        public string Sz
        {
            get { return _Sz; }
            set { _Sz = value; }
        }

        [DataMember]
        public string Mag
        {
            get { return _Mag; }
            set { _Mag = value; }
        }

        [DataMember]
        public string PO
        {
            get { return _PO; }
            set { _PO = value; }
        }

        [DataMember]
        public string PP
        {
            get { return _PP; }
            set { _PP = value; }
        }
    }
}
