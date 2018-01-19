using SupersonicTipCalculatorService.Logic;
using System;

namespace SupersonicTipCalculatorService.Service
{
    public class Service : IService
    {
        private ICapaLogica capaLogica = new CapaLogica();

        public string GetJsonRates()
        {
            return capaLogica.GetJsonRates();
        }

        public string GetJsonOrders()
        {
            return capaLogica.GetJsonOrders();
        }

        public Tuple<string, decimal> CalculateTip(string sku, string currency)
        {
            return capaLogica.CalculateTip(sku, currency);
        }
    }
}
