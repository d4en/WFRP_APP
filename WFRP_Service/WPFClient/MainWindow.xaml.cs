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
            LoginModelServerIP = "192.168.0.11",
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

            servCom.LogIn(pswdTxtBox.Password);
        }

        private void btnRegister_Click(object sender,
                                    RoutedEventArgs e)
        {
            servCom.Register(txtBoxNewPsw.Password, txtBoxNewPswConf.Password);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if(optWin != null)
                optWin.Close();
            optWin = null;
            initializeThread.Abort();
        }
        private void ExpandLogin(object sender, RoutedEventArgs e)
        {
            _loginModel.LoginModelRegExpander = false;
        }

        private void ExpandRegister(object sender, RoutedEventArgs e)
        {
            _loginModel.LoginModelExpander = false;
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
