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
        public string CreateHeroModelSW
        {
            get
            {
                return _US;
            }
            set
            {

                _US = value;
                OnPropertyChanged("CreateHeroModelSW");
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
    }
}
