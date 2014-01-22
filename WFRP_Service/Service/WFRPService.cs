using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false)]
    public class WFRPService : IWFRP
    {
        Dictionary<Client, IWFRPCallback> clients =
                 new Dictionary<Client, IWFRPCallback>();
        
        List<Client> clientList = new List<Client>();
        List<Session> sessionList = new List<Session>();

        public IWFRPCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.
                       GetCallbackChannel<IWFRPCallback>();
            }
        }

        object syncObj = new object();

        /// <summary>
        /// Local function that searches in clients dictionary for a client with current name
        /// </summary>
        /// <param name="name">name by which client will be searched</param>
        /// <returns></returns>
        private bool SearchClientsByName(string name)
        {
            foreach (Client c in clients.Keys)
            {
                if (c.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Searching a session of which member is a client with name
        /// </summary>
        /// <param name="name">clients name</param>
        /// <returns>session</returns>
        private Session SearchSessionByClientName(string name)
        {
            foreach (Session s in sessionList)
            {
                foreach (Client c in s.Members.Keys)
                {
                    if (c.Name == name)
                        return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Local function that searches in clients dictionary for a callback with current name
        /// </summary>
        /// <param name="name">name by which callback will be searche</param>
        /// <returns></returns>
        private KeyValuePair<Client, IWFRPCallback> SearchClientCallbackByName(string name)
        {
            KeyValuePair<Client, IWFRPCallback> client = new KeyValuePair<Client, IWFRPCallback>();
            foreach (KeyValuePair<Client, IWFRPCallback> c in clients)
            {
                if (c.Key.Name == name)
                {
                    client = c;
                }
            }
            return client;
        }

        /// <summary>
        /// Function that checks if session has a member with current name
        /// </summary>
        /// <param name="session">session which will be searched</param>
        /// <param name="name">name of a client</param>
        /// <returns></returns>
        private bool DoesSessionHaveMemberByName(Session session, string name)
        {
            foreach (Client c in session.Members.Keys)
            {
                if (c.Name == name)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Information about active users sent to them
        /// </summary>
        private void SetClientList()
        {
            // Info to all users
            List<string> clientsNames = new List<string>();
            foreach (Client c in clients.Keys)
                clientsNames.Add(c.Name);
            try
            {
                foreach (Client c in clients.Keys)
                {
                    lock (syncObj)
                    {
                        foreach (IWFRPCallback call in clients.Values)
                        {
                            call.SetClientList(clientsNames);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SetClientList error: " + ex.ToString());
            }
        }


        /// <summary>
        /// Add memebrs to a session - if they're aviable
        /// </summary>
        /// <param name="session">Session to which members are to be added</param>
        /// <param name="members">List of names of all members to be added</param>
        /// <returns>list of added members</returns>
        private Dictionary<Client,IWFRPCallback> SessionNewMembers(Session session, List<String> members)
        {
            Dictionary<Client, IWFRPCallback> newMembers = new Dictionary<Client, IWFRPCallback>();           
            foreach (string s in members)
            {
                // check if a member isn't already in a session
                bool isFree = true;
                foreach (Session ses in sessionList)
                {
                    foreach (Client c in ses.Members.Keys)
                    {
                        if (c.Name == s)
                            isFree = false;

                    }
                }

                KeyValuePair<Client, IWFRPCallback> member = SearchClientCallbackByName(s);
                if (!session.Members.ContainsKey(member.Key) && isFree)
                {
                    session.Members.Add(member.Key, member.Value);
                    newMembers.Add(member.Key, member.Value);
                }
            }

            return newMembers;
        }

        /// <summary>
        /// Control console print about current sessions status
        /// </summary>
        private void SessionsInfo()
        {
            Console.WriteLine("SESJE:");
            int i = 1;
            foreach (Session s in sessionList)
            {
                Console.WriteLine("sesja " + i);
                foreach (Client c in s.Members.Keys)
                    Console.WriteLine(c.Name);
                i++;
            }
        }


        #region IWFRP Members

        public bool Initialize()
        {
            IWFRPCallback callback = CurrentCallback;
            return true;
        }

        public void Disconnect(Client client)
        {
            this.clients.Remove(client);
            this.clientList.Remove(client);
            Console.WriteLine(client.Name + " has left.");

            // Info to user that had left
            ServerMessage msgClient = new ServerMessage();
            msgClient.IsStatusCorrect = true;
            msgClient.Type = ServerMessageTypeEnum.DisconnectInfoClient;
            msgClient.Content = "Disconnected succesfully.";
            IWFRPCallback callbackClient = CurrentCallback;
            try
            {
                callbackClient.GetServerMessageStatus(msgClient);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Disconnect error: " + ex.ToString());
            }

            // Info to all users
            SetClientList();

        }

        public void Register(Client client)
        {
            IWFRPCallback callback = CurrentCallback;

            DBConnector DataBase = new DBConnector();
            KeyValuePair<bool, string> response = DataBase.Register(client);
            ServerMessage msg = new ServerMessage();
            msg.Content = response.Value;
            msg.IsStatusCorrect = response.Key;
            msg.Type = ServerMessageTypeEnum.Register;
            try
            {
                callback.GetServerMessageStatus(msg);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Register error: " + ex.ToString());
            }

        }

        public void LogIn(Client client)
        {
            IWFRPCallback callback = CurrentCallback;

            DBConnector DataBase = new DBConnector();
            KeyValuePair<bool, string> response = new KeyValuePair<bool, string>();
            ServerMessage msg = new ServerMessage();   
     
            response = DataBase.LogIn(client);

            if (response.Key)
            {
                Identity accountID = new Identity();
                accountID.AccountID = response.Value;
                callback.GetIdentity(accountID);
                
                msg = new ServerMessage();
                msg.Content = "SERWER: POŁĄCZONO";
                msg.IsStatusCorrect = response.Key;
                msg.Type = ServerMessageTypeEnum.Login;
                try
                {
                    callback.GetServerMessageStatus(msg);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Login error: " + ex.ToString());
                }

                if (!clients.ContainsValue(CurrentCallback) &&
                !SearchClientsByName(client.Name))
                {
                    lock (syncObj)
                    {
                        clients.Add(client, CurrentCallback);
                        clientList.Add(client);

                    }
                    Console.WriteLine(client.Name + " has connected.");                  
                }
                // Info to all users
                SetClientList();             
            }
            else
            {
                msg = new ServerMessage();
                msg.Type = ServerMessageTypeEnum.Login;
                msg.Content = response.Value;
                msg.IsStatusCorrect = response.Key;
                try
                {
                    callback.GetServerMessageStatus(msg);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Login error: " + ex.ToString());
                }
                clients.Remove(client);                      
            }           
        }

        public void StartSession(Client client, List<string> members)
        {
            // Creating session
            Session session = new Session();
            session.MG = new KeyValuePair<Client,IWFRPCallback>(client, CurrentCallback);
            session.Members.Add(client, CurrentCallback);
            // Add all aviable members to the session
            SessionNewMembers(session, members);
            sessionList.Add(session);

            // Information to all session members
            ServerMessage msg = new ServerMessage();
            msg.Type = ServerMessageTypeEnum.StartSession;
            msg.IsStatusCorrect = true;
            msg.Content = "Session created";

            try
            {
                foreach (Client c in session.Members.Keys)
                {
                    lock (syncObj)
                    {
                        foreach (IWFRPCallback callback in session.Members.Values)
                        {
                            // MG
                            if (callback == session.MG.Value)
                            {
                                callback.GetServerMessageStatus(msg);
                                callback.SessionInitMGSettings(session);
                            }
                            // Other members
                            else
                            {
                                callback.JoinedToSession(msg);
                                callback.SessionInitSettings(session);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("StartSession error: " + ex.ToString());
            }
            SessionsInfo();
        }

        public void AddMemberToSession(Client client, List<string> members)
        {
            Session session = new Session();
            Dictionary<Client, IWFRPCallback> newMembers = new Dictionary<Client, IWFRPCallback>();

            // searching for a proper session (client = MG)
            foreach (Session s in sessionList)
            {
                if (s.MG.Key.Name == client.Name)
                    session = s;
            }
            // Add all aviable members to the session
            newMembers = SessionNewMembers(session, members);

            // If any new member has been added
            if (newMembers.Count > 0)
            {
                // sending a message to new members
                ServerMessage msg = new ServerMessage();
                msg.Type = ServerMessageTypeEnum.StartSession;
                msg.IsStatusCorrect = true;
                msg.Content = "Session created";
                try
                {
                    foreach (Client c in newMembers.Keys)
                    {
                        lock (syncObj)
                        {
                            foreach (IWFRPCallback callback in newMembers.Values)
                            {
                                callback.JoinedToSession(msg);
                                callback.SessionInitSettings(session);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("AddMemberToSession error: " + ex.ToString());
                }

                // Informing all about new members
                Message newMsg = new Message();
                newMsg.Content = "New members have come.";
                List<string> membersNames = new List<string>();
                foreach (Client c in session.Members.Keys)
                    membersNames.Add(c.Name);
                try
                {
                    foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
                    {
                        lock (syncObj)
                        {
                            c.Value.SetSessionList(membersNames, newMsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("AddMemberToSession error: " + ex.ToString());
                }

            }

            SessionsInfo();
        }

        public void GetAllClients()
        {
            // Info to all users
            List<string> clientsNames = new List<string>();
            foreach (Client c in clients.Keys)
                clientsNames.Add(c.Name);

            try
            {
                CurrentCallback.SetClientList(clientsNames);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("GetAllClients error: " + ex.ToString());
            }
        }

        public void EndSession(Client client)
        {
            foreach (Session session in sessionList)
            {
                if (session.Members.ContainsKey(client))
                {
                    session.Members.Remove(client);
                    Message msg = new Message();
                    msg.Content = client.Name + " has left.";

                    List<string> membersNames = new List<string>();
                    foreach (Client c in session.Members.Keys)
                        membersNames.Add(c.Name);

                    try
                    {
                        foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
                        {
                            lock (syncObj)
                                c.Value.SetSessionList(membersNames, msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("EndSession error: " + ex.ToString());
                    }
                }               
            }

            List<Session> emptySessions = sessionList.Where(s => s.Members.Count == 0).ToList();
            foreach (Session s in emptySessions)
                sessionList.Remove(s);

            SessionsInfo();
        }

        public void Send(Message msg)
        {
            Session session = new Session();
            // Searching for a proper session
            session = SearchSessionByClientName(msg.Sender);

            // sending a message to all session members
            try
            {
                foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
                {
                    lock (syncObj)
                        c.Value.Receive(msg);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Send error: " + ex.ToString());
            }

        }

        public void Whisper(Message msg)
        {
            Session session = new Session();
            // Searching for a proper session
            session = SearchSessionByClientName(msg.Sender);

            try
            {
                // sending a message to a receiver and sender
                foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
                {
                    if (c.Key.Name == msg.Receiver || c.Key.Name == msg.Sender)
                        c.Value.ReceiveWhisper(msg);
                }
                // sending a message to MG
                if (session.MG.Key.Name != msg.Receiver && session.MG.Key.Name != msg.Sender)
                    session.MG.Value.ReceiveWhisper(msg);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Whisper error: " + ex.ToString());
            }
        }

        public void UpdateParchment(Client client, FileMessage fMsg)
        {
            Session session = SearchSessionByClientName(client.Name);

            Message msg = new Message();
            msg.Sender = client.Name;
            msg.Content = "I'M SENDING A FILE... " + fMsg.FileName;

            try
            {
                // sending a parchment to all session members
                foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
                {
                    lock (syncObj)
                    {
                        c.Value.Receive(msg);
                        c.Value.ReceivePerchment(fMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("UpdateParchment error: " + ex.ToString());
            }
        }

        public void GetHero(string client)
        {
            Session session = SearchSessionByClientName(client);

            Hero hero = new Hero();
            hero.ClientName = SearchClientCallbackByName(client).Key.Name;
            // TO DO: more hero data (also in DataContract)
            try
            {
                IWFRPCallback callback = CurrentCallback;
                callback.ReceiveHero(hero);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("GetHero error: " + ex.ToString());
            }

        }

        public void GetHeroEyeColor(string race)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            HeroDetails_Eyes eyes = new HeroDetails_Eyes();

            try
            {
                eyes.HeroEyes = DataBase.GetHeroEyes(race);

                callback.ReciveHeroEyes(eyes);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
          
        }

        public void GetHeroOccupationByRace(string race)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            AllHeroOccupations occupations = new AllHeroOccupations();

            try
            {
                occupations.HeroOccupations = DataBase.GetOccupationsByRace(race);

                callback.ReciveHeroOccupationByRace(occupations);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
        }

        public void GetOccupationInfo(string occupation)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            OccupationInfo info = new OccupationInfo();

            try
            {
                info.Info = DataBase.GetOccupationInfo(occupation);

                callback.ReciveOccupationInfo(info);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
        }

        public void AddHeroBasicInfo(HeroBasicInfo info)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            ServerMessage msg = new ServerMessage();
            KeyValuePair<bool, string> dbResponse = new KeyValuePair<bool, string>();
            dbResponse = DataBase.CreateHeroPartBasicInfo(info);
            msg.IsStatusCorrect = dbResponse.Key;
            msg.Content = dbResponse.Value;
            callback.HeroRegistrationPartOne(msg);
        }

        public void GetSkillsAndAbilities(HeroRaceAndOccupation info)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            OccupationAndRaceInfo dbResponse = new OccupationAndRaceInfo();
            dbResponse = DataBase.GetSkillsAndAbilities(info);
            callback.ReciveSkillsAndAbilities(dbResponse);
        }

        public void AddHeroSkillsAndAbilities(HeroAbilitiesAndSkills AbsNSki)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            ServerMessage msg = new ServerMessage();
            string result = string.Empty;
            result = DataBase.AddSkillsAndAbilities(AbsNSki);
            if (result == "done")
            {
                msg.Content = "Added";
                msg.IsStatusCorrect = true;
                msg.Type = ServerMessageTypeEnum.AddedSnA;
                callback.GetServerMessageStatus(msg);
            }
            else
            {
                msg.Content = "Error";
                msg.IsStatusCorrect = false;
                msg.Type = ServerMessageTypeEnum.AddedSnA;
                callback.GetServerMessageStatus(msg);
            }
        }

        public void GetAbilityName(List<string> IDabilities)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            AbilityNames abNames = new AbilityNames();
            abNames = DataBase.GetAbilityName(IDabilities);
            callback.ReciveAbilityNames(abNames);
        }

        public void GetSkillName(List<string> IDskills)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            SkillNames skNames = new SkillNames();
            skNames = DataBase.GetSkillName(IDskills);
            callback.ReciveSkillNames(skNames);
        }

        public void GetFullAbilityInfo(string abName)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            FullAbilityInfo abInfo = new FullAbilityInfo();
            abInfo = DataBase.GetFullAbilityInfo(abName);
            callback.ReciveFullAbilityInfo(abInfo);

        }

        public void GetFullSkillInfo(string skName)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            FullSkillInfo skInfo = new FullSkillInfo();
            skInfo = DataBase.GetFullSkillInfo(skName);
            callback.ReciveFullSkillInfo(skInfo);
        }

        public void AddStartStats(StartStats strSta)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            ServerMessage msg = new ServerMessage();
            string result = string.Empty;
            result = DataBase.AddStartStats(strSta);
            if (result == "done")
            {
                msg.Content = "Added";
                msg.IsStatusCorrect = true;
                msg.Type = ServerMessageTypeEnum.AddedStartStats;
                callback.GetServerMessageStatus(msg);
            }
            else
            {
                msg.Content = "Error";
                msg.IsStatusCorrect = false;
                msg.Type = ServerMessageTypeEnum.AddedStartStats;
                callback.GetServerMessageStatus(msg);
            }

        }

        public void GetHeroChart(string Id_acc)
        {
            IWFRPCallback callback = CurrentCallback;
            DBConnector DataBase = new DBConnector();
            HeroFullChart chart = new HeroFullChart();
            chart = DataBase.GetHeroChart(Id_acc);
            callback.ReciveFullHeroChart(chart);
        }
        #endregion



        
    }

}
