using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SupersonicTipCalculatorService.Service
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        void GetRates();

        [OperationContract]
        void GetPedido();

        [OperationContract]
        void DeserializeRates();

        [OperationContract]
        void DeserializeOrders();
    }
}
