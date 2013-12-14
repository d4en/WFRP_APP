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

        private void SessionWindow_Closed(object sender, EventArgs e)
        {           
            //servCom.EndSession();
        }
    }
}
