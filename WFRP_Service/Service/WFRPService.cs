using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Service
{

    [DataContract]
    public class Client
    {
        private string _name;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }

    [DataContract]
    public class Message
    {
        private string _sender;
        private string _content;

        [DataMember]
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        [DataMember]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
    }


    [ServiceContract(CallbackContract = typeof(IWFRPCallback), SessionMode = SessionMode.Required)]
    public interface IWFRP
    {
        [OperationContract(IsInitiating = true)]
        bool Connect(Client client);

        [OperationContract(IsOneWay = true)]
        void Disconnect(Client client);
    }

    public interface IWFRPCallback
    {

        [OperationContract(IsOneWay = true)]
        void Receive(Message msg);

    }

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
