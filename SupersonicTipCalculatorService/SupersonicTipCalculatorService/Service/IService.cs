using System;
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
