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
        string _heroName = null;
        public string CreateHeroModelHeroName
        {
            get
            {
                return _heroName;
            }
            set
            {
                _heroName = value;

                OnPropertyChanged("CreateHeroModelHeroName");
            }
        }
        
        List<string> _sex = new List<string> { "male", "female"};
        public List<string> CreateHeroModelSex
        {
            get
            {
                return _sex;
            }
            set
            {
                _sex = value;

                OnPropertyChanged("CreateHeroModelSex");
            }
        }
        string _sexItem = null;
        public string CreateHeroModelSexItem
        {
            get
            {
                return _sexItem;
            }
            set
            {
                _sexItem = value;

                OnPropertyChanged("CreateHeroModelSexItem");
            }
        }
        List<string> _source = new List<string> { "człowiek", "elf", "krasnolud", "niziołek" };
        string _selectedItem = null;
        public List<string> Source { get { return _source; } }
        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem == "człowiek")
                {
                    CreateHeroModelSz = "4";
                }
                else if (SelectedItem == "elf")
                {
                    CreateHeroModelSz = "5";
                }
                else if (SelectedItem == "krasnolud")
                {
                    CreateHeroModelSz = "3";
                }
                else if (SelectedItem == "niziołek")
                {
                    CreateHeroModelSz = "4";
                }
            }
        }
        List<string> _eyeColor = new List<String>();
        public List<string> HeroModelEyeColor
        {
            get
            {
                return _eyeColor;
            }
            set
            {
                _eyeColor = value;
                OnPropertyChanged("HeroModelEyeColor");
            }
        }
        string _eyeColorItem = null;
        public string CreateHeroModelEyeColorItem
        {
            get
            {
                return _eyeColorItem;
            }
            set
            {
                _eyeColorItem = value;

                OnPropertyChanged("CreateHeroModelEyeColorItem");
            }
        }
       
        public List<string> _occupation = new List<string>();
        public List<string> CreateHeroModelOccupation
        {
            get
            {
                return _occupation;
            }
            set
            {
                _occupation = value;

                OnPropertyChanged("CreateHeroModelOccupation");
            }
        }
        public string _occupationItem = null;
        public string CreateHeroModelOccupationItem
        {
            get
            {
                return _occupationItem;
            }
            set
            {
                _occupationItem = value;

                OnPropertyChanged("CreateHeroModelOccupationItem");
            }
        }
        public string _occupationText = null;
        public string CreateHeroModelOccupationText
        {
            get
            {
                return _occupationText;
            }
            set
            {
                _occupationText = value;

                OnPropertyChanged("CreateHeroModelOccupationText");
            }
        }
        string _age = null;
        public string CreateHeroModelAge
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;

                OnPropertyChanged("CreateHeroModelAge");
            }
        }
        string _height = null;
        public string CreateHeroModelHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;

                OnPropertyChanged("CreateHeroModelHeight");
            }
        }
        string _weight = null;
        public string CreateHeroModelWeight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;

                OnPropertyChanged("CreateHeroModelWeight");
            }
        }
        string _origin = null;
        public string CreateHeroModelOrigin
        {
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;

                OnPropertyChanged("CreateHeroModelOrigin");
            }
        }
        string _family = null;
        public string CreateHeroModelFamily
        {
            get
            {
                return _family;
            }
            set
            {
                _family = value;

                OnPropertyChanged("CreateHeroModelFamily");
            }
        }
        string _socialPosition = null;
        public string CreateHeroModelSocialPosition
        {
            get
            {
                return _socialPosition;
            }
            set
            {
                _socialPosition = value;

                OnPropertyChanged("CreateHeroModelSocialPosition");
            }
        }
        string _whoHeWas = null;
        public string CreateHeroModelwhoHeWas
        {
            get
            {
                return _whoHeWas;
            }
            set
            {
                _whoHeWas = value;

                OnPropertyChanged("CreateHeroModelwhoHeWas");
            }
        }
        string _traveling = null;
        public string CreateHeroModelTraveling
        {
            get
            {
                return _traveling;
            }
            set
            {
                _traveling = value;

                OnPropertyChanged("CreateHeroModelTraveling");
            }
        }
        string _WHS = null;
        public string CreateHeroModelWHS
        {
            get
            {
                return _WHS;
            }
            set
            {
                _WHS = value;

                OnPropertyChanged("CreateHeroModelWHS");
            }
        }
        string _friends = null;
        public string CreateHeroModelFriends
        {
            get
            {
                return _friends;
            }
            set
            {
                _friends = value;

                OnPropertyChanged("CreateHeroModelFriends");
            }
        }
        string _enemys = null;
        public string CreateHeroModelEnemys
        {
            get
            {
                return _enemys;
            }
            set
            {
                _enemys = value;
                OnPropertyChanged("CreateHeroModelEnemys");
            }
        }
        string _WDHL = null;
        public string CreateHeroModelWDHL
        {
            get
            {
                return _WDHL;
            }
            set
            {
                _WDHL = value;

                OnPropertyChanged("CreateHeroModelWDHL");
            }
        }
        string _motivations = null;
        public string CreateHeroModelMotivations
        {
            get
            {
                return _motivations;
            }
            set
            {
                _motivations = value;

                OnPropertyChanged("CreateHeroModelMotivations");
            }
        }
        string _personality = null;
        public string CreateHeroModelPersonality
        {
            get
            {
                return _personality;
            }
            set
            {
                _personality = value;

                OnPropertyChanged("CreateHeroModelPersonality");
            }
        }
        string _WDHNL = null;
        public string CreateHeroModelWDNHL
        {
            get
            {
                return _WDHNL;
            }
            set
            {
                _WDHNL = value;

                OnPropertyChanged("CreateHeroModelWDNHL");
            }
        }
        bool _submitButtonIsEnabled = true;
        public bool CreateHeroModelSubmitIsEnabled
        {
            get
            {
                return _submitButtonIsEnabled;
            }
            set
            {
                _submitButtonIsEnabled = value;

                OnPropertyChanged("CreateHeroModelSubmitIsEnabled");
            }
        }

        string _WW = null;
        public string CreateHeroModelWW
        {
            get
            {
                return _WW;
            }
            set
            {
                _WW = value;
                
                OnPropertyChanged("CreateHeroModelWW");
            }
        }
        string _US = null;
        public string CreateHeroModelUS
        {
            get
            {
                return _US;
            }
            set
            {

                _US = value;
                OnPropertyChanged("CreateHeroModelUS");
            }
        }
        string _K = null;
        public string CreateHeroModelK
        {
            get
            {
                return _K;
            }
            set
            {
                _K = value;

                OnPropertyChanged("CreateHeroModelK");
            }
        }
        string _Odp = null;
        public string CreateHeroModelOdp
        {
            get
            {
                return _Odp; 
            }
            set
            {
                _Odp = value;

                OnPropertyChanged("CreateHeroModelOdp");
            }
        }
        string _Zr = null;
        public string CreateHeroModelZr
        {
            get
            {
                return _Zr;
            }
            set
            {
                _Zr = value;

                OnPropertyChanged("CreateHeroModelZr");
            }
        }
        string _Int = null;
        public string CreateHeroModelInt
        {
            get
            {
                return _Int;
            }
            set
            {
                _Int = value;

                OnPropertyChanged("CreateHeroModelInt");
            }
        }
        string _SW = null;
        public string CreateHeroModelSW
        {
            get
            {
                return _SW;
            }
            set
            {
                _SW = value;

                OnPropertyChanged("CreateHeroModelSW");
            }
        }
        string _Ogd = null;
        public string CreateHeroModelOgd
        {
            get
            {
                return _Ogd;
            }
            set
            {
                _Ogd = value;

                OnPropertyChanged("CreateHeroModelOgd");
            }
        }
        string _Zyw = null;
        public string CreateHeroModelZyw
        {
            get
            {
                return _Zyw;
            }
            set
            {
                _Zyw = value;

                OnPropertyChanged("CreateHeroModelZyw");
            }
        }
        string _PP = null;
        public string CreateHeroModelPP
        {
            get
            {
                return _PP;
            }
            set
            {
                _PP = value;

                OnPropertyChanged("CreateHeroModelPP");
            }
        }
        string _Sz = null;
        public string CreateHeroModelSz
        {
            get
            {

                return _Sz;
            }
            set
            {

                _Sz = value;

            }
        }
        string _skAbInfo = null;
        public string CreateHeroModelSkAbInfo
        {
            get
            {
                return _skAbInfo;
            }
            set
            {
                _skAbInfo = value;
                OnPropertyChanged("CreateHeroModelSkAbInfo");
            }
        }
        #region Occupation Skills
        List<string> _occupationSkills = new List<string>();
        public List<string> CreateHeroModelOccupationSkillsListBox
        {
            get
            {
                return _occupationSkills;
            }
            set
            {
                _occupationSkills = value;

                OnPropertyChanged("CreateHeroModelOccupationSkillsListBox");
            }
        }
        List<string> _occupationSkillsNames = new List<string>();
        public List<string> CreateHeroModelOccupationSkillsNames
        {
            get
            {
                return _occupationSkillsNames;
            }
            set
            {
                _occupationSkillsNames = value;

            }
        }
        List<string> _occupationSkillsID = new List<string>();
        public List<string> CreateHeroModelOccupationSkillsID
        {
            get
            {
                return _occupationSkillsID;
            }
            set
            {
                _occupationSkillsID = value;

            }
        }
        List<List<String>> _occupationSkillChoose = new List<List<string>>();
        public List<List<String>> CreateHeroModelOccupationSkillsChoose
        {
            get
            {
                return _occupationSkillChoose;
            }
            set
            {
                _occupationSkillChoose = value;
            }
        }
        List<List<String>> _occupationSkillNameChoose = new List<List<string>>();
        public List<List<String>> CreateHeroModelOccupationSkillsNameChoose
        {
            get
            {
                return _occupationSkillNameChoose;
            }
            set
            {
                _occupationSkillNameChoose = value;
            }
        }
        int _occupationSkillState = 1;
        public int OccupationSkillState
        {
            get
            {
                return _occupationSkillState;
            }
            set
            {
                _occupationSkillState = value;
            }
        }
        bool _occupationSkillsIsEnabled = true;
        public bool CreateHeroModelOccupationSkillsIsEnabled
        {
            get
            {
                return _occupationSkillsIsEnabled;
            }
            set
            {
                _occupationSkillsIsEnabled = value;
                OnPropertyChanged("CreateHeroModelOccupationSkillsIsEnabled");
            }
        }
#endregion
        #region Occupation Abilities
        List<string> _occupationAbilities = new List<string>();
        public List<string> CreateHeroModelOccupationAbilitiesListBox
        {
            get
            {
                return _occupationAbilities;
            }
            set
            {
                _occupationAbilities = value;

                OnPropertyChanged("CreateHeroModelOccupationAbilitiesListBox");
            }
        }
        List<string> _occupationAbilitiesNames = new List<string>();
        public List<string> CreateHeroModelOccupationAbilitiesNames
        {
            get
            {
                return _occupationAbilitiesNames;
            }
            set
            {
                _occupationAbilitiesNames = value;

            }
        }
        List<string> _occupationAbilitiesID = new List<string>();
        public List<string> CreateHeroModelOccupationAbilitiesID
        {
            get
            {
                return _occupationAbilitiesID;
            }
            set
            {
                _occupationAbilitiesID = value;

            }
        }
        List<List<String>> _occupationAbilitiesChoose = new List<List<string>>();
        public List<List<String>> CreateHeroModelOccupationAbilitiesChoose
        {
            get
            {
                return _occupationAbilitiesChoose;
            }
            set
            {
                _occupationAbilitiesChoose = value;
            }
        }
        List<List<String>> _occupationAbilitiesNameChoose = new List<List<string>>();
        public List<List<String>> CreateHeroModelOccupationAbilitiesNameChoose
        {
            get
            {
                return _occupationAbilitiesNameChoose;
            }
            set
            {
                _occupationAbilitiesNameChoose = value;
            }
        }
        int _occupationAbilitiesState = 1;
        public int OccupationAbilitiesState
        {
            get
            {
                return _occupationAbilitiesState;
            }
            set
            {
                _occupationAbilitiesState = value;
            }
        }
        bool _occupationAbilitiesIsEnabled = true;
        public bool CreateHeroModelOccupationAbilitiesIsEnabled
        {
            get
            {
                return _occupationAbilitiesIsEnabled;
            }
            set
            {
                _occupationAbilitiesIsEnabled = value;
                OnPropertyChanged("CreateHeroModelOccupationAbilitiesIsEnabled");
            }
        }
#endregion
        #region Race Skills
        List<string> _raceSkills = new List<string>();
        public List<string> CreateHeroModelRaceSkillsListBox
        {
            get
            {
                return _raceSkills;
            }
            set
            {
                _raceSkills = value;

                OnPropertyChanged("CreateHeroModelRaceSkillsListBox");
            }
        }
        List<string> _raceSkillsNames = new List<string>();
        public List<string> CreateHeroModelRaceSkillsNames
        {
            get
            {
                return _raceSkillsNames;
            }
            set
            {
                _raceSkillsNames = value;

            }
        }
        List<string> _raceSkillsID = new List<string>();
        public List<string> CreateHeroModelRaceSkillsID
        {
            get
            {
                return _raceSkillsID;
            }
            set
            {
                _raceSkillsID = value;

            }
        }
        List<List<String>> _raceSkillsChoose = new List<List<string>>();
        public List<List<String>> CreateHeroModelRaceSkillsChoose
        {
            get
            {
                return _raceSkillsChoose;
            }
            set
            {
                _raceSkillsChoose = value;
            }
        }
        List<List<String>> _raceSkillsNameChoose = new List<List<string>>();
        public List<List<String>> CreateHeroModelRaceSkillsNameChoose
        {
            get
            {
                return _raceSkillsNameChoose;
            }
            set
            {
                _raceSkillsNameChoose = value;
            }
        }
        int _raceSkillsState = 1;
        public int RaceSkillsState
        {
            get
            {
                return _raceSkillsState;
            }
            set
            {
                _raceSkillsState = value;
            }
        }
        bool _raceSkillsIsEnabled = true;
        public bool CreateHeroModelRaceSkillsIsEnabled
        {
            get
            {
                return _raceSkillsIsEnabled;
            }
            set
            {
                _raceSkillsIsEnabled = value;
                OnPropertyChanged("CreateHeroModelRaceSkillsIsEnabled");
            }
        }
#endregion
        #region Race Abilities
        List<string> _raceAbilities = new List<string>();
        public List<string> CreateHeroModelRaceAbilitiesListBox
        {
            get
            {
                return _raceAbilities;
            }
            set
            {
                _raceAbilities = value;

                OnPropertyChanged("CreateHeroModelRaceAbilitiesListBox");
            }
        }
        List<string> _raceAbilitiesNames = new List<string>();
        public List<string> CreateHeroModelRaceAbilitiesNames
        {
            get
            {
                return _raceAbilitiesNames;
            }
            set
            {
                _raceAbilitiesNames = value;

            }
        }
        List<string> _raceAbilitiesID = new List<string>();
        public List<string> CreateHeroModelRaceAbilitiesID
        {
            get
            {
                return _raceAbilitiesID;
            }
            set
            {
                _raceAbilitiesID = value;

            }
        }
        List<List<String>> _raceAbilitiesChoose = new List<List<string>>();
        public List<List<String>> CreateHeroModelRaceAbilitiesChoose
        {
            get
            {
                return _raceAbilitiesChoose;
            }
            set
            {
                _raceAbilitiesChoose = value;
            }
        }
        List<List<String>> _raceAbilitiesNameChoose = new List<List<string>>();
        public List<List<String>> CreateHeroModelRaceAbilitiesNameChoose
        {
            get
            {
                return _raceAbilitiesNameChoose;
            }
            set
            {
                _raceAbilitiesNameChoose = value;
            }
        }
        int _raceAbilitiesState = 1;
        public int RaceAbilitiesState
        {
            get
            {
                return _raceAbilitiesState;
            }
            set
            {
                _raceAbilitiesState = value;
            }
        }
        bool _raceAbilitiesIsEnabled = true;
        public bool CreateHeroModelRaceAbilitiesIsEnabled
        {
            get
            {
                return _raceAbilitiesIsEnabled;
            }
            set
            {
                _raceAbilitiesIsEnabled = value;
                OnPropertyChanged("CreateHeroModelRaceAbilitiesIsEnabled");
            }
        }
        #endregion
        bool _basicInfoIsEnabled = true;
        public bool CreateHeroModelBasicInfoIsEnabled
        {
            get
            {
                return _basicInfoIsEnabled;
            }
            set
            {
                _basicInfoIsEnabled = value;
                OnPropertyChanged("CreateHeroModelBasicInfoIsEnabled");
            }
        }
        bool _skAbIsSelected = false;
        public bool CreateHeroModelSkAbIsSelected
        {
            get
            {
                return _skAbIsSelected;
            }
            set
            {
                _skAbIsSelected = value;
                OnPropertyChanged("CreateHeroModelSkAbIsSelected");
            }
        }
        bool _skAbIsEnabled = true;
        public bool CreateHeroModelSkAbIsEnabled
        {
            get
            {
                return _skAbIsEnabled;
            }
            set
            {
                _skAbIsEnabled = value;
                OnPropertyChanged("CreateHeroModelSkAbIsEnabled");
            }
        }
        List<String> _allAbilities = new List<string>();
        public List<String> AllAbilities
        {
            get
            {
                return _allAbilities;
            }
            set
            {
                _allAbilities = value;
            }
        }
        List<String> _allSkills = new List<string>();
        public List<String> AllSkills
        {
            get
            {
                return _allSkills;
            }
            set
            {
                _allSkills = value;
            }
        }
        List<String> _randomStats = new List<string>();
        public List<String> RandomStats
        {
            get
            {
                return _randomStats;
            }
            set
            {
                _randomStats = value;
            }
        }
        public void Roll(int race)
        {
            Random random = new Random();
            if (race == 0)
            {
                 CreateHeroModelWW = (20 + random.Next(1, 21)).ToString();
                 CreateHeroModelUS = (20 + random.Next(1, 21)).ToString();
                 CreateHeroModelK = (20 + random.Next(1, 21)).ToString();
                 CreateHeroModelOdp = (20 + random.Next(1, 21)).ToString();
                 CreateHeroModelZr = (20 + random.Next(1, 21)).ToString();
                 CreateHeroModelInt = (20 + random.Next(1, 21)).ToString();
                 CreateHeroModelSW = (20 + random.Next(1, 21)).ToString();
                 CreateHeroModelOgd = (20 + random.Next(1, 21)).ToString();
                 int temp = random.Next(1, 11);
                 if (temp > 0 && temp < 4)
                 {
                     CreateHeroModelZyw = "10";
                 }
                 else if (temp >= 4 && temp < 7)
                 {
                     CreateHeroModelZyw = "11";
                 }
                 else if (temp >= 7 && temp < 10)
                 {
                     CreateHeroModelZyw = "12";
                 }
                 else if (temp == 10)
                 {
                     CreateHeroModelZyw = "13";
                 }
                 if (temp > 0 && temp < 5)
                 {
                     CreateHeroModelPP = "2";
                 }
                 else if (temp >= 5)
                 {
                     CreateHeroModelPP = "3";
                 }
            }
            else if (race == 1)
            {
                CreateHeroModelWW = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelUS = (30 + random.Next(1, 21)).ToString();
                CreateHeroModelK = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelOdp = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelZr = (30 + random.Next(1, 21)).ToString();
                CreateHeroModelInt = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelSW = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelOgd = (20 + random.Next(1, 21)).ToString();
                int temp = random.Next(1, 11);
                if (temp > 0 && temp < 4)
                {
                    CreateHeroModelZyw = "9";
                }
                else if (temp >= 4 && temp < 7)
                {
                    CreateHeroModelZyw = "10";
                }
                else if (temp >= 7 && temp < 10)
                {
                    CreateHeroModelZyw = "11";
                }
                else if (temp == 10)
                {
                    CreateHeroModelZyw = "12";
                }
                if (temp > 0 && temp < 5)
                {
                    CreateHeroModelPP = "1";
                }
                else if (temp >= 5)
                {
                    CreateHeroModelPP = "2";
                }
            }
            else if (race == 2)
            {
                CreateHeroModelWW = (30 + random.Next(1, 21)).ToString();
                CreateHeroModelUS = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelK = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelOdp = (30 + random.Next(1, 21)).ToString();
                CreateHeroModelZr = (10 + random.Next(1, 21)).ToString();
                CreateHeroModelInt = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelSW = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelOgd = (10 + random.Next(1, 21)).ToString();
                int temp = random.Next(1, 11);
                if (temp > 0 && temp < 4)
                {
                    CreateHeroModelZyw = "11";
                }
                else if (temp >= 4 && temp < 7)
                {
                    CreateHeroModelZyw = "12";
                }
                else if (temp >= 7 && temp < 10)
                {
                    CreateHeroModelZyw = "13";
                }
                else if (temp == 10)
                {
                    CreateHeroModelZyw = "14";
                }
                if (temp > 0 && temp < 5)
                {
                    CreateHeroModelPP = "1";
                }
                else if (temp >= 5 && temp < 8)
                {
                    CreateHeroModelPP = "2";
                }
                else if( temp >=8 && temp <= 10)
                {
                    CreateHeroModelPP = "3";
                }
            }
            else if (race == 3)
            {
                CreateHeroModelWW = (10 + random.Next(1, 21)).ToString();
                CreateHeroModelUS = (30 + random.Next(1, 21)).ToString();
                CreateHeroModelK = (10 + random.Next(1, 21)).ToString();
                CreateHeroModelOdp = (10 + random.Next(1, 21)).ToString();
                CreateHeroModelZr = (30 + random.Next(1, 21)).ToString();
                CreateHeroModelInt = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelSW = (20 + random.Next(1, 21)).ToString();
                CreateHeroModelOgd = (30 + random.Next(1, 21)).ToString();
                int temp = random.Next(1, 11);
                if (temp > 0 && temp < 4)
                {
                    CreateHeroModelZyw = "8";
                }
                else if (temp >= 4 && temp < 7)
                {
                    CreateHeroModelZyw = "9";
                }
                else if (temp >= 7 && temp < 10)
                {
                    CreateHeroModelZyw = "10";
                }
                else if (temp == 10)
                {
                    CreateHeroModelZyw = "11";
                }
                if (temp > 0 && temp < 8)
                {
                    CreateHeroModelPP = "2";
                }
                else if (temp >= 8)
                {
                    CreateHeroModelPP = "3";
                }
            }
        }

        bool _randomIsEnabled = true;
        public bool CreateHeroModePrimaryRandomStatslIsEnabled
        {
            get
            {
                return _randomIsEnabled;
            }
            set
            {
                _randomIsEnabled = value;
                OnPropertyChanged("CreateHeroModePrimaryRandomStatslIsEnabled");
            }
        }
        bool _stSkAbIsEnabled = true;
        public bool CreateHeroModelHeroStSkAbSubmitIsEnabled
        {
            get
            {
                return _stSkAbIsEnabled;
            }
            set
            {
                _stSkAbIsEnabled = value;
                OnPropertyChanged("CreateHeroModelHeroStSkAbSubmitIsEnabled");
            }
        }
        bool _primaryStatsSubmit = false;
        public bool CreateHeroModelPrimaryStatsSubmitIsEnabled
        {
            get
            {
                return _primaryStatsSubmit;
            }
            set
            {
                _primaryStatsSubmit = value;
                OnPropertyChanged("CreateHeroModelPrimaryStatsSubmitIsEnabled");
            }
        }
        
    }
}
