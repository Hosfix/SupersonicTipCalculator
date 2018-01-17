using SupersonicTipCalculatorService.Logic;

namespace SupersonicTipCalculatorService.Service
{
    public class Service : IService
    {
        public void GetRates()
        {
            CapaLogica.GetRates();
        }

        public void GetPedido()
        {
            CapaLogica.GetPedido();
        }

        public void DeserializeRates()
        {
            CapaLogica.DeserializeRates();
        }

        public void DeserializeOrders()
        {
            CapaLogica.DeserializeOrders();
        }
    }
}
