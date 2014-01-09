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
using System.IO;

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

        #region windows
        HeroWindow heroWindow = null;
        #endregion

        public SessionWindow(ServiceCommunication servCom, HeroWindow heroWindow)
        {
            InitializeComponent();
            this.DataContext = _sessionModel;

            this.servCom = servCom;
            servCom.SetSessionModel(_sessionModel);

            this.heroWindow = heroWindow;

        }

        #region UI events

        private void SessionWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            servCom.EndSession();
            e.Cancel = true;
            Visibility = Visibility.Collapsed;

            if (heroWindow != null)
                heroWindow.Hide();
        }

        public new void Close()
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
                // whisper (with a checkbox 'whisper' marked)
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
            Stream strm = null;
            try
            {
                Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
                fileDialog.Multiselect = false;
                fileDialog.InitialDirectory = @"C:\";
                fileDialog.DefaultExt = ".jpg";
                fileDialog.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";

                if (fileDialog.ShowDialog() == DialogResult.HasValue)
                    return;

                strm = fileDialog.OpenFile();
                if ((int)strm.Length < Model.SessionModel.maxFileSize)
                    servCom.UpdateParchment(strm, fileDialog.SafeFileName);
                else
                {
                    MessageBoxResult msgBox = MessageBox.Show("File sending is limited to " + Model.SessionModel.maxFileSize + " bytes.", "Error", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult msgBox = MessageBox.Show("File sending failed!\n" + ex.ToString(), "Error", MessageBoxButton.OK);
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                }
            }
            
        }

        private void MembersListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_sessionModel.SessionModelSelectedMember != null)
            {
                if (heroWindow == null)
                    heroWindow = new HeroWindow(servCom);
                servCom.GetHero();
                heroWindow.Show();
            }
        }

        #endregion 
 
        #region MetroStyle window bar

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            servCom.EndSession();
            Visibility = Visibility.Collapsed;

            if (heroWindow != null)
                heroWindow.Hide();
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
