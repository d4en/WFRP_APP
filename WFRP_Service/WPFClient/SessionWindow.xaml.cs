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
using System.Drawing;

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

        private void chatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            chatTextBox.SelectionStart = chatTextBox.Text.Length;
            chatTextBox.ScrollToEnd();
        }

        private void msgTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // TO DO whisper (probably with a checkbox 'whisper' and if it's marked, sent only whisper
                if (whisperCheckBox.IsChecked == true)
                    servCom.WhisperMessage(msgTextBox.Text);
                // Message to all session members
                else
                    servCom.SendMessage(msgTextBox.Text);
                msgTextBox.Text = "";
            }

        }

        private void updateParchmentButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = @"C:\";
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";

            var result = dlg.ShowDialog();
            if (result == true)
            {
                Bitmap bmp = new Bitmap(dlg.FileName);
                servCom.UpdateParchment(bmp);

            }
            
        }

        #endregion

        

        

    }
}
