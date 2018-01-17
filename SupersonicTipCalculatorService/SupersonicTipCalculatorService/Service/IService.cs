using System.ServiceModel;

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
