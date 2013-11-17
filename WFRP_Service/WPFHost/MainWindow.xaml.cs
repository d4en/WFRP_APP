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
using System.ServiceModel;
using Service;
using System.ServiceModel.Description;
using System.Xml;
using System.Windows.Threading;

namespace WPFHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string localhost = "localhost";//"192.168.0.11";

        public MainWindow()
        {
            InitializeComponent();
        }

        ServiceHost host;
        //delegate void MyDelegateType();

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            
            buttonStart.IsEnabled = false;

            //Define base addresses so all endPoints can go under it

            Uri tcpAdrs = new Uri("net.tcp://" + localhost + ":7997/WPFHost/");

            Uri httpAdrs = new Uri("http://" + localhost + ":7998/WPFHost/");

            Uri[] baseAdresses = { tcpAdrs, httpAdrs };

            host = new ServiceHost(typeof(Service.WFRPService), baseAdresses);


            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None, true);
            //Updated: to enable file transefer of 64 MB
            tcpBinding.MaxBufferPoolSize = (int)67108864;
            tcpBinding.MaxBufferSize = 67108864;
            tcpBinding.MaxReceivedMessageSize = (int)67108864;
            tcpBinding.TransferMode = TransferMode.Buffered;
            tcpBinding.ReaderQuotas.MaxArrayLength = 67108864;
            tcpBinding.ReaderQuotas.MaxBytesPerRead = 67108864;
            tcpBinding.ReaderQuotas.MaxStringContentLength = 67108864;


            tcpBinding.MaxConnections = 100;
            //To maxmize MaxConnections you have to assign another port for mex endpoint

            //and configure ServiceThrottling as well
            ServiceThrottlingBehavior throttle;
            throttle = host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (throttle == null)
            {
                throttle = new ServiceThrottlingBehavior();
                throttle.MaxConcurrentCalls = 100;
                throttle.MaxConcurrentSessions = 100;
                host.Description.Behaviors.Add(throttle);
            }


            //Enable reliable session and keep the connection alive for 20 hours.
            tcpBinding.ReceiveTimeout = new TimeSpan(20, 0, 0);
            tcpBinding.ReliableSession.Enabled = true;
            tcpBinding.ReliableSession.InactivityTimeout = new TimeSpan(20, 0, 10);

            host.AddServiceEndpoint(typeof(Service.IWFRP), tcpBinding, "tcp");

            //Define Metadata endPoint, So we can publish information about the service
            ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
            host.Description.Behaviors.Add(mBehave);

            host.AddServiceEndpoint(typeof(IMetadataExchange),
                MetadataExchangeBindings.CreateMexTcpBinding(),
                "net.tcp://" + localhost + ":7996/WPFHost/mex");

            //MyDelegateType work = delegate
            //{
            //    DoTheMainWorkDone();
            //};
            //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, work);

            try
            {
                host.Open();
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message.ToString();
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    labelStatus.Content = "Opened";
                    buttonStop.IsEnabled = true;
                }
            }
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
           

            if (host != null)
            {
                try
                {
                    host.Close();
                }
                catch (Exception ex)
                {
                    labelStatus.Content = ex.Message.ToString();
                }
                finally
                {
                    if (host.State == CommunicationState.Closed)
                    {
                        labelStatus.Content = "Closed";
                        buttonStart.IsEnabled = true;
                        buttonStop.IsEnabled = false;
                    }
                }
            }
        }

        private void btn_test_Click(object sender, RoutedEventArgs e)
        {
            DBConnector DBcon = new DBConnector();
            Client client = new Client();
            client.Name = "asdadkjdadddafsdadasffdd";
            client.Password = "asdd";
            Console.WriteLine(DBcon.LogIn(client));

        }



        //private void DoTheMainWorkDone()
        //{
        //    Console.WriteLine("DoTheMainWorkDone");
        //}
    }
}
