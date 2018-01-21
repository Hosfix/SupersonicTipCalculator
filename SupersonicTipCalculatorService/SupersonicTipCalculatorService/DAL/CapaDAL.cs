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
    public class CapaDal : ICapaDal
    {
        private static FileHelperEngine<RateEntity> EngineRates { get; set; }
        private static FileHelperEngine<OrderEntity> EngineOrders { get; set; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly string RatesFile = ConfigurationManager.AppSettings["RatesFile"];
        public static readonly string OrdersFile = ConfigurationManager.AppSettings["OrdersFile"];
        
        public List<RateEntity> GetRates()
        {
            var result = new List<RateEntity>();

            try
            {
                EngineRates = new FileHelperEngine<RateEntity>();
                result = EngineRates.ReadFile(RatesFile).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al leer las conversiones almacenadas");
            }

            return result;
        }

        public List<OrderEntity> GetOrders()
        {
            List<OrderEntity> result = new List<OrderEntity>();

            try
            {
                EngineOrders = new FileHelperEngine<OrderEntity>();
                result = EngineOrders.ReadFile(OrdersFile).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al leer los pedidos almacenados");
            }

            return result;
        }

        public void InsertRates(string json)
        {
            try
            {
                var ratesList = Deserialize<RateEntity>(json).ToList();
                EngineRates.WriteFile(RatesFile, ratesList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al leer las conversiones almacenados");
            }
        }

        public void InsertOrders(string json)
        {
            try
            {
                var ordersList = Deserialize<OrderEntity>(json).ToList();
                EngineOrders.WriteFile(OrdersFile, ordersList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al leer los pedidos almacenados");
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
                Logger.Error(ex, "Error al deserializar la entidad: " + typeof(T).Name);
            }

            return result;
        }

        public string Serialize<T>(List<T> list)
        {
            var result = string.Empty;

            try
            {
                result = JsonConvert.SerializeObject(list, Formatting.Indented);
            }
            catch (JsonException ex)
            {
                Logger.Error(ex, "Error al serializar la entidad: " + typeof(T).Name);
            }

            return result;
        }

        public string DownloadJson(string url)
        {
            var result = string.Empty;

            try
            {
                using (var webClient = new WebClient())
                {
                    result = webClient.DownloadString(url);
                }
            }
            catch (WebException ex)
            {
                Logger.Error(ex, "Error al descargar el json de la Url: " + url);
            }

            return result;
        }
    }
}
