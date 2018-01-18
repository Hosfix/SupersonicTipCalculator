using SupersonicTipCalculatorService.Entity;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SupersonicTipCalculatorService.Service
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetJsonRates();

        [OperationContract]
        string GetJsonOrders();

        [OperationContract]
        Tuple<string, decimal> CalculateTip(string sku, string currency);
    }
}
