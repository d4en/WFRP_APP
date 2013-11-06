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

        public bool Connect(Client client)
        {
            if (!clients.ContainsValue(CurrentCallback) &&
                !SearchClientsByName(client.Name))
            {
                lock (syncObj)
                {
                    clients.Add(client, CurrentCallback);
                    clientList.Add(client);

                    foreach (Client key in clients.Keys)
                    {
                        IWFRPCallback callback = clients[key];
                        try
                        {

                            Message msg = new Message();
                            msg.Sender = "Server";
                            msg.Content = "SERWER: POŁĄCZONO";
                            callback.Receive(msg);
                        }
                        catch
                        {
                            clients.Remove(key);
                            return false;
                        }

                    }        

                }

                Console.WriteLine(client.Name + " has connected.");
                return true;
            }
            return false;
        }

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
                        callback.Receive(msg);
                    }
                } 
            }

        }
        

        #endregion
    }

}
