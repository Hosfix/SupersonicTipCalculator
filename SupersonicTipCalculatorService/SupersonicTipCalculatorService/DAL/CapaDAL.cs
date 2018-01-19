using FileHelpers;
using Newtonsoft.Json;
using NLog;
using SupersonicTipCalculatorService.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace SupersonicTipCalculatorService.DAL
{
    public static class CapaDAL
    {
        private static FileHelperEngine<RateEntity> _engineRates { get; set; }
        private static FileHelperEngine<OrderEntity> _engineOrders { get; set; }
        private static Logger _logger = LogManager.GetLogger("CapaDAL");

        private static string _ratesFile = ConfigurationManager.AppSettings["RatsFile"];
        private static string _ordersFile = ConfigurationManager.AppSettings["OrdersFile"];
        
        public static List<RateEntity> GetRates()
        {
            List<RateEntity> result = new List<RateEntity>();

            try
            {
                _engineRates = new FileHelperEngine<RateEntity>();
                result = _engineRates.ReadFile(_ratesFile).ToList();
            }
            catch (FileHelpersException ex)
            {
                _logger.Error("Error al leer las conversiones almacenadas", ex);
            }

            return result;
        }

        public static List<OrderEntity> GetOrders()
        {
            List<OrderEntity> result = new List<OrderEntity>();

            try
            {
                _engineOrders = new FileHelperEngine<OrderEntity>();
                result = _engineOrders.ReadFile(_ordersFile).ToList();
            }
            catch (FileHelpersException ex)
            {
                _logger.Error("Error al leer los Pedidos almacenados", ex);
            }

            return result;
        }

        public static void InsertRates(string json)
        {
            try
            {
                var ratesList = Deserialize<RateEntity>(json).ToList();
                _engineRates.WriteFile(_ratesFile, ratesList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al leer los Rates almacenados");
            }
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

        public static string DownloadJson<T>(string Url)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadString(Url);
            }
        }
    }
}
