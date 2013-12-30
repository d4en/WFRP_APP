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

        #region models
        Model.LoginModel _loginModel = new Model.LoginModel {
            LoginModelStatus = "Not connected",
            LoginModelUserName = "",
            LoginModelServerIP = "localhost",
            LoginModelConnectButtonIsEnabled = true,
            LoginModelExpander = true,
            LoginModelRegUserName = "",
            LoginModelRegNewPsw = "",
            LoginModelRegNewRePsw = "",
            LoginModelRegStatus = "Fill Name and Password",
            LoginModelRegExpander = false,
            LoginModelRegisterButtonIsEnabled = false,
            LoginModelLoginWindowIsVisible = Visibility.Visible
            
        };
        #endregion

        #region service data
        ServiceCommunication servCom = null;
        public bool isLoggedIn = false;
        #endregion

        #region windows
        OptionsWindow optWin = null;
        #endregion

        Thread initializeThread = null;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _loginModel;

            servCom = new ServiceCommunication(_loginModel, this.Dispatcher);

            initializeThread = new Thread(servCom.Connect);
            initializeThread.Start();
        }


        #region UI_Events

        private void connectButton_Click(object sender, 
                                   RoutedEventArgs e)
        {
            _loginModel.LoginModelConnectButtonIsEnabled = false;
            _loginModel.LoginModelStatus = "Connecting...";
            optWin = new OptionsWindow(servCom);
            
            servCom.LogIn();
        }

        private void btnRegister_Click(object sender,
                                    RoutedEventArgs e)
        {
            servCom.Register();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if(optWin != null)
                optWin.Close();
            optWin = null;
            initializeThread.Abort();
        }

        #endregion


    }
}
