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

        private KeyValuePair<Client, IWFRPCallback> ClientCallbackByName(string name)
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

        private bool DoesSessionHaveMemberByName(Session session, string member)
        {
            foreach (Client c in session.Members.Keys)
            {
                if (c.Name == member)
                    return true;
            }

            return false;
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
            callbackClient.GetServerMessageStatus(msgClient);

            // Info to all users
            List<string> clientsNames = new List<string>();
            foreach (Client c in clients.Keys)
                clientsNames.Add(c.Name);
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

        public void Register(Client client)
        {
            IWFRPCallback callback = CurrentCallback;

            DBConnector DataBase = new DBConnector();
            KeyValuePair<bool, string> response = DataBase.Register(client);
            ServerMessage msg = new ServerMessage();
            msg.Content = response.Value;
            msg.IsStatusCorrect = response.Key;
            msg.Type = ServerMessageTypeEnum.Register;
            callback.GetServerMessageStatus(msg);

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
                callback.GetServerMessageStatus(msg);

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
                List<string> clientsNames = new List<string>();
                foreach (Client c in clients.Keys)
                    clientsNames.Add(c.Name);
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
            else
            {
                msg = new ServerMessage();
                msg.Type = ServerMessageTypeEnum.Login;
                msg.Content = response.Value;
                msg.IsStatusCorrect = response.Key;
                callback.GetServerMessageStatus(msg);  
                clients.Remove(client);                      
            }
            
        }

        public void StartSession(Client client, List<string> members)
        {
            // Creating session
            Session session = new Session();
            session.MG = new KeyValuePair<Client,IWFRPCallback>(client, CurrentCallback);
            session.Members.Add(client, CurrentCallback);
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

                KeyValuePair<Client, IWFRPCallback> member = ClientCallbackByName(s);
                if(!session.Members.ContainsKey(member.Key) && isFree)
                    session.Members.Add(member.Key, member.Value);
            }
            sessionList.Add(session);

            // Information to all session members
            ServerMessage msg = new ServerMessage();
            msg.Type = ServerMessageTypeEnum.StartSession;
            msg.IsStatusCorrect = true;
            msg.Content = "Session created";

            foreach (Client c in session.Members.Keys)
            {
                lock (syncObj)
                {
                    foreach (IWFRPCallback callback in session.Members.Values)
                    {
                        // MG
                        if (callback == session.MG.Value)
                            callback.GetServerMessageStatus(msg);
                        // Other members
                        else
                            callback.JoinedToSession(msg);

                        // All
                        callback.SessionInitSettings(session);
                    }
                }
            }

            Console.WriteLine("SESJE:");
            int i = 1;
            foreach (Session s in sessionList)
            {
                Console.WriteLine("sesja " + i);
                foreach(Client c in s.Members.Keys)
                    Console.WriteLine(c.Name);
                i++;
            }

        }

        public void GetAllClients()
        {
            // Info to all users
            List<string> clientsNames = new List<string>();
            foreach (Client c in clients.Keys)
            {
                clientsNames.Add(c.Name);
            }

            CurrentCallback.SetClientList(clientsNames);
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

                    foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
                    {
                        lock (syncObj)
                        {
                            c.Value.SetSessionList(membersNames, msg);
                        }
                    }
                }               
            }

            List<Session> emptySessions = sessionList.Where(s => s.Members.Count == 0).ToList();
            foreach (Session s in emptySessions)
                sessionList.Remove(s);
        }

        public void Send(Message msg)
        {
            Session session = new Session();
            // Searching for a proper session
            foreach (Session s in sessionList)
            {
                foreach (Client c in s.Members.Keys)
                {
                    if (c.Name == msg.Sender)
                    {
                        session = s;
                    }
                }
            }

            // sending a message to all session members
            foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
            {
                lock (syncObj)
                {
                    c.Value.Receive(msg);
                }
            }

        }

        public void Whisper(Message msg)
        {
            Session session = new Session();
            // Searching for a proper session
            foreach (Session s in sessionList)
            {
                foreach (Client c in s.Members.Keys)
                {
                    if (c.Name == msg.Sender)
                    {
                        session = s;
                    }
                }
            }

            // sending a message to a receiver and sender
            foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
            {
                if(c.Key.Name == msg.Receiver || c.Key.Name == msg.Sender)
                    c.Value.ReceiveWhisper(msg);
            }
            // sending a message to MG
            if (session.MG.Key.Name != msg.Receiver && session.MG.Key.Name != msg.Sender)
            {
                session.MG.Value.ReceiveWhisper(msg);
            }

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

                KeyValuePair<Client, IWFRPCallback> member = ClientCallbackByName(s);
                if (!session.Members.ContainsKey(member.Key) && isFree)
                {
                    session.Members.Add(member.Key, member.Value);
                    newMembers.Add(member.Key, member.Value);
                }
            }

            // If any new member has been added
            if (newMembers.Count > 0)
            {
                // sending a message to new members
                ServerMessage msg = new ServerMessage();
                msg.Type = ServerMessageTypeEnum.StartSession;
                msg.IsStatusCorrect = true;
                msg.Content = "Session created";
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

                // Informing all about new members
                Message newMsg = new Message();
                newMsg.Content = "New members have come.";
                List<string> membersNames = new List<string>();
                foreach (Client c in session.Members.Keys)
                    membersNames.Add(c.Name);
                foreach (KeyValuePair<Client, IWFRPCallback> c in session.Members)
                {
                    lock (syncObj)
                    {
                        c.Value.SetSessionList(membersNames, newMsg);
                    }
                }

            }

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

        #endregion

        
    }

}
