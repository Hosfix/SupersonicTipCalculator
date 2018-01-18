using FileHelpers;
using Newtonsoft.Json;
using SupersonicTipCalculatorService.Entity;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SupersonicTipCalculatorService.DAL
{
    public static class CapaDAL
    {
        private static FileHelperEngine<RateEntity> _engineRates { get; set; }
        private static FileHelperEngine<OrderEntity> _engineOrders { get; set; }

        private static string _ratesFile = ConfigurationManager.AppSettings["RatesFile"];
        private static string _ordersFile = ConfigurationManager.AppSettings["OrdersFile"];

        public static List<RateEntity> GetRates()
        {
            _engineRates = new FileHelperEngine<RateEntity>();
            return _engineRates.ReadFile(_ratesFile).ToList();
        }

        public static List<OrderEntity> GetOrders()
        {
            _engineOrders = new FileHelperEngine<OrderEntity>();
            return _engineOrders.ReadFile(_ordersFile).ToList();
        }

        public static void InsertRates(string json)
        {
            var ratesList = Deserialize<RateEntity>(json).ToList();
            _engineRates.WriteFile(_ratesFile, ratesList);
        }

        public static void InsertOrders(string json)
        {
            var ordersList = Deserialize<OrderEntity>(json).ToList();
            _engineOrders.WriteFile(_ordersFile, ordersList);
        }

        public static List<T> Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json).ToList();
        }

        public static string Serialize<T>(List<T> list)
        {
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }
    }
}
