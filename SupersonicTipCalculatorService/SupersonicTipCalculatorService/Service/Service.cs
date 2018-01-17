using SupersonicTipCalculatorService.Entity;
using SupersonicTipCalculatorService.Logic;
using System.Collections.Generic;

namespace SupersonicTipCalculatorService.Service
{
    public class Service : IService
    {
        public List<RateEntity> GetRates()
        {
            return CapaLogica.GetRates();
        }

        public List<OrderEntity> Getorders()
        {
            return CapaLogica.GetOrders();
        }

        public decimal CalculateTip(string sku, string currency)
        {
            return CapaLogica.CalculateTip(sku, currency);
        }
    }
}
