using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.ServiceModel;
using WPFClient.SVC;
using System.Drawing;
using System.IO;

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
        Model.HeroModel _heroModel = null;
        Model.CreateHeroModel _createHeroModel = null;

        Dispatcher dispatcher = null;

        string rcvFilesPath = @"C:/WFRP/Parchments/";

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

        public void SetHeroModel(Model.HeroModel heroModel)
        {
            this._heroModel = heroModel;
        }

        public void SetCreateHeroModel(Model.CreateHeroModel createHeroModel)
        {
            this._createHeroModel = createHeroModel;
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
                    Proxy.DisconnectAsync(localClient);
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

        public void LogIn(string password)
        {
          this.localClient = new SVC.Client();
          this.localClient.Name = _loginModel.LoginModelUserName;
          this.localClient.Password = password;
          this.Proxy.LogInAsync(this.localClient);
             
        }

        public void Register(string password, string passwordConf)
        {
            if (_loginModel.LoginModelRegUserName.Length >0
                && password.Length > 0
                && passwordConf.Length > 0)
            {
                if (password == passwordConf)
                {
                    SVC.Client _client = new SVC.Client();
                    _client.Name = _loginModel.LoginModelRegUserName;
                    _client.Password = password;
                    try
                    {
                        Proxy.RegisterAsync(_client);
                    }
                    catch (Exception ex)
                    {
                        _loginModel.LoginModelRegStatus = "Error: " + ex.ToString();
                    }
                }
                else
                    _loginModel.LoginModelRegStatus = "Passwords don't match";
            }        
        }

        public void StartSession()
        {
            try
            {
                this.Proxy.StartSessionAsync(localClient, _optionsModel.OptionsModelClientListBoxSelectedItems);
            }
            catch (Exception) { }
            _optionsModel.OptionsModelAddMemberToSessionButtonIsEnabled = true;
        }

        public void EndSession()
        {
            if (session != null)
            {
                try
                {
                    this.Proxy.EndSessionAsync(localClient);
                }
                catch (Exception) { }
            }
            _optionsModel.OptionsModelStartButtonIsEnabled = true;
            _optionsModel.OptionsModelAddMemberToSessionButtonIsEnabled = false;
            session = null;
        }

        public void SendMessage(string msg)
        {
            Message m = new Message();
            m.Content = msg;
            m.Sender = localClient.Name;
            try
            {
                this.Proxy.SendAsync(m);
            }
            catch (Exception) { }
        }

        public void WhisperMessage(string msg)
        {
            if (_sessionModel.SessionModelSelectedMember != null)
            {
                Message m = new Message();
                m.Content = msg;
                m.Sender = localClient.Name;
                m.Receiver = _sessionModel.SessionModelSelectedMember;
                try
                {
                    this.Proxy.WhisperAsync(m);
                }
                catch (Exception) { }
            }
        }

        public void AddMemberToSession()
        {
            try
            {
                Proxy.AddMemberToSessionAsync(localClient, _optionsModel.OptionsModelClientListBoxSelectedItems);
            }
            catch (Exception) { }
        }

        public void UpdateParchment(Stream strm, string fileName)
        {
            if (strm != null)
            {
                byte[] buffer = new byte[(int)strm.Length];

                int i = strm.Read(buffer, 0, buffer.Length);

                if (i > 0)
                {
                    FileMessage fMsg = new FileMessage();
                    fMsg.FileName = fileName;
                    fMsg.Data = buffer;
                    try
                    {
                        Proxy.UpdateParchmentAsync(localClient, fMsg);
                        _sessionModel.SessionModelParchmentStatus = "Sending a parchment...";
                    }
                    catch (Exception) { _sessionModel.SessionModelParchmentStatus = "Error: sending a parchment has failed."; }                  
                }
            }
        }

        public void GetHero()
        {
            if (_sessionModel.SessionModelSelectedMember != null)
            {              
                try
                {
                    this.Proxy.GetHero(_sessionModel.SessionModelSelectedMember);
                }
                catch (Exception) { }
            }
        }

        public void AskEyes(string race)
        {
            try
            {
                this.Proxy.GetHeroEyeColor(race);
            }
            catch (Exception) { }
        }
        void IWFRPCallback.ReciveHeroOccupationByRace(AllHeroOccupations Occupations)
        {
            _createHeroModel.CreateHeroModelOccupation = Occupations.HeroOccupations;
        }

        public void GetOccupations(string race)
        {
            try
            {
                this.Proxy.GetHeroOccupationByRace(race);
            }
            catch (Exception) { }
        }
        public void ReciveOccupationInfo(OccupationInfo info)
        {
            _createHeroModel.CreateHeroModelOccupationText = info.Info;
        }

        public void AskOccupationDetails(string occupation)
        {
            try
            {
                this.Proxy.GetOccupationInfo(occupation);
            }
            catch (Exception) { }
        }
      
        public void HeroRegistrationPartOne(ServerMessage msg)
        {
            
        }

        public void HeroBasicInfoSubmit()
        {
            HeroBasicInfo info = new HeroBasicInfo();
            info.AccountID = Convert.ToInt16(_optionsModel.OptionsModelID);
            Console.WriteLine(_optionsModel.OptionsModelID);
            Console.WriteLine(_createHeroModel.CreateHeroModelAge);
            info.Age = _createHeroModel.CreateHeroModelAge;
            info.DontLikes = _createHeroModel.CreateHeroModelWDNHL;
            info.Enemies = _createHeroModel.CreateHeroModelEnemys;
            info.EyeColor = _createHeroModel.CreateHeroModelEyeColorItem;
            info.Family = _createHeroModel.CreateHeroModelFamily;
            info.Friends = _createHeroModel.CreateHeroModelFriends;
            info.Height = _createHeroModel.CreateHeroModelHeight;
            info.HeroName = _createHeroModel.CreateHeroModelHeroName;
            info.HHWB = _createHeroModel.CreateHeroModelwhoHeWas;
            info.Likes = _createHeroModel.CreateHeroModelWDHL;
            info.Motivation = _createHeroModel.CreateHeroModelMotivations;
            info.Origin = _createHeroModel.CreateHeroModelOrigin;
            info.Personality = _createHeroModel.CreateHeroModelPersonality;
            info.Race = _createHeroModel.SelectedItem;
            info.Sex = _createHeroModel.CreateHeroModelSexItem;
            info.SocialPosition = _createHeroModel.CreateHeroModelSocialPosition;
            info.Weight = _createHeroModel.CreateHeroModelWeight;
            info.WhoHeServes = _createHeroModel.CreateHeroModelWHS;
            info.WhyTravel = _createHeroModel.CreateHeroModelTraveling;
            try
            {
                this.Proxy.AddHeroBasicInfo(info);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        public void ViewHero()
        {
            try
            {
                this.Proxy.GetHero(localClient.Name);
            }
            catch (Exception) { }
        }

        public void SendOccupationAndRace()
        {
            HeroRaceAndOccupation info = new HeroRaceAndOccupation();
            info.Occupation = _createHeroModel.CreateHeroModelOccupationItem;
            info.Race = _createHeroModel.SelectedItem;
            this.Proxy.GetSkillsAndAbilities(info);
        }
        void IWFRPCallback.ReciveSkillsAndAbilities(OccupationAndRaceInfo info)
        {
            try
            {
                List<String> _ocAbilityIDTemp = new List<string>();
                List<String> _ocSkillsIDTemp = new List<string>();
                List<String> _rcAbilityIDTemp = new List<string>();
                List<String> _rcSkillsIDTemp = new List<string>();

                _ocAbilityIDTemp = info.OccupationAbilities.Split('$').ToList<string>();
                _ocSkillsIDTemp = info.OccupationSkills.Split('$').ToList<string>();
                _rcAbilityIDTemp = info.RaceAbilities.Split('$').ToList<string>();
                _rcSkillsIDTemp = info.RaceSkills.Split('$').ToList<string>();

                List<List<String>> _ocAbilityID = new List<List<string>>();
                List<List<String>> _ocSkillsID = new List<List<string>>();
                List<List<String>> _rcAbilityID = new List<List<string>>();
                List<List<String>> _rcSkillsID = new List<List<string>>();
                for (int i = 0; i < _ocAbilityIDTemp.Count; i++)
                {
                    _ocAbilityID.Add(_ocAbilityIDTemp[i].Split('|').ToList<string>());
                }
                _createHeroModel.CreateHeroModelOccupationAbilitiesChoose = _ocAbilityID;
                for (int i = 0; i < _ocSkillsIDTemp.Count; i++)
                {
                    _ocSkillsID.Add(_ocSkillsIDTemp[i].Split('|').ToList<string>());
                }
                _createHeroModel.CreateHeroModelOccupationSkillsChoose = _ocSkillsID;
                for (int i = 0; i < _rcAbilityIDTemp.Count; i++)
                {
                    _rcAbilityID.Add(_rcAbilityIDTemp[i].Split('|').ToList<string>());
                }
                _createHeroModel.CreateHeroModelRaceAbilitiesChoose = _rcAbilityID;
                for (int i = 0; i < _rcSkillsIDTemp.Count; i++)
                {
                    _rcSkillsID.Add(_rcSkillsIDTemp[i].Split('|').ToList<string>());
                }
                _createHeroModel.CreateHeroModelRaceSkillsChoose = _rcSkillsID;
                _ocAbilityIDTemp = new List<string>();
                _ocSkillsIDTemp = new List<string>();
                _rcAbilityIDTemp = new List<string>();
                _rcSkillsIDTemp = new List<string>();
                for (int i = 0; i < _rcAbilityID.Count; i++)
                {
                    for (int j = 0; j < _rcAbilityID[i].Count; j++)
                    {
                        _createHeroModel.CreateHeroModelRaceAbilitiesID.Add(_rcAbilityID[i][j]);
                    }
                }
                if (_createHeroModel.CreateHeroModelRaceAbilitiesID.Count > 0)
                {
                    SendRaceAbilitiesID(_createHeroModel.CreateHeroModelRaceAbilitiesID);
                }
                for (int i = 0; i < _rcSkillsID.Count; i++)
                {
                    for (int j = 0; j < _rcSkillsID[i].Count; j++)
                    {
                        _createHeroModel.CreateHeroModelRaceSkillsID.Add(_rcSkillsID[i][j]);
                    }
                }
                if (_createHeroModel.CreateHeroModelRaceSkillsID.Count > 0)
                {
                    SendRaceSkillsID(_createHeroModel.CreateHeroModelRaceSkillsID);
                }
                for (int i = 0; i < _ocAbilityID.Count; i++)
                {
                    for (int j = 0; j < _ocAbilityID[i].Count; j++)
                    {
                        _createHeroModel.CreateHeroModelOccupationAbilitiesID.Add(_ocAbilityID[i][j]);
                    }
                }
                if (_createHeroModel.CreateHeroModelOccupationAbilitiesID.Count > 0)
                {
                    SendOccupationAbilitiesID(_createHeroModel.CreateHeroModelOccupationAbilitiesID);
                }
                for (int i = 0; i < _ocSkillsID.Count; i++)
                {
                    for (int j = 0; j < _ocSkillsID[i].Count; j++)
                    {
                        _createHeroModel.CreateHeroModelOccupationSkillsID.Add(_ocSkillsID[i][j]);
                    }
                }
                if (_createHeroModel.CreateHeroModelOccupationSkillsID.Count > 0)
                {
                    SendOccupationSkillsID(_createHeroModel.CreateHeroModelOccupationSkillsID);
                }
            }
            catch (Exception) { }
        }

        public void SendRaceAbilitiesID(List<String> id)
        {
            this.Proxy.GetRaceAbilityName(id);
        }

        public void SendRaceSkillsID(List<String> id)
        {
            this.Proxy.GetRaceSkillName(id);
        }
        public void SendOccupationAbilitiesID(List<String> id)
        {
            this.Proxy.GetOccupationAbilityName(id);
        }

        public void SendOccupationSkillsID(List<String> id)
        {
            this.Proxy.GetOccupationSkillName(id);
        }
        void IWFRPCallback.ReciveRaceAbilityNames(AbilityNames abNames)
        {
            try
            {
                int k = 0;
                _createHeroModel.CreateHeroModelRaceAbilitiesNames = abNames.Names;
                for (int i = 0; i < _createHeroModel.CreateHeroModelRaceAbilitiesChoose[0].Count; i++)
                {
                    _createHeroModel.CreateHeroModelRaceAbilitiesListBox.Add(_createHeroModel.CreateHeroModelRaceAbilitiesNames[i]);
                    
                }
                for (int i = 0; i < _createHeroModel.CreateHeroModelRaceAbilitiesChoose.Count; i++)
                {
                    List<String> temp = new List<string>();
                    for (int j = 0; j < _createHeroModel.CreateHeroModelRaceAbilitiesChoose[i].Count; j++)
                    {
                        temp.Add(_createHeroModel.CreateHeroModelRaceAbilitiesNames[k]);
                        k++;
                    }
                    _createHeroModel.CreateHeroModelRaceAbilitiesNameChoose.Add(temp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        void IWFRPCallback.ReciveOccupationAbilityNames(AbilityNames abNames)
        {
            try
            {
                int k = 0;
                _createHeroModel.CreateHeroModelOccupationAbilitiesNames = abNames.Names;

                for (int i = 0; i < _createHeroModel.CreateHeroModelOccupationAbilitiesChoose[0].Count; i++)
                {
                    _createHeroModel.CreateHeroModelOccupationAbilitiesListBox.Add(_createHeroModel.CreateHeroModelOccupationAbilitiesNames[i]);
                }
                for (int i = 0; i < _createHeroModel.CreateHeroModelOccupationAbilitiesChoose.Count; i++)
                {
                    List<String> temp = new List<string>();
                    for (int j = 0; j < _createHeroModel.CreateHeroModelOccupationAbilitiesChoose[i].Count; j++)
                    {
                        temp.Add(_createHeroModel.CreateHeroModelOccupationAbilitiesNames[k]); 
                        k++;
                    }
                    _createHeroModel.CreateHeroModelOccupationAbilitiesNameChoose.Add(temp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        void IWFRPCallback.ReciveRaceSkillNames(SkillNames skNames)
        {
            try
            {
                int k = 0;
                _createHeroModel.CreateHeroModelRaceSkillsNames = skNames.Names;
                for (int i = 0; i < _createHeroModel.CreateHeroModelRaceSkillsChoose[0].Count; i++)
                {
                    _createHeroModel.CreateHeroModelRaceSkillsListBox.Add(_createHeroModel.CreateHeroModelRaceSkillsNames[i]);
                }
                for (int i = 0; i < _createHeroModel.CreateHeroModelRaceSkillsChoose.Count; i++)
                {
                    List<String> temp = new List<string>();
                    for (int j = 0; j < _createHeroModel.CreateHeroModelRaceSkillsChoose[i].Count; j++)
                    {
                        temp.Add(_createHeroModel.CreateHeroModelRaceSkillsNames[k]); 
                        k++;
                    }
                    _createHeroModel.CreateHeroModelRaceSkillsNameChoose.Add(temp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        void IWFRPCallback.ReciveOccupationSkillNames(SkillNames skNames)
        {
            try
            {
                int k = 0;
                _createHeroModel.CreateHeroModelOccupationSkillsNames = skNames.Names;
                for (int i = 0; i < _createHeroModel.CreateHeroModelOccupationSkillsChoose[0].Count; i++)
                {
                    _createHeroModel.CreateHeroModelOccupationSkillsListBox.Add(_createHeroModel.CreateHeroModelOccupationSkillsNames[i]);
                }
                for (int i = 0; i < _createHeroModel.CreateHeroModelOccupationSkillsChoose.Count; i++)
                {
                    List<String> temp = new List<string>();
                    for (int j = 0; j < _createHeroModel.CreateHeroModelOccupationSkillsChoose[i].Count; j++)
                    {
                        temp.Add(_createHeroModel.CreateHeroModelOccupationSkillsNames[k]);
                        k++;
                    }
                    _createHeroModel.CreateHeroModelOccupationSkillsNameChoose.Add(temp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void GetSkillInfo(string name)
        {
            this.Proxy.GetFullSkillInfo(name);
        }
        public void GetAbilityInfo(string name)
        {
            this.Proxy.GetFullAbilityInfo(name);
        }

        public void ReciveFullSkillInfo(FullSkillInfo skInfo)
        {
            _createHeroModel.CreateHeroModelSkAbInfo = skInfo.Description;
        }
        public void ReciveFullAbilityInfo(FullAbilityInfo abInfo)
        {
            _createHeroModel.CreateHeroModelSkAbInfo = abInfo.Description;
        }
        //TO DO add CreateHero
        public void CreateHero()
        {
            try
            {
               
            }
            catch (Exception) { }
        }

        private void InitSessionChat(Session session)
        {
            _sessionModel.SessionModelChat = "";
            _sessionModel.SessionModelChatList = new List<string>();
            _sessionModel.SessionModelParchmentStatus = "";
            _sessionModel.SessionModelParchmentSource = null;
            _optionsModel.OptionsModelStartButtonIsEnabled = false;

            this.session = session;
            List<string> members = new List<string>();
            foreach (Client c in session.Members.Keys)
                members.Add(c.Name);
            _sessionModel.SessionModelMembersListBox = members;
        }

        void IWFRPCallback.ReciveHeroEyes(HeroDetails_Eyes heroEyes)
        {
            _createHeroModel.HeroModelEyeColor=heroEyes.HeroEyes.Split('$').ToList<string>();
        }

        public void SendAbilitiesAndSkills(List<String> _abilities, List<String> _skills)
        {
            try
            {
                HeroAbilitiesAndSkills hero = new HeroAbilitiesAndSkills();
                hero.IDAcc = _optionsModel.OptionsModelID;
                hero.Occupation = _createHeroModel.CreateHeroModelOccupationItem;
                hero.Abilities = _abilities;
                hero.Skills = _skills;
                this.Proxy.AddHeroSkillsAndAbilities(hero);
            }
            catch (Exception) { }
        }
        void IWFRPCallback.ReciveFullHeroChart(HeroFullChart chart)
        {
            try
            {
                _heroModel.HeroModelDisplayWW = chart.WW;
                _heroModel.HeroModelDisplayUS = chart.US;
                _heroModel.HeroModelDisplayK = chart.Krz;
                _heroModel.HeroModelDisplayOdp = chart.Odp;
                _heroModel.HeroModelDisplayZr = chart.Zr;
                _heroModel.HeroModelDisplayInt = chart.Int;
                _heroModel.HeroModelDisplaySW = chart.Sw;
                _heroModel.HeroModelDisplayOgd = chart.Ogd;
                _heroModel.HeroModelDisplayA = chart.Atk;
                _heroModel.HeroModelDisplayZyw = chart.Zyw;
                _heroModel.HeroModelDisplayS = chart.Sil;
                _heroModel.HeroModelDisplayWt = chart.Wt;
                _heroModel.HeroModelDisplaySz = chart.Sz;
                _heroModel.HeroModelDisplayMag = chart.Mag;
                _heroModel.HeroModelDisplayPO = chart.PO;
                _heroModel.HeroModelDisplayPP = chart.PP;

            }
            catch (Exception) { }
        }
        public void SendPrimaryStats()
        {
            try
            {
                StartStats stats = new StartStats();
                stats.Id = _optionsModel.OptionsModelID;
                stats.WW = _createHeroModel.CreateHeroModelWW;
                stats.US = _createHeroModel.CreateHeroModelUS;
                stats.Krz = _createHeroModel.CreateHeroModelK;
                stats.Odp = _createHeroModel.CreateHeroModelOdp;
                stats.Zr = _createHeroModel.CreateHeroModelZr;
                stats.Int = _createHeroModel.CreateHeroModelInt;
                stats.Sw = _createHeroModel.CreateHeroModelSW;
                stats.Ogd = _createHeroModel.CreateHeroModelOgd;
                stats.Atk = "1";
                stats.Zyw = _createHeroModel.CreateHeroModelZyw;
                stats.Sil = (Convert.ToInt16(_createHeroModel.CreateHeroModelK) / 10).ToString();
                stats.Wt = (Convert.ToInt16(_createHeroModel.CreateHeroModelOdp) / 10).ToString();
                stats.Sz = _createHeroModel.CreateHeroModelSz;
                stats.Mag = "0";
                stats.PO = "0";
                stats.PP = _createHeroModel.CreateHeroModelPP;
                this.Proxy.AddStartStats(stats);
                
            }
            catch (Exception) { }
        }
        public void GetHeroChart()
        {
            this.Proxy.GetHeroChart(_optionsModel.OptionsModelID);
        }
        #region Async IWFRPCallback members

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
                    

                    try
                    {
                        Proxy.GetAllClientsAsync();
                    }
                    catch (Exception) { }
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
                if (msg.Content == "Registered")
                {
                    _loginModel.LoginModelRegExpander = false;
                    _loginModel.LoginModelExpander = true;
                }
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
            InitSessionChat(session);
            _sessionModel.SessionModelUpdateParchmentButtonIsEnabled = false;
        }

        void IWFRPCallback.SetSessionList(List<string> clients, Message msg)
        {
            _sessionModel.SessionModelMembersListBox = clients;
            _sessionModel.SessionModelChatList.Add(msg.Content + "\n");

            if (_sessionModel.SessionModelChatList.Count >= Model.SessionModel.maxChatSize)
                _sessionModel.SessionModelChatList.Remove(_sessionModel.SessionModelChatList[0]);

            _sessionModel.SessionModelChat = "";
            foreach (string s in _sessionModel.SessionModelChatList)
                _sessionModel.SessionModelChat += s;
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
            _sessionModel.SessionModelChatList.Add("[wshisper][" + msg.Sender + "]->[" + msg.Receiver + "] " + msg.Content + "\n");

            if (_sessionModel.SessionModelChatList.Count >= Model.SessionModel.maxChatSize)
                _sessionModel.SessionModelChatList.Remove(_sessionModel.SessionModelChatList[0]);

            _sessionModel.SessionModelChat = "";
            foreach (string s in _sessionModel.SessionModelChatList)
                _sessionModel.SessionModelChat += s;
        }

        void IWFRPCallback.SessionInitMGSettings(Session session)
        {
            InitSessionChat(session);
            _sessionModel.SessionModelUpdateParchmentButtonIsEnabled = true;
        }

        void IWFRPCallback.ReceivePerchment(FileMessage fMsg)
        {
            Stream fileStream = null;
            try
            {
                Directory.CreateDirectory(rcvFilesPath);
                if (!Directory.Exists(rcvFilesPath + fMsg.FileName))
                {
                    fileStream = new FileStream(rcvFilesPath + fMsg.FileName, FileMode.Create, FileAccess.ReadWrite);
                    fileStream.Write(fMsg.Data, 0, fMsg.Data.Length);
                    _sessionModel.SessionModelParchmentStatus = "Parchment received.";
                }
                else
                {
                    File.Replace(rcvFilesPath + fMsg.FileName, rcvFilesPath + fMsg.FileName, rcvFilesPath + fMsg.FileName + ".bac");
                    _sessionModel.SessionModelParchmentStatus = "Parchment received.";
                }
            }
            catch (Exception ex)
            {
                _sessionModel.SessionModelParchmentStatus = "Error: " + ex.ToString();
            }
            OperationContext clientContext = OperationContext.Current;
            clientContext.OperationCompleted += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (fileStream != null)
                    fileStream.Dispose();
                _sessionModel.SessionModelParchmentSource = rcvFilesPath + fMsg.FileName;
            });
           
        }

        void IWFRPCallback.ReceiveHero(Hero hero)
        {
            // TO DO: more hero data (also in HeroModel)
            _heroModel.HeroModelClientName = hero.ClientName;
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

        public void SessionInitMGSettings(Session session)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginSessionInitMGSettings(Session session, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndSessionInitMGSettings(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReceivePerchment(FileMessage fMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceivePerchment(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void ReceiveHero(Hero hero)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReceiveHero(Hero hero, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceiveHero(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void EndReciveHeroEyes(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        //to do
        public IAsyncResult BeginReciveHeroEyes(HeroDetails_Eyes heroEyes, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveOccupation(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReciveOccupation(AllHeroOccupations Occupations, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReciveHeroOccupationByRace(AllHeroOccupations Occupations, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveHeroOccupationByRace(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginReciveOccupationInfo(OccupationInfo info, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveOccupationInfo(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginHeroRegistrationPartOne(ServerMessage msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndHeroRegistrationPartOne(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReciveSkillsAndAbilities(OccupationAndRaceInfo info, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveSkillsAndAbilities(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginReciveAbilityNames(AbilityNames abNames, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveAbilityNames(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReciveSkillNames(SkillNames skNames, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveSkillNames(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginReciveFullAbilityInfo(FullAbilityInfo abInfo, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveFullAbilityInfo(IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public IAsyncResult BeginReciveFullSkillInfo(FullSkillInfo skInfo, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveFullSkillInfo(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginReciveFullHeroChart(HeroFullChart chart, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveFullHeroChart(IAsyncResult result)
        {
            throw new NotImplementedException();
        }



        public IAsyncResult BeginReciveRaceAbilityNames(AbilityNames abNames, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveRaceAbilityNames(IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public IAsyncResult BeginReciveOccupationAbilityNames(AbilityNames abNames, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveOccupationAbilityNames(IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public IAsyncResult BeginReciveRaceSkillNames(SkillNames skNames, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveRaceSkillNames(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReciveOccupationSkillNames(SkillNames skNames, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReciveOccupationSkillNames(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        #endregion

        

        public void ReciveAbilityNames(AbilityNames abNames)
        {
            throw new NotImplementedException();
        }

        public void ReciveSkillNames(SkillNames skNames)
        {
            throw new NotImplementedException();
        }
    }
}
