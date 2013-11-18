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

        public void Disconnect(Client client)
        {
            this.clients.Remove(client);
            this.clientList.Remove(client);
            Console.WriteLine(client.Name + " has left.");
            foreach (Client c in clients.Keys)
            {
                lock (syncObj)
                {
                    foreach (IWFRPCallback callback in clients.Values)
                    {
                        Message msg = new Message();
                        msg.Sender = "Server";
                        msg.Content = client.Name + " has left.";
                        callback.GetStatus(msg);
                    }
                } 
            }

        }

        public void Register(Client client)
        {
            IWFRPCallback callback = CurrentCallback;

            DBConnector DataBase = new DBConnector();
            string response = string.Empty;

            response = DataBase.Register(client);
            Message msg = new Message();
            msg.Sender = "SERVER";
            msg.Content = response;
            callback.GetStatus(msg);

        }

        public bool LogIn(Client client)
        {
            IWFRPCallback callback = CurrentCallback;

            DBConnector DataBase = new DBConnector();
            string response = string.Empty;

            Message msg = new Message();
            

            response = DataBase.LogIn(client);

            if (Int32.Parse(response) != 0)
            {
                Identity accountID = new Identity();
                accountID.AccountID = response;
                callback.GetIdentity(accountID);
                
                msg = new Message();
                msg.Sender = "Server";
                msg.Content = "SERWER: POŁĄCZONO";
                callback.GetStatus(msg);

                if (!clients.ContainsValue(CurrentCallback) &&
                !SearchClientsByName(client.Name))
                {
                    lock (syncObj)
                    {
                        clients.Add(client, CurrentCallback);
                        clientList.Add(client);

                    }

                    Console.WriteLine(client.Name + " has connected.");
                    return true;
                }
                return true;
            }
            else
            {
                msg = new Message();
                msg.Sender = "SERVER";
                msg.Content = "Wrong Credentials";
                clients.Remove(client);
                callback.GetStatus(msg);
                return false;
            }
            
        }
        #endregion
    }

}
