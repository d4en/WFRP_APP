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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.Threading;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region windows
        OptionsWindow optWin = null;
        #endregion

        #region models
        Model.LoginModel _loginModel = new Model.LoginModel {
            LoginModelStatus = "Not connected",
            LoginModelMsg = "",
            LoginModelUserName = "",
            LoginModelServerIP = "192.168.1.4",
            LoginModelConnectButtonIsEnabled = true,
            LoginModelDisconnectButtonIsEnabled = false,
            LoginModelExpander = true,
            LoginModelRegUserName = "",
            LoginModelRegNewPsw = "",
            LoginModelRegNewRePsw = "",
            LoginModelRegStatus = "Fill Name and Password",
            LoginModelRegExpander = false,
            LoginModelRegisterButtonIsEnabled = false
            
        };
        #endregion

        #region service data
        ServiceCommunication servCom = null;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _loginModel;
            servCom = new ServiceCommunication(_loginModel,this.Dispatcher);

            Thread initializeThread = new Thread(servCom.Connect);
            initializeThread.Start();
        }


        #region UI_Events

        private void connectButton_Click(object sender, 
                                   RoutedEventArgs e)
        {
            _loginModel.LoginModelConnectButtonIsEnabled = false;
            _loginModel.LoginModelDisconnectButtonIsEnabled = true;
            _loginModel.LoginModelStatus = "Connecting...";
            
            //servCom.Connect();
            servCom.LogIn();
            //this.Visibility = System.Windows.Visibility.Hidden;
            //optWin = new OptionsWindow(servCom);

        }

        private void disconnectButton_Click(object sender,
                                  RoutedEventArgs e)
        {
            _loginModel.LoginModelConnectButtonIsEnabled = true;
            _loginModel.LoginModelDisconnectButtonIsEnabled = false;
            _loginModel.LoginModelStatus = "Disconnecting...";
            servCom.Disconnect();
        }

        private void btnRegister_Click(object sender,
                                    RoutedEventArgs e)
        {
            servCom.Register();
        }

        #endregion

    }
}
