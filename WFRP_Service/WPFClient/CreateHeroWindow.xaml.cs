using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for CreateHeroWindow.xaml
    /// </summary>
    public partial class CreateHeroWindow : Window
    {
        #region models
        Model.CreateHeroModel _CreateHeroModel = new Model.CreateHeroModel
        {
            CreateHeroModelCreateHeroWindowIsVisible = Visibility.Hidden,

        };
        #endregion

        #region service data
        ServiceCommunication servCom = null;
        #endregion

        public CreateHeroWindow(ServiceCommunication servCom)
        {
            InitializeComponent();
            this.DataContext = _CreateHeroModel;

            this.servCom = servCom;
            servCom.SetCreateHeroModel(_CreateHeroModel);

        }

        private void CreateHeroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Collapsed;
        }

        #region MetroStyle window bar

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void PART_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        #endregion

        private void CMB_Race_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.servCom.AskEyes(_CreateHeroModel.SelectedItem);
                this.servCom.GetOccupations(_CreateHeroModel.SelectedItem);
            }
            catch (Exception) { }
        }

        private void BTN_Submit_Click(object sender, RoutedEventArgs e)
        {
            this.servCom.HeroBasicInfoSubmit();
            this.servCom.SendOccupationAndRace();
            _CreateHeroModel.CreateHeroModelBasicInfoIsEnabled = false;
            _CreateHeroModel.CreateHeroModelSubmitIsEnabled = false;
            _CreateHeroModel.CreateHeroModelSkAbIsEnabled = true;
        }

        private void CMB_Occupation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.servCom.AskOccupationDetails(_CreateHeroModel.CreateHeroModelOccupationItem);
            }
            catch (Exception) { }
        }

        private void CMB_eyeColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this._CreateHeroModel.CreateHeroModelEyeColorItem = CMB_eyeColor.SelectedValue.ToString();
            }
            catch(Exception) { }
        }

        private void CMB_Sex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this._CreateHeroModel.CreateHeroModelSexItem = CMB_Sex.SelectedItem.ToString();
            }
            catch (Exception) { }
        }

        private void LTB_occupationSkills_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                servCom.GetSkillInfo(LTB_occupationSkills.SelectedItem.ToString());
                Console.WriteLine(_CreateHeroModel.CreateHeroModelOccupationSkillsChoose[_CreateHeroModel.OccupationSkillState][LTB_occupationSkills.SelectedIndex]);
            }
            catch (Exception) { }
        }

        private void LTB_occupationAbilities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                servCom.GetAbilityInfo(LTB_occupationAbilities.SelectedItem.ToString());
            }
            catch (Exception) { }
        }

        private void LTB_raceSkills_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                servCom.GetSkillInfo(LTB_raceSkills.SelectedItem.ToString());
            }
            catch (Exception) { }
        }

        private void LTB_raceAbilities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                servCom.GetAbilityInfo(LTB_raceAbilities.SelectedItem.ToString());
            }
            catch (Exception) { }
        }

        private void BTN_occupationSkills_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = null;
                if (_CreateHeroModel.OccupationSkillState < _CreateHeroModel.CreateHeroModelOccupationSkillsChoose.Count)
                {
                    _CreateHeroModel.CreateHeroModelOccupationSkillsListBox = _CreateHeroModel.CreateHeroModelOccupationSkillsNameChoose[_CreateHeroModel.OccupationSkillState];
                 //   Console.WriteLine("##  "+_CreateHeroModel.CreateHeroModelOccupationSkillsChoose[_CreateHeroModel.OccupationSkillState][LTB_occupationSkills.SelectedIndex]);
                 //   temp=_CreateHeroModel.CreateHeroModelOccupationSkillsChoose[_CreateHeroModel.OccupationSkillState][LTB_occupationSkills.SelectedIndex];
                   // Console.WriteLine(temp);
                }
                _CreateHeroModel.OccupationSkillState++;
                if (_CreateHeroModel.OccupationSkillState >= _CreateHeroModel.CreateHeroModelOccupationSkillsChoose.Count + 1)
                {
                    _CreateHeroModel.CreateHeroModelOccupationSkillsListBox = null;
                    _CreateHeroModel.CreateHeroModelOccupationSkillsIsEnabled = false;
                }
            }
            catch (Exception) { }
        }

        private void BTN_occupationAbilities_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_CreateHeroModel.OccupationAbilitiesState < _CreateHeroModel.CreateHeroModelOccupationAbilitiesChoose.Count)
                {
                    _CreateHeroModel.CreateHeroModelOccupationAbilitiesListBox = _CreateHeroModel.CreateHeroModelOccupationAbilitiesNameChoose[_CreateHeroModel.OccupationAbilitiesState];
                 //   _CreateHeroModel.AllAbilities.Add(_CreateHeroModel.CreateHeroModelOccupationAbilitiesChoose[_CreateHeroModel.OccupationAbilitiesState - 1][LTB_occupationAbilities.SelectedIndex]);
                }
                _CreateHeroModel.OccupationAbilitiesState++;
                if (_CreateHeroModel.OccupationAbilitiesState >= _CreateHeroModel.CreateHeroModelOccupationAbilitiesChoose.Count+1)
                {
                    _CreateHeroModel.CreateHeroModelOccupationAbilitiesListBox = null;
                    _CreateHeroModel.CreateHeroModelOccupationAbilitiesIsEnabled = false;
                }
            }
            catch (Exception) { }
        }

        private void BTN_raceSkills_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_CreateHeroModel.RaceSkillsState < _CreateHeroModel.CreateHeroModelRaceSkillsChoose.Count)
                {
                    _CreateHeroModel.CreateHeroModelRaceSkillsListBox = _CreateHeroModel.CreateHeroModelRaceSkillsNameChoose[_CreateHeroModel.RaceSkillsState];
                //    _CreateHeroModel.AllSkills.Add(_CreateHeroModel.CreateHeroModelRaceSkillsChoose[_CreateHeroModel.RaceSkillsState - 1][LTB_raceSkills.SelectedIndex]);
                }
                _CreateHeroModel.RaceSkillsState++;
                if (_CreateHeroModel.RaceSkillsState >= _CreateHeroModel.CreateHeroModelRaceSkillsChoose.Count + 1)
                {
                    _CreateHeroModel.CreateHeroModelRaceSkillsListBox = null;
                    _CreateHeroModel.CreateHeroModelRaceSkillsIsEnabled = false;
                }
            }
            catch (Exception) { }
        }

        private void BTN_raceAbilities_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_CreateHeroModel.RaceAbilitiesState < _CreateHeroModel.CreateHeroModelRaceAbilitiesChoose.Count)
                {
                    _CreateHeroModel.CreateHeroModelRaceAbilitiesListBox = _CreateHeroModel.CreateHeroModelRaceAbilitiesNameChoose[_CreateHeroModel.RaceAbilitiesState];
                //    _CreateHeroModel.AllAbilities.Add(_CreateHeroModel.CreateHeroModelRaceAbilitiesChoose[_CreateHeroModel.RaceAbilitiesState - 1][LTB_raceAbilities.SelectedIndex]);
                }
                _CreateHeroModel.RaceAbilitiesState++;
                if (_CreateHeroModel.RaceAbilitiesState >= _CreateHeroModel.CreateHeroModelRaceAbilitiesChoose.Count + 1)
                {
                    _CreateHeroModel.CreateHeroModelRaceAbilitiesListBox = null;
                    _CreateHeroModel.CreateHeroModelRaceAbilitiesIsEnabled = false;
                }
            }
            catch (Exception) { }
        }
    }
}
