using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPFClient.Model
{
    public class HeroModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        System.Windows.Visibility _heroWindowIsVisible = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility HeroModelHeroWindowIsVisible
        {
            get
            {
                return _heroWindowIsVisible;
            }
            set
            {
                _heroWindowIsVisible = value;

                OnPropertyChanged("HeroModelHeroWindowIsVisible");
            }
        }

        string _clientName = null;
        public string HeroModelClientName
        {
            get
            {
                return _clientName;
            }
            set
            {
                _clientName = value;

                OnPropertyChanged("HeroModelClientName");
            }
        }
        string _displayWW = null;
        public string HeroModelDisplayWW
        {
            get
            {
                return _displayWW;
            }
            set
            {
                _displayWW = value;

                OnPropertyChanged("HeroModelDisplayWW");
            }
        }
        string _displayUS = null;
        public string HeroModelDisplayUS
        {
            get
            {
                return _displayUS;
            }
            set
            {
                _displayUS = value;

                OnPropertyChanged("HeroModelDisplayUS");
            }
        }
        string _displayK = null;
        public string HeroModelDisplayK
        {
            get
            {
                return _displayK;
            }
            set
            {
                _displayK = value;

                OnPropertyChanged("HeroModelDisplayK");
            }
        }
        string _displayOdp = null;
        public string HeroModelDisplayOdp
        {
            get
            {
                return _displayOdp;
            }
            set
            {
                _displayOdp = value;

                OnPropertyChanged("HeroModelDisplayOdp");
            }
        }
        string _displayZr = null;
        public string HeroModelDisplayZr
        {
            get
            {
                return _displayZr;
            }
            set
            {
                _displayZr = value;

                OnPropertyChanged("HeroModelDisplayZr");
            }
        }
        string _displayInt = null;
        public string HeroModelDisplayInt
        {
            get
            {
                return _displayInt;
            }
            set
            {
                _displayInt = value;

                OnPropertyChanged("HeroModelDisplayInt");
            }
        }
        string _displaySW = null;
        public string HeroModelDisplaySW
        {
            get
            {
                return _displaySW;
            }
            set
            {
                _displaySW = value;

                OnPropertyChanged("HeroModelDisplaySW");
            }
        }
        string _displayOgd = null;
        public string HeroModelDisplayOgd
        {
            get
            {
                return _displayOgd;
            }
            set
            {
                _displayOgd = value;

                OnPropertyChanged("HeroModelDisplayOgd");
            }
        }
        string _displayA = null;
        public string HeroModelDisplayA
        {
            get
            {
                return _displayA;
            }
            set
            {
                _displayA = value;

                OnPropertyChanged("HeroModelDisplayA");
            }
        }
        string _displayZyw = null;
        public string HeroModelDisplayZyw
        {
            get
            {
                return _displayZyw;
            }
            set
            {
                _displayZyw = value;

                OnPropertyChanged("HeroModelDisplayZyw");
            }
        }
        string _displayS = null;
        public string HeroModelDisplayS
        {
            get
            {
                return _displayS;
            }
            set
            {
                _displayS = value;

                OnPropertyChanged("HeroModelDisplayS");
            }
        }
        string _displayWt = null;
        public string HeroModelDisplayWt
        {
            get
            {
                return _displayWt;
            }
            set
            {
                _displayWt = value;

                OnPropertyChanged("HeroModelDisplayWt");
            }
        }
        string _displaySz = null;
        public string HeroModelDisplaySz
        {
            get
            {
                return _displaySz;
            }
            set
            {
                _displaySz = value;

                OnPropertyChanged("HeroModelDisplaySz");
            }
        }
        string _displayMag = null;
        public string HeroModelDisplayMag
        {
            get
            {
                return _displayMag;
            }
            set
            {
                _displayMag = value;

                OnPropertyChanged("HeroModelDisplayMag");
            }
        }
        string _displayPO = null;
        public string HeroModelDisplayPO
        {
            get
            {
                return _displayPO;
            }
            set
            {
                _displayPO = value;

                OnPropertyChanged("HeroModelDisplayPO");
            }
        }
        string _displayPP = null;
        public string HeroModelDisplayPP
        {
            get
            {
                return _displayPP;
            }
            set
            {
                _displayPP = value;

                OnPropertyChanged("HeroModelDisplayPP");
            }
        }
        string _displayArmorHead = null;
        public string HeroModelDisplayArmorHead
        {
            get
            {
                return _displayArmorHead;
            }
            set
            {
                _displayArmorHead = value;
                OnPropertyChanged("HeroModelDisplayArmorHead");
            }
        }
        string _displayArmorBody = null;
        public string HeroModelDisplayArmorBody
        {
            get
            {
                return _displayArmorBody;
            }
            set
            {
                _displayArmorBody = value;
                OnPropertyChanged("HeroModelDisplayArmorBody");
            }
        }
        string _displayArmorLeftArm = null;
        public string HeroModelDisplayArmorLeftArm
        {
            get
            {
                return _displayArmorLeftArm;
            }
            set
            {
                _displayArmorLeftArm = value;
                OnPropertyChanged("HeroModelDisplayArmorLeftArm");
            }
        }
        string _displayArmorRightArm = null;
        public string HeroModelDisplayArmorRightArm
        {
            get
            {
                return _displayArmorRightArm;
            }
            set
            {
                _displayArmorRightArm = value;
                OnPropertyChanged("HeroModelDisplayArmorRightArm");
            }
        }
        string _displayArmorLeftLeg = null;
        public string HeroModelDisplayArmorLeftLeg
        {
            get
            {
                return _displayArmorLeftLeg;
            }
            set
            {
                _displayArmorLeftLeg = value;
                OnPropertyChanged("HeroModelDisplayArmorLeftLeg");
            }
        }
        string _displayArmorRightLeg = null;
        public string HeroModelDisplayArmorRightLeg
        {
            get
            {
                return _displayArmorRightLeg;
            }
            set
            {
                _displayArmorRightLeg = value;
                OnPropertyChanged("HeroModelDisplayArmorRightLeg");
            }
        }
        string _displayHeroName = null;
        public string HeroModelDisplayName
        {
            get
            {
                return _displayHeroName;
            }
            set
            {
                _displayHeroName = value;

                OnPropertyChanged("HeroModelDisplayName");
            }
        }
        List<String> _displaySkills = new List<string>();
        public List<String> HeroModelDisplaySkillsListBox
        {
            get
            {
                return _displaySkills;
            }
            set
            {
                _displaySkills = value;
                OnPropertyChanged("HeroModelDisplaySkillsListBox");
            }
        }
        List<String> _displayAbilities = new List<string>();
        public List<String> HeroModelDisplayAbilitiesListBox
        {
            get
            {
                return _displayAbilities;
            }
            set
            {
                _displayAbilities = value;
                OnPropertyChanged("HeroModelDisplayAbilitiesListBox");
            }
        }
        string _displaySkillAndAbilitiesInfo = null;
        public string HeroModelDisplaySkillAndAbilitiesInfo
        {
            get
            {
                return _displaySkillAndAbilitiesInfo;
            }
            set
            {
                _displaySkillAndAbilitiesInfo = value;
                OnPropertyChanged("HeroModelDisplaySkillAndAbilitiesInfo");
            }
        }
        bool _displaySkillsAndAbilitiesFlag = false;
        public bool HeroModelDisplaySkillsAndAbilitiesFlag
        {
            get
            {
                return _displaySkillsAndAbilitiesFlag;
            }
            set
            {
                _displaySkillsAndAbilitiesFlag = value;
            }
        }
    }
}
