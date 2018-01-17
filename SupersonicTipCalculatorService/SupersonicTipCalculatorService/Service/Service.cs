using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FileHelpers;
using Newtonsoft.Json;
using SupersonicTipCalculatorService.Entity;

namespace SupersonicTipCalculatorService.Service
{
    public class Service : IService
    {
        private const string UrlJsonRates = "http://quiet-stone-2094.herokuapp.com/rates.json";
        private const string UrlJsonOrders = "http://quiet-stone-2094.herokuapp.com/transactions.json";
        private const string RatesFile = "Rates.txt";
        private const string OrdersFile = "Orders.txt";

        public void GetRates()
        {
            var engine = new FileHelperEngine<RateEntity>();
            var records = engine.ReadFile(RatesFile).ToList();
        }

        public void GetPedido()
        {
            var engine = new FileHelperEngine<OrderEntity>();
            var records = engine.ReadFile(OrdersFile).ToList();
        }

        public void DeserializeRates()
        {
            InsertRates(Deserialize<RateEntity>(UrlJsonRates));
        }

        public void DeserializeOrders()
        {
            InsertOrders(Deserialize<OrderEntity>(UrlJsonOrders));
        }

        private List<T> Deserialize<T>(string Url)
        {
            string json = new WebClient().DownloadString(Url);
            return JsonConvert.DeserializeObject<List<T>>(json).ToList();
        }

        private void Serialize<T>(List<T> list, string Url)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            new WebClient().UploadString(Url, json);
        }
        private void InsertRates(List<RateEntity> listRates)
        {
            var engine = new FileHelperEngine<RateEntity>();
            engine.WriteFile(RatesFile, listRates);
        }

        private void InsertOrders(List<OrderEntity> orderList)
        {
            var engine = new FileHelperEngine<OrderEntity>();
            engine.WriteFile(OrdersFile, orderList);
        }
    }
}
