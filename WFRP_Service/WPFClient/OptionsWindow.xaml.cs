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
            OptionsModelStatus = "Uknown"
        };
        #endregion

        #region service data
        ServiceCommunication servCom = null;
        #endregion

        public OptionsWindow()
        {
            InitializeComponent();
            this.DataContext = _optionsModel;
        }

        public void SetServiceCommunication(ServiceCommunication servCom)
        {
            this.servCom = servCom;
        }

        public Model.OptionsModel GetModel()
        {
            return this._optionsModel;
        }


        #region UI events

        private void disconnectButton_Click(object sender,
                          RoutedEventArgs e)
        {
            _optionsModel.OptionsModelStatus = "Disconnecting...";
            servCom.Disconnect();
        }

        #endregion
    }
}
