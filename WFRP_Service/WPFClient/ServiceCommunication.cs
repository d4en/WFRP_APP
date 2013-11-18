using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.ServiceModel;
using WPFClient.SVC;

namespace WPFClient
{
    public class ServiceCommunication : SVC.IWFRPCallback
    {
        //SVC holds references to the Proxy and cotracts..
        public SVC.WFRPClient Proxy { get; set; }
        
        SVC.Client localClient = null;
        Model.LoginModel _loginModel = null;
        Dispatcher dispatcher = null;

        //When the communication object 
        //turns to fault state it will
        //require another thread to invoke a fault event
        private delegate void FaultedInvoker();


        public ServiceCommunication(Model.LoginModel loginModel, Dispatcher dispatcher)
        {
            this._loginModel = loginModel;
            this.dispatcher = dispatcher;
        }

        //Service might be disconnected or stopped for any reason,
        //so we have to handle the state of the communication object,
        //the communication object will fire 
        //an event for each transitioning
        //from a state to another, notice that when a connection state goes
        //from opening to opened or from opened to closing state.. it can't go
        //back so, if it is closed or faulted you have to set the Proxy = null;
        //to be able to create a Proxy again and open a connection
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
            if (!this.dispatcher.CheckAccess())
            {
                this.dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                new FaultedInvoker(HandleProxy));
                return;
            }
            HandleProxy();
        }

        void InnerDuplexChannel_Opened(object sender, EventArgs e)
        {
            if (!this.dispatcher.CheckAccess())
            {
                this.dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                new FaultedInvoker(HandleProxy));
                return;
            }
            HandleProxy();
        }

        void InnerDuplexChannel_Faulted(object sender, EventArgs e)
        {
            if (!this.dispatcher.CheckAccess())
            {
                this.dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                new FaultedInvoker(HandleProxy));
                return;
            }
            HandleProxy();
        }

        /// This is the most method I like, it helps us alot
        /// We may can't know when a connection is lost in 
        /// of network failure or service stopped.
        /// And also to maintain performance client doesnt know
        /// that the connection will be lost when hitting the 
        /// disconnect button, but when a session is terminated
        /// this method will be called, and it will handle everything.
        public void HandleProxy()
        {
            if (Proxy != null)
            {
                switch (this.Proxy.State)
                {
                    case CommunicationState.Closed:
                        _loginModel.LoginModelStatus = "Disconnected";
                        _loginModel.LoginModelConnectButtonIsEnabled = true;
                        _loginModel.LoginModelDisconnectButtonIsEnabled = false;
                        Proxy = null;
                        break;
                    case CommunicationState.Closing:
                        break;
                    case CommunicationState.Created:
                        break;
                    case CommunicationState.Faulted:
                        Proxy.Abort();
                        Proxy = null;
                        _loginModel.LoginModelStatus = "Disconnected";
                        _loginModel.LoginModelConnectButtonIsEnabled = true;
                        _loginModel.LoginModelDisconnectButtonIsEnabled = false;
                        break;
                    case CommunicationState.Opened:
                        _loginModel.LoginModelStatus = "Connected";
                        _loginModel.LoginModelConnectButtonIsEnabled = false;
                        _loginModel.LoginModelDisconnectButtonIsEnabled = true;
                        break;
                    case CommunicationState.Opening:
                        break;
                    default:
                        break;
                }
            }

        }

        void proxy_ConnectCompleted(object sender,
                   LogInCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _loginModel.LoginModelStatus = e.ToString();
                _loginModel.LoginModelConnectButtonIsEnabled = true;
                _loginModel.LoginModelDisconnectButtonIsEnabled = false;
            }
            else if (e.Result)
            {
                HandleProxy();
            }
            else if (!e.Result)
            {
                _loginModel.LoginModelStatus = "Name found - already logged in!";
                _loginModel.LoginModelConnectButtonIsEnabled = true;
                _loginModel.LoginModelDisconnectButtonIsEnabled = false;
            }
        }

        /// This is the second important method, which creates 
        /// the Proxy, subscribe to connection state events
        /// and open a connection with the service
        public void Connect()
        {
            if (Proxy == null)
            {
                try
                {
                    this.localClient = new SVC.Client();
                    this.localClient.Name = _loginModel.LoginModelUserName;
                    this.localClient.Password = _loginModel.LoginModelPswd;

                    InstanceContext context = new InstanceContext(this);
                    Proxy = new SVC.WFRPClient(context);

                    //As the address in the configuration file is set to localhost
                    //we want to change it so we can call a service in internal 
                    //network, or over internet
                    string servicePath = Proxy.Endpoint.ListenUri.AbsolutePath;
                    string serviceListenPort =
                      Proxy.Endpoint.Address.Uri.Port.ToString();
                    Proxy.Endpoint.Address = new EndpointAddress("net.tcp://"
                       + _loginModel.LoginModelServerIP + ":" +
                       serviceListenPort + servicePath);

                    Proxy.Open();

                    Proxy.InnerDuplexChannel.Faulted +=
                      new EventHandler(InnerDuplexChannel_Faulted);
                    Proxy.InnerDuplexChannel.Opened +=
                      new EventHandler(InnerDuplexChannel_Opened);
                    Proxy.InnerDuplexChannel.Closed +=
                      new EventHandler(InnerDuplexChannel_Closed);
                    Proxy.LogInAsync(this.localClient);
                    Proxy.LogInCompleted += new EventHandler<
                          LogInCompletedEventArgs>(proxy_ConnectCompleted);
                }
                catch (Exception ex)
                {
                    _loginModel.LoginModelStatus = "Offline";
                    _loginModel.LoginModelStatus += " / " + ex.ToString();
                    _loginModel.LoginModelConnectButtonIsEnabled = true;
                    _loginModel.LoginModelDisconnectButtonIsEnabled = false;
                }
            }
            else
            {
                HandleProxy();
            }
        }

        public void Disconnect()
        {
            if (Proxy != null)
            {
                try
                {
                    Proxy.DisconnectAsync(localClient);
                    _loginModel.LoginModelStatus = "Disconnected";
                    _loginModel.LoginModelMsg = "";
                }
                catch (Exception ex)
                {
                    _loginModel.LoginModelStatus = "Uknown";
                    _loginModel.LoginModelStatus += " / " + ex.ToString();
                    _loginModel.LoginModelConnectButtonIsEnabled = false;
                    _loginModel.LoginModelDisconnectButtonIsEnabled = true;
                }
            }
            else
            {
                HandleProxy();
            }
        }


        #region IWFRPCallback Members

        public void GetStatus(WPFClient.SVC.Message msg)
        {
            _loginModel.LoginModelMsg = "MESSAGE RECEIVED: " + msg.Content;
        }

        public void GetIdentity(Identity userID)
        {
            _loginModel.LoginModelID += " " + userID.AccountID;
        }

        #endregion


        #region Async

        public IAsyncResult BeginGetStatus(Message msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndGetStatus(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetIdentity(Identity userID, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndGetIdentity(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion




      
    }
}
