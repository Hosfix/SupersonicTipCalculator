using FileHelpers;
using Newtonsoft.Json;
using SupersonicTipCalculatorService.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SupersonicTipCalculatorService.DAL
{
    public static class CapaDAL
    {
        private static FileHelperEngine<RateEntity> _engineRates { get; set; }
        private static FileHelperEngine<OrderEntity> _engineOrders { get; set; }

        private static string _urlJsonRates = ConfigurationManager.AppSettings["Rates"];
        private static string _urlJsonOrders = ConfigurationManager.AppSettings["Transactions"];
        private static string _ratesFile = ConfigurationManager.AppSettings["RatesFile"];
        private static string _ordersFile = ConfigurationManager.AppSettings["OrdersFile"];

        public static List<RateEntity> GetRates()
        {
            var ratesList = Deserialize<RateEntity>(_urlJsonRates);
            _engineRates = new FileHelperEngine<RateEntity>();

            if (ratesList != null && ratesList.Count > 0)
                InsertRates(ratesList);

            return _engineRates.ReadFile(_ratesFile).ToList();
        }

        public static List<OrderEntity> GetOrders()
        {
            var ordersList = Deserialize<OrderEntity>(_urlJsonOrders);
            var _engineOrders = new FileHelperEngine<OrderEntity>();

            if (ordersList != null && ordersList.Count > 0)
                InsertOrders(ordersList);

            return _engineOrders.ReadFile(_ordersFile).ToList();
        }

        public static void InsertRates(List<RateEntity> listRates)
        {
            _engineRates.WriteFile(_ratesFile, listRates);
        }

        public static void InsertOrders(List<OrderEntity> orderList)
        {
            _engineOrders.WriteFile(_ordersFile, orderList);
        }


        private static List<T> Deserialize<T>(string Url)
        {
            using (var webClient = new WebClient())
            {
                string json = webClient.DownloadString(Url);
                return JsonConvert.DeserializeObject<List<T>>(json).ToList();
            }
        }

        private static void Serialize<T>(List<T> list, string Url)
        {
            using (var webClient = new WebClient())
            {
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                webClient.UploadString(Url, json);
            }
        }
    }
}
