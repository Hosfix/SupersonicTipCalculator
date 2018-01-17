using FileHelpers;
using Newtonsoft.Json;
using SupersonicTipCalculatorService.Entity;
using SupersonicTipCalculatorService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SupersonicTipCalculatorService.Logic
{
    public static class CapaLogica
    {
        private static string _urlJsonRates = ConfigurationManager.AppSettings["Rates"];
        private static string _urlJsonOrders = ConfigurationManager.AppSettings["Transactions"];

        public static void GetRates()
        {
            CapaDAL.GetRates();
        }

        public static void GetPedido()
        {
            CapaDAL.GetPedido();
        }

        public static void DeserializeRates()
        {
            CapaDAL.InsertRates(Deserialize<RateEntity>(_urlJsonRates));
        }
        public static void DeserializeOrders()
        {
            CapaDAL.InsertOrders(Deserialize<OrderEntity>(_urlJsonOrders));
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
