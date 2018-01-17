using SupersonicTipCalculatorService.Entity;
using System.Collections.Generic;
using System.ServiceModel;

namespace SupersonicTipCalculatorService.Service
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        List<RateEntity> GetRates();

        [OperationContract]
        List<OrderEntity> Getorders();

        [OperationContract]
        decimal CalculateTip(string sku, string currency);
    }
}
