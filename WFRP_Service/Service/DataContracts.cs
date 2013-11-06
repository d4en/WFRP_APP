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

        public bool Equals(Client other)
        {
            if (other.Name == this.Name)
                return true;
            else
                return false;

        }

        public override bool Equals(Object other)
        {
            return Equals(other as Client);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
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
}
