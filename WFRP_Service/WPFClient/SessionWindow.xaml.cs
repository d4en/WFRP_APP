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
    /// Interaction logic for SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {

        #region models
        Model.SessionModel _sessionModel = new Model.SessionModel
        {
            
        };
        #endregion

        #region service data
        ServiceCommunication servCom = null;
        #endregion

        public SessionWindow(ServiceCommunication servCom)
        {
            InitializeComponent();
            this.DataContext = _sessionModel;

            this.servCom = servCom;
            servCom.SetSessionModel(_sessionModel);          

        }

        #region UI events

        private void SessionWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            servCom.EndSession();
            e.Cancel = true;
            Visibility = Visibility.Collapsed;
        }

        public void Close()
        {
            this.Closing -= SessionWindow_Closing;
            base.Close();
        }

        private void chatTextBox_TextChanged(object sender, EventArgs e)
        {
            chatTextBox.SelectionStart = chatTextBox.Text.Length;
            chatTextBox.ScrollToEnd();
        }

        #endregion

        private void chatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
