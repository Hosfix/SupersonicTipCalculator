using System;
using System.ServiceModel;

namespace SupersonicTipCalculatorService.Logic
{
    [ServiceContract]
    public interface ICapaLogica
    {
        [OperationContract]
        string GetJsonRates();

        [OperationContract]
        string GetJsonOrders();

        [OperationContract]
        Tuple<string, decimal> CalculateTip(string sku, string currency);
    }
}
