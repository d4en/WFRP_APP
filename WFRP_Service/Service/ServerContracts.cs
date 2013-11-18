using System;
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
        [OperationContract(IsOneWay = true)]
        void Disconnect(Client client);

        [OperationContract(IsOneWay = true)]
        void Register(Client client);

        [OperationContract(IsInitiating = true)]
        bool LogIn(Client client);
    }
}
