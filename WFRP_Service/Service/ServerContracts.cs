﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Service
{
    [ServiceContract(CallbackContract = typeof(IWFRPCallback), SessionMode = SessionMode.Required)]
    public interface IWFRP
    {
        [OperationContract(IsInitiating = true)]
        bool Initialize();

        [OperationContract(IsOneWay = true)]
        void Disconnect(Client client);

        [OperationContract(IsOneWay = true)]
        void Register(Client client);

        [OperationContract(IsOneWay = true)]
        void LogIn(Client client);

        [OperationContract(IsOneWay = true)]
        void StartSession(Client client, List<string> members);

        [OperationContract(IsOneWay = true)]
        void GetAllClients();

        [OperationContract(IsOneWay = true)]
        void EndSession(Client client);

        [OperationContract(IsOneWay = true)]
        void Send(Message msg);

        [OperationContract(IsOneWay = true)]
        void Whisper(Message msg, string receiver);

    }
}
