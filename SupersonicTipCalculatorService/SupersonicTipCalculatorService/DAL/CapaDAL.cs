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
    public class CapaDAL : ICapaDAL
    {
        private static FileHelperEngine<RateEntity> _engineRates { get; set; }
        private static FileHelperEngine<OrderEntity> _engineOrders { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static string _ratesFile = ConfigurationManager.AppSettings["RatesFile"];
        private static string _ordersFile = ConfigurationManager.AppSettings["OrdersFile"];
        
        public List<RateEntity> GetRates()
        {
            List<RateEntity> result = new List<RateEntity>();

            try
            {
                _engineRates = new FileHelperEngine<RateEntity>();
                result = _engineRates.ReadFile(_ratesFile).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al leer las conversiones almacenadas");
            }

            return result;
        }

        public List<OrderEntity> GetOrders()
        {
            List<OrderEntity> result = new List<OrderEntity>();

            try
            {
                _engineOrders = new FileHelperEngine<OrderEntity>();
                result = _engineOrders.ReadFile(_ordersFile).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al leer los pedidos almacenados");
            }

            return result;
        }

        public void InsertRates(string json)
        {
            try
            {
                var ratesList = Deserialize<RateEntity>(json).ToList();
                _engineRates.WriteFile(_ratesFile, ratesList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al leer las conversiones almacenados");
            }
        }

        public void InsertOrders(string json)
        {
            try
            {
                var ordersList = Deserialize<OrderEntity>(json).ToList();
                _engineOrders.WriteFile(_ordersFile, ordersList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al leer los pedidos almacenados");
            }
        }

        public List<T> Deserialize<T>(string json)
        {
            List<T> result = new List<T>();

            try
            {
                result = JsonConvert.DeserializeObject<List<T>>(json).ToList();
            }
            catch (JsonException ex)
            {
                _logger.Error(ex, "Error al deserializar la entidad: " + typeof(T).Name);
            }

            return result;
        }

        public string Serialize<T>(List<T> list)
        {
            string result = string.Empty;

            try
            {
                result = JsonConvert.SerializeObject(list, Formatting.Indented);
            }
            catch (JsonException ex)
            {
                _logger.Error(ex, "Error al serializar la entidad: " + typeof(T).Name);
            }

            return result;
        }

        public string DownloadJson(string Url)
        {
            string result = string.Empty;

            try
            {
                using (var webClient = new WebClient())
                {
                    result = webClient.DownloadString(Url);
                }
            }
            catch (WebException ex)
            {
                _logger.Error(ex, "Error al descargar el json de la Url: " + Url);
            }

            return result;
        }
    }
}
