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


        public SVC.Client localClient = null;
        public SVC.Session session = null;

        Model.LoginModel _loginModel = null;
        Model.OptionsModel _optionsModel = null;
        Model.SessionModel _sessionModel = null;
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

        public void SetOptionsModel(Model.OptionsModel optionsModel)
        {
            this._optionsModel = optionsModel;
        }

        public void SetSessionModel(Model.SessionModel sessionModel)
        {
            this._sessionModel = sessionModel;
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
                        _optionsModel.OptionsModelStatus = "Disconnected";
                        _loginModel.LoginModelConnectButtonIsEnabled = true;
                        _optionsModel.OptionsModelDisconnectButtonIsEnabled = false;
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
                        _optionsModel.OptionsModelStatus = "Disconnected";
                        _loginModel.LoginModelConnectButtonIsEnabled = true;
                        _optionsModel.OptionsModelDisconnectButtonIsEnabled = false;
                        break;
                    case CommunicationState.Opened:
                        _loginModel.LoginModelStatus = "Connected";
                        _optionsModel.OptionsModelStatus = "Connected";
                        _loginModel.LoginModelConnectButtonIsEnabled = false;
                        _optionsModel.OptionsModelDisconnectButtonIsEnabled = true;
                        break;
                    case CommunicationState.Opening:
                        break;
                    default:
                        break;
                }
            }

        }

        void proxy_ConnectCompleted(object sender,
                   InitializeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _loginModel.LoginModelStatus = e.ToString();
            }
            else if (e.Result)
            {
                HandleProxy();
            }
            else if (!e.Result)
            {
                _loginModel.LoginModelStatus = "Name found - already logged in!";
                _optionsModel.OptionsModelStatus = "Name found - already logged in!";
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
                    _loginModel.LoginModelStatus = "Offline";
                    _loginModel.LoginModelConnectButtonIsEnabled = false;

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
                    _loginModel.LoginModelStatus = "SERVER UP";
                    _loginModel.LoginModelConnectButtonIsEnabled = true;
                    _loginModel.LoginModelRegisterButtonIsEnabled = true;
                    
                    Proxy.InnerDuplexChannel.Faulted +=
                      new EventHandler(InnerDuplexChannel_Faulted);
                    Proxy.InnerDuplexChannel.Opened +=
                      new EventHandler(InnerDuplexChannel_Opened);
                    Proxy.InnerDuplexChannel.Closed +=
                      new EventHandler(InnerDuplexChannel_Closed);
                    Proxy.Initialize();
                    Proxy.InitializeCompleted += new EventHandler<
                          InitializeCompletedEventArgs>(proxy_ConnectCompleted);
                    
                }
                catch (Exception ex)
                {
                    _loginModel.LoginModelStatus = "Offline";
                    _loginModel.LoginModelStatus += " / " + ex.ToString();
                    _loginModel.LoginModelConnectButtonIsEnabled = true;
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
                    Proxy.Disconnect(localClient);
                }
                catch (Exception ex)
                {
                    _loginModel.LoginModelStatus = "Uknown";
                    _loginModel.LoginModelStatus += " / " + ex.ToString();
                    _loginModel.LoginModelConnectButtonIsEnabled = false;
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = true;
                }
            }
            else
            {
                HandleProxy();
            }
        }

        public void LogIn()
        {
          this.localClient = new SVC.Client();
          this.localClient.Name = _loginModel.LoginModelUserName;
          this.localClient.Password = _loginModel.LoginModelPswd;
          this.Proxy.LogInAsync(this.localClient);
             
        }

        public void Register()
        {
            if (_loginModel.LoginModelRegUserName.Length >0
                && _loginModel.LoginModelRegNewPsw.Length >0
                && _loginModel.LoginModelRegNewRePsw.Length >0)
            {
                if (_loginModel.LoginModelRegNewPsw == _loginModel.LoginModelRegNewRePsw)
                {
                    SVC.Client _client = new SVC.Client();
                    _client.Name = _loginModel.LoginModelRegUserName;
                    _client.Password = _loginModel.LoginModelRegNewPsw;

                    Proxy.Register(_client);
                }
                else
                    _loginModel.LoginModelRegStatus = "Passwords don't match";
            }        
        }

        public void StartSession()
        {        
            this.Proxy.StartSession(localClient, _optionsModel.OptionsModelClientListBoxSelectedItems);
        }

        public void EndSession()
        {
            if (session != null)
                this.Proxy.EndSessionAsync(localClient);
            _optionsModel.OptionsModelStartButtonIsEnabled = true;
            session = null;
        }

        public void SendMessage(string msg)
        {
            Message m = new Message();
            m.Content = msg;
            m.Sender = localClient.Name;
            this.Proxy.Send(m);
        }

        public void WhisperMessage(string msg)
        {
            Message m = new Message();
            m.Content = msg;
            m.Sender = localClient.Name;
            this.Proxy.Whisper(m, _sessionModel.SessionModelSelectedMember);
        }

        #region IWFRPCallback Members

        public void GetServerMessageStatus(WPFClient.SVC.ServerMessage msg)
        {
            if (msg.Type == ServerMessageType.Login)
            {
                if (msg.IsStatusCorrect)
                {                    
                    _loginModel.LoginModelConnectButtonIsEnabled = false;
                    _loginModel.LoginModelStatus = "Connected";
                    _loginModel.LoginModelRegStatus = "Disconnect in order to register.";
                    _loginModel.LoginModelRegisterButtonIsEnabled = false;

                    _optionsModel.OptionsModelMsg = "Success! " + msg.Content;
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = true;
                    _optionsModel.OptionsModelOptionsWindowIsVisible = System.Windows.Visibility.Visible;
                    _optionsModel.OptionsModelStatus = "Connected";

                    Proxy.GetAllClients();
                }
                else
                {
                    _loginModel.LoginModelStatus = "Error: " + msg.Content;
                    _loginModel.LoginModelConnectButtonIsEnabled = true;
                    _loginModel.LoginModelRegStatus = "Fill Name and Password";
                    _loginModel.LoginModelRegisterButtonIsEnabled = true;

                    _optionsModel.OptionsModelStatus = "Disconnected";
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = false;
                    _optionsModel.OptionsModelOptionsWindowIsVisible = System.Windows.Visibility.Hidden;
                }
            }
            else if (msg.Type == ServerMessageType.DisconnectInfoClient)
            {
                if (msg.IsStatusCorrect)
                {
                    _loginModel.LoginModelConnectButtonIsEnabled = true;
                    _loginModel.LoginModelStatus = "Disconnected";
                    _loginModel.LoginModelRegStatus = "Fill Name and Password";
                    _loginModel.LoginModelRegisterButtonIsEnabled = true;

                    _optionsModel.OptionsModelMsg = "Success! " + msg.Content;
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = false;
                    _optionsModel.OptionsModelStatus = "Disconnected";
                    _optionsModel.OptionsModelOptionsWindowIsVisible = System.Windows.Visibility.Hidden;
                }
                else
                {
                    _loginModel.LoginModelConnectButtonIsEnabled = false;
                    _loginModel.LoginModelStatus = "Uknown";
                    _loginModel.LoginModelRegStatus = "Disconnect in order to register.";
                    _loginModel.LoginModelRegisterButtonIsEnabled = false;

                    _optionsModel.OptionsModelMsg = "Error: " + msg.Content;
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = true;
                    _optionsModel.OptionsModelStatus = "Unknown";
                }
            }
            else if (msg.Type == ServerMessageType.Register)
            {
                _loginModel.LoginModelRegStatus = msg.Content;
            }
            else if (msg.Type == ServerMessageType.StartSession)
            {
                _optionsModel.OptionsModelMsg = msg.Content;
            }
            else
            {
                _loginModel.LoginModelRegStatus = "Error";
                _optionsModel.OptionsModelStatus = "Error";
                _optionsModel.OptionsModelMsg = "Uknown server message type.";
            }
        }

        public void GetIdentity(Identity userID)
        {
            _optionsModel.OptionsModelID = userID.AccountID;
        }

        public void SetClientList(List<string> clients)
        {
            _optionsModel.OptionsModelClientListBox = clients;
        }

        public void JoinedToSession(ServerMessage msg)
        {
            _optionsModel.OptionsModelMsg = msg.Content + " [member]";
            _sessionModel.SessionModelSessionWindowIsVisible = System.Windows.Visibility.Visible;
        }

        public void SessionInitSettings(Session session)
        {
            _sessionModel.SessionModelChat = "";
            _sessionModel.SessionModelChatList = new List<string>();
            _optionsModel.OptionsModelStartButtonIsEnabled = false;

            this.session = session;
            List<string> members = new List<string>();
            foreach (Client c in session.Members.Keys)
                members.Add(c.Name);
            _sessionModel.SessionModelMembersListBox = members;
        }

        public void SetSessionList(List<string> clients, Message msg)
        {
            _sessionModel.SessionModelMembersListBox = clients;
            _sessionModel.SessionModelChat += msg.Content + "\n";
        }

        public void Receive(Message msg)
        {
            _sessionModel.SessionModelChatList.Add("[" + msg.Sender + "] " + msg.Content + "\n");

            if (_sessionModel.SessionModelChatList.Count >= Model.SessionModel.maxChatSize)
                _sessionModel.SessionModelChatList.Remove(_sessionModel.SessionModelChatList[0]);

            _sessionModel.SessionModelChat = "";
            foreach (string s in _sessionModel.SessionModelChatList)
                _sessionModel.SessionModelChat += s;
        }

        public void ReceiveWhisper(Message msg)
        {
            _sessionModel.SessionModelChatList.Add("[wshisper][" + msg.Sender + "] " + msg.Content + "\n");

            if (_sessionModel.SessionModelChatList.Count >= Model.SessionModel.maxChatSize)
                _sessionModel.SessionModelChatList.Remove(_sessionModel.SessionModelChatList[0]);

            _sessionModel.SessionModelChat = "";
            foreach (string s in _sessionModel.SessionModelChatList)
                _sessionModel.SessionModelChat += s;
        }

        #endregion


        #region Async

        IAsyncResult IWFRPCallback.BeginGetServerMessageStatus(ServerMessage msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IWFRPCallback.EndGetServerMessageStatus(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetServerMessageStatus(ServerMessage msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndGetServerMessageStatus(IAsyncResult result)
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

        void IWFRPCallback.GetServerMessageStatus(ServerMessage msg)
        {
            if (msg.Type == ServerMessageType.Login)
            {
                if (msg.IsStatusCorrect)
                {
                    _loginModel.LoginModelConnectButtonIsEnabled = false;
                    _loginModel.LoginModelStatus = "Connected";
                    _loginModel.LoginModelRegStatus = "Disconnect in order to register.";
                    _loginModel.LoginModelRegisterButtonIsEnabled = false;

                    _optionsModel.OptionsModelMsg = "Success! " + msg.Content;
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = true;
                    _optionsModel.OptionsModelOptionsWindowIsVisible = System.Windows.Visibility.Visible;
                    _optionsModel.OptionsModelStatus = "Connected";

                    Proxy.GetAllClients();
                }
                else
                {
                    _loginModel.LoginModelStatus = "Error: " + msg.Content;
                    _loginModel.LoginModelConnectButtonIsEnabled = true;
                    _loginModel.LoginModelRegStatus = "Fill Name and Password";
                    _loginModel.LoginModelRegisterButtonIsEnabled = true;

                    _optionsModel.OptionsModelStatus = "Disconnected";
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = false;
                    _optionsModel.OptionsModelOptionsWindowIsVisible = System.Windows.Visibility.Hidden;
                }
            }
            else if (msg.Type == ServerMessageType.DisconnectInfoClient)
            {
                if (msg.IsStatusCorrect)
                {
                    _loginModel.LoginModelConnectButtonIsEnabled = true;
                    _loginModel.LoginModelStatus = "Disconnected";
                    _loginModel.LoginModelRegStatus = "Fill Name and Password";
                    _loginModel.LoginModelRegisterButtonIsEnabled = true;

                    _optionsModel.OptionsModelMsg = "Success! " + msg.Content;
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = false;
                    _optionsModel.OptionsModelStatus = "Disconnected";
                    _optionsModel.OptionsModelOptionsWindowIsVisible = System.Windows.Visibility.Hidden;
                }
                else
                {
                    _loginModel.LoginModelConnectButtonIsEnabled = false;
                    _loginModel.LoginModelStatus = "Uknown";
                    _loginModel.LoginModelRegStatus = "Disconnect in order to register.";
                    _loginModel.LoginModelRegisterButtonIsEnabled = false;

                    _optionsModel.OptionsModelMsg = "Error: " + msg.Content;
                    _optionsModel.OptionsModelDisconnectButtonIsEnabled = true;
                    _optionsModel.OptionsModelStatus = "Unknown";
                }
            }
            else if (msg.Type == ServerMessageType.Register)
            {
                _loginModel.LoginModelRegStatus = msg.Content;
            }
            else if (msg.Type == ServerMessageType.StartSession)
            {
                _optionsModel.OptionsModelMsg = msg.Content;
            }
            else
            {
                _loginModel.LoginModelRegStatus = "Error";
                _optionsModel.OptionsModelStatus = "Error";
                _optionsModel.OptionsModelMsg = "Uknown server message type.";
            }
        }

        void IWFRPCallback.GetIdentity(Identity userID)
        {
            _optionsModel.OptionsModelID = userID.AccountID;
        }

        void IWFRPCallback.SetClientList(List<string> clients)
        {
            _optionsModel.OptionsModelClientListBox = clients;
        }

        void IWFRPCallback.JoinedToSession(ServerMessage msg)
        {           
            _optionsModel.OptionsModelMsg = msg.Content + " [member]";
            _sessionModel.SessionModelSessionWindowIsVisible = System.Windows.Visibility.Visible;
        }

        void IWFRPCallback.SessionInitSettings(Session session)
        {
            _sessionModel.SessionModelChat = "";
            _sessionModel.SessionModelChatList = new List<string>();
            _optionsModel.OptionsModelStartButtonIsEnabled = false;

            this.session = session;
            List<string> members = new List<string>();
            foreach (Client c in session.Members.Keys)
                members.Add(c.Name);
            _sessionModel.SessionModelMembersListBox = members;
        }

        void IWFRPCallback.SetSessionList(List<string> clients, Message msg)
        {
            _sessionModel.SessionModelMembersListBox = clients;
            _sessionModel.SessionModelChat += msg.Content + "\n";
        }

        void IWFRPCallback.Receive(Message msg)
        {
            _sessionModel.SessionModelChatList.Add("[" + msg.Sender + "] " + msg.Content + "\n");

            if (_sessionModel.SessionModelChatList.Count >= Model.SessionModel.maxChatSize)
                _sessionModel.SessionModelChatList.Remove(_sessionModel.SessionModelChatList[0]);

            _sessionModel.SessionModelChat = "";
            foreach (string s in _sessionModel.SessionModelChatList)
                _sessionModel.SessionModelChat += s;
        }

        void IWFRPCallback.ReceiveWhisper(Message msg)
        {
            _sessionModel.SessionModelChatList.Add("[wshisper][" + msg.Sender + "] " + msg.Content + "\n");

            if (_sessionModel.SessionModelChatList.Count >= Model.SessionModel.maxChatSize)
                _sessionModel.SessionModelChatList.Remove(_sessionModel.SessionModelChatList[0]);

            _sessionModel.SessionModelChat = "";
            foreach (string s in _sessionModel.SessionModelChatList)
                _sessionModel.SessionModelChat += s;
        }

        IAsyncResult IWFRPCallback.BeginGetIdentity(Identity userID, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        void IWFRPCallback.EndGetIdentity(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginSetClientList(List<string> clients, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndSetClientList(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginJoinedToSession(ServerMessage msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndJoinedToSession(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginSessionInitSettings(Session session, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndSessionInitSettings(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginSetSessionList(List<string> clients, Message msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndSetSessionList(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReceive(Message msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceive(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReceiveWhisper(Message msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceiveWhisper(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion

        
    }
}
