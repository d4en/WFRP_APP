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

        public void StartSession(Client client)
        {
            Session session = new Session();
            session.MG = new KeyValuePair<Client,IWFRPCallback>(client, CurrentCallback);
            session.Members.Add(client, CurrentCallback);
            sessionList.Add(session);

            IWFRPCallback callback = CurrentCallback;
            ServerMessage msg = new ServerMessage();
            msg.Type = ServerMessageTypeEnum.StartSession;
            msg.IsStatusCorrect = true;
            msg.Content = "Session created";
            callback.GetServerMessageStatus(msg);

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

        // if client is MG, removes session from a list - deleting whole session, removing other clients
        // if client is not MG, just removes him
   /*     public void EndSession(Client client)
        {
            // Info to all users
            foreach (Session s in sessionList)
            {
                if (s.MG.Key == client)
                {
                    // send callback to MG
                    s.MG.Value.EndUserSession();
                    // send callback to all session memebrs

                    // TO DO

                    // destroy session
                    sessionList.Remove(s);
                }
                else
                {
                    // inform other session members about clients leave

                    // TO DO
                }
            }
        }*/

        #endregion

        
    }

}
