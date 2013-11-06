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
        void Receive(Message msg);

    }
}
