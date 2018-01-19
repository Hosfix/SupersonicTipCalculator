using SupersonicTipCalculatorService.Logic;
using System;

namespace SupersonicTipCalculatorService.Service
{
    public class Service : IService
    {
        public string GetJsonRates()
        {
            return CapaLogica.GetJsonRates();
        }

        public string GetJsonOrders()
        {
            return CapaLogica.GetJsonOrders();
        }

        public Tuple<string, decimal> CalculateTip(string sku, string currency)
        {
            return CapaLogica.CalculateTip(sku, currency);
        }
    }
}
