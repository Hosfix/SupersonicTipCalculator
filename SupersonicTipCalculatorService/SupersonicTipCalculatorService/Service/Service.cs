using SupersonicTipCalculatorService.Logic;
using System;

namespace SupersonicTipCalculatorService.Service
{
    public class Service : IService
    {
        private readonly ICapaLogica _capaLogica = new CapaLogica();

        public string GetJsonRates()
        {
            return _capaLogica.GetJsonRates();
        }

        public string GetJsonOrders()
        {
            return _capaLogica.GetJsonOrders();
        }

        public Tuple<string, decimal> CalculateTip(string sku, string currency)
        {
            return _capaLogica.CalculateTip(sku, currency);
        }
    }
}
