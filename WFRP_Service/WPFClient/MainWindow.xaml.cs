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
using System.ServiceModel;
using WPFClient.SVC;
using System.Collections;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, SVC.IWFRPCallback
    {
        //SVC holds references to the proxy and cotracts..
        SVC.WFRPClient proxy = null;
        SVC.Client localClient = null;

        //When the communication object 
        //turns to fault state it will
        //require another thread to invoke a fault event
        private delegate void FaultedInvoker();

        public MainWindow()
        {
            InitializeComponent();
        }

        //Service might be disconnected or stopped for any reason,
        //so we have to handle the state of the communication object,
        //the communication object will fire 
        //an event for each transitioning
        //from a state to another, notice that when a connection state goes
        //from opening to opened or from opened to closing state.. it can't go
        //back so, if it is closed or faulted you have to set the proxy = null;
        //to be able to create a proxy again and open a connection
        //..
        //I have made a method called HandleProxy() to handle the state
        //of the connection, so in each event like opened, closed or faulted
        //we will call this method, and it will switch on the connection state
        //and apply a suitable reaction.
        //..
        //Because this events will need to be invoked on another thread
        //you can do like so in WPF applications (I've got this idea from
        //Sacha Barber's greate article on WCF WPF Application)

        void InnerDuplexChannel_Closed(object sender, EventArgs e)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, 
                                new FaultedInvoker(HandleProxy));
                return;
            }
            HandleProxy();
        }

        void InnerDuplexChannel_Opened(object sender, EventArgs e)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, 
                                new FaultedInvoker(HandleProxy));
                return;
            }
            HandleProxy();
        }

        void InnerDuplexChannel_Faulted(object sender, EventArgs e)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, 
                                new FaultedInvoker(HandleProxy));
                return;
            }
            HandleProxy();
        }

        #region Private Methods

        /// <span class="code-SummaryComment">< summary></span>
        /// This is the most method I like, it helps us alot
        /// We may can't know when a connection is lost in 
        /// of network failure or service stopped.
        /// And also to maintain performance client doesnt know
        /// that the connection will be lost when hitting the 
        /// disconnect button, but when a session is terminated
        /// this method will be called, and it will handle everything.
        /// <span class="code-SummaryComment">< /summary></span>
        private void HandleProxy()
        {
            if (proxy != null)
            {
                switch (this.proxy.State)
                {
                    case CommunicationState.Closed:
                        loginLabelStatus.Content = "Disconnected";
                        loginButtonConnect.IsEnabled = true;
                        proxy = null;
                        break;
                    case CommunicationState.Closing:
                        break;
                    case CommunicationState.Created:
                        break;
                    case CommunicationState.Faulted:
                        proxy.Abort();
                        proxy = null;
                        loginLabelStatus.Content = "Disconnected";
                        loginButtonConnect.IsEnabled = true;
                        break;
                    case CommunicationState.Opened:
                        loginLabelStatus.Content = "Connected";
                        loginButtonDisconnect.IsEnabled = true;
                        break;
                    case CommunicationState.Opening:
                        break;
                    default:
                        break;
                }
            }

        }
        
        /// <span class="code-SummaryComment">< summary></span>
        /// This is the second important method, which creates 
        /// the proxy, subscribe to connection state events
        /// and open a connection with the service
        /// <span class="code-SummaryComment">< /summary></span>
        private void Connect()
        {
            if (proxy == null)
            {
                try
                {
                    this.localClient = new SVC.Client();
                    this.localClient.Name = loginTxtBoxUName.Text.ToString();
                    InstanceContext context = new InstanceContext(this);
                    proxy = new SVC.WFRPClient(context);

                    //As the address in the configuration file is set to localhost
                    //we want to change it so we can call a service in internal 
                    //network, or over internet
                    string servicePath = proxy.Endpoint.ListenUri.AbsolutePath;
                    string serviceListenPort = 
                      proxy.Endpoint.Address.Uri.Port.ToString();
                    proxy.Endpoint.Address = new EndpointAddress("net.tcp://" 
                       + loginTxtBoxIP.Text.ToString() + ":" + 
                       serviceListenPort + servicePath);


                    proxy.Open();

                    proxy.InnerDuplexChannel.Faulted += 
                      new EventHandler(InnerDuplexChannel_Faulted);
                    proxy.InnerDuplexChannel.Opened += 
                      new EventHandler(InnerDuplexChannel_Opened);
                    proxy.InnerDuplexChannel.Closed += 
                      new EventHandler(InnerDuplexChannel_Closed);
                    proxy.ConnectAsync(this.localClient);
                    proxy.ConnectCompleted += new EventHandler< 
                          ConnectCompletedEventArgs>(proxy_ConnectCompleted);
                }
                catch (Exception ex)
                {
                    loginTxtBoxUName.Text = ex.Message.ToString();
                    loginLabelStatus.Content = "Offline";
                    loginButtonConnect.IsEnabled = true;
                }
            }
            else
            {
                HandleProxy();
            }
        }

        private void Disconnect()
        {
            if (proxy != null)
            {
                try
                {
                    proxy.DisconnectAsync(localClient);
                }
                catch (Exception ex)
                {
                    loginTxtBoxUName.Text = ex.Message.ToString();
                    loginLabelStatus.Content = "Uknown";
                    loginButtonDisconnect.IsEnabled = true;
                }
            }
            else
            {
                HandleProxy();
            }
        }

        #endregion


        #region UI_Events

        private void buttonConnect_Click(object sender, 
                                   RoutedEventArgs e)
        {
            loginLabelStatus.Content = "Connecting..";
            proxy = null;
            Connect();

        }

        private void buttonDisconnect_Click(object sender,
                                  RoutedEventArgs e)
        {
            loginButtonConnect.IsEnabled = true;
            loginButtonDisconnect.IsEnabled = true;
            loginLabelStatus.Content = "Disconnecting..";
            Disconnect();
        }

        void proxy_ConnectCompleted(object sender, 
                   ConnectCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                loginLabelStatus.Foreground = 
                        new SolidColorBrush(Colors.Red);
                loginTxtBoxUName.Text = e.Error.Message.ToString();
                loginButtonConnect.IsEnabled = true;
            }
            else if (e.Result)
            {
                HandleProxy();
            }
            else if (!e.Result)
            {
                loginLabelStatus.Content = "Name found";
                loginButtonConnect.IsEnabled = true;
            }


        }

        #endregion

        #region IChatCallback Members

        public void Receive(WPFClient.SVC.Message msg)
        {
            serverMsg.Content = "MESSAGE RECEIVED: " + msg.Content;
        }

        #endregion

        #region Async

        public IAsyncResult BeginReceive(Message msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceive(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
