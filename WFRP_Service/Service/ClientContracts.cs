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
        void GetStatus(Message msg);

        [OperationContract(IsOneWay = true)]
        void GetIdentity(Identity userID);

    }
}
