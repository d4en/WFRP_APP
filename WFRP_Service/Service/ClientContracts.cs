using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Service
{
    public interface IWFRPCallback
    {
        [OperationContract(IsOneWay = true)]
        void GetServerMessageStatus(ServerMessage msg);

        [OperationContract(IsOneWay = true)]
        void GetIdentity(Identity userID);

        [OperationContract(IsOneWay = true)]
        void SetClientList(List<string> clients);

        [OperationContract(IsOneWay = true)]
        void JoinedToSession(ServerMessage msg);

        [OperationContract(IsOneWay = true)]
        void SessionInitSettings(Session session);

        [OperationContract(IsOneWay = true)]
        void SetSessionList(List<string> clients, Message msg);

        [OperationContract(IsOneWay = true)]
        void Receive(Message msg);

        [OperationContract(IsOneWay = true)]
        void ReceiveWhisper(Message msg);

        [OperationContract(IsOneWay = true)]
        void SessionInitMGSettings(Session session);

        [OperationContract(IsOneWay = true)]
        void ReceivePerchment(FileMessage fMsg);

        [OperationContract(IsOneWay = true)]
        void ReceiveHero(Hero hero);

    }
}
