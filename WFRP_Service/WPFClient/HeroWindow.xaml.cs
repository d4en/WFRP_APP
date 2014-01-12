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
    /// Interaction logic for HeroWindow.xaml
    /// </summary>
    public partial class HeroWindow : Window
    {

        #region models
        Model.HeroModel _heroModel = new Model.HeroModel
        {
            
        };
        #endregion

        #region windows
        CreateHeroWindow createheroWindow = null;
        #endregion

        #region service data
        ServiceCommunication servCom = null;
        #endregion

        public HeroWindow(ServiceCommunication servCom)
        {
            InitializeComponent();
            this.DataContext = _heroModel;

            this.servCom = servCom;
            servCom.SetHeroModel(_heroModel);

            createheroWindow = new CreateHeroWindow(servCom);
            
        }

        #region UI events

        private void HeroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Collapsed;
        }

        public new void Close()
        {
            this.Closing -= HeroWindow_Closing;
            base.Close();
        }

        private void CreateHeroButton_Click(object sender, RoutedEventArgs e)
        {
            
            servCom.CreateHero();
            createheroWindow.Show();
        }

        #endregion

        #region MetroStyle window bar

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
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
