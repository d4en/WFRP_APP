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
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        #region models
        Model.OptionsModel _optionsModel = new Model.OptionsModel
        {
            OptionsModelOptionsWindowIsVisible = Visibility.Hidden,
            OptionsModelDisconnectButtonIsEnabled = true,
            OptionsModelMsg = "",
            OptionsModelID = "",
            OptionsModelStatus = "Uknown",
            OptionsModelStartButtonIsEnabled = true
        };
        #endregion

        #region service data
        ServiceCommunication servCom = null;
        #endregion

        #region windows
        SessionWindow sessionWindow = null;
        HeroWindow heroWindow = null;
        #endregion

        public OptionsWindow(ServiceCommunication servCom)
        {
            InitializeComponent();
            this.DataContext = _optionsModel;

            this.servCom = servCom;
            servCom.SetOptionsModel(_optionsModel);
            heroWindow = new HeroWindow(servCom);
            sessionWindow = new SessionWindow(servCom, heroWindow);
        }

        #region UI events

        private void disconnectButton_Click(object sender,
                          RoutedEventArgs e)
        {
            _optionsModel.OptionsModelStatus = "Disconnecting...";
            if (sessionWindow != null)
                sessionWindow.Close();
            sessionWindow = null;
            servCom.EndSession();
            servCom.Disconnect();
        }

        private void sessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sessionWindow == null)
                sessionWindow = new SessionWindow(servCom, heroWindow);                              
            servCom.StartSession();
            sessionWindow.Show();
        }

        private void OptionWindow_Closed(object sender, EventArgs e)
        {
            if(sessionWindow != null)
                sessionWindow.Close();
            sessionWindow = null;

            if (heroWindow != null)
                heroWindow.Close();
            heroWindow = null;

            servCom.EndSession();
            servCom.Disconnect();
        }

        private void clientListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (string item in e.RemovedItems)
            {
                _optionsModel.OptionsModelClientListBoxSelectedItems.Remove(item);
            }

            foreach (string item in e.AddedItems)
            {
                _optionsModel.OptionsModelClientListBoxSelectedItems.Add(item);
            }
        }

        private void addMemberToSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_optionsModel.OptionsModelClientListBoxSelectedItems.Count != 0)
                servCom.AddMemberToSession();
        }

        private void viewHeroButton_Click(object sender, RoutedEventArgs e)
        {
            servCom.ViewHero();
            heroWindow.Show();
        }
        
        #endregion

        #region MetroStyle window bar

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
    }
}
