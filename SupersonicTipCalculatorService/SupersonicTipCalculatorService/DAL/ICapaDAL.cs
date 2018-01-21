using SupersonicTipCalculatorService.Entity;
using System.Collections.Generic;
using System.ServiceModel;

namespace SupersonicTipCalculatorService.DAL
{
    [ServiceContract]
    public interface ICapaDal
    {
        [OperationContract]
        List<RateEntity> GetRates();

        [OperationContract]
        List<OrderEntity> GetOrders();

        [OperationContract]
        void InsertRates(string json);

        [OperationContract]
        void InsertOrders(string json);

        [OperationContract]
        List<T> Deserialize<T>(string json);

        [OperationContract]
        string Serialize<T>(List<T> list);

        [OperationContract]
        string DownloadJson(string url);
    }
}
