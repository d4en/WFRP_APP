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
            OptionsModelDisconnectButtonIsEnabled = false,
            OptionsModelMsg = "",
            OptionsModelID = "",
            OptionsModelStatus = "Uknown",
            OptionsModelClientListBox = new List<string>{ "a", "b", "c" }
        };
        #endregion

        #region service data
        ServiceCommunication servCom = null;
        #endregion

        #region windows
        SessionWindow sessionWindow = null;
        #endregion

        public OptionsWindow(ServiceCommunication servCom)
        {
            InitializeComponent();
            this.DataContext = _optionsModel;

            this.servCom = servCom;
            servCom.SetOptionsModel(_optionsModel);
        }

        #region UI events

        private void disconnectButton_Click(object sender,
                          RoutedEventArgs e)
        {
            _optionsModel.OptionsModelStatus = "Disconnecting...";
            if (sessionWindow != null)
                sessionWindow.Close();
            sessionWindow = null;
            //servCom.EndSession();
            servCom.Disconnect();
        }

        private void sessionButton_Click(object sender, RoutedEventArgs e)
        {
            sessionWindow = new SessionWindow(servCom);
            sessionWindow.Show();
        }

        private void OptionWindow_Closed(object sender, EventArgs e)
        {
            if(sessionWindow != null)
                sessionWindow.Close();
            sessionWindow = null;
            //servCom.EndSession();
            servCom.Disconnect();
        }

        #endregion

        
    }
}
