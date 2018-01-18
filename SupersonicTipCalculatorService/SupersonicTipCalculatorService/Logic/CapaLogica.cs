using Newtonsoft.Json;
using SupersonicTipCalculatorService.DAL;
using SupersonicTipCalculatorService.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace SupersonicTipCalculatorService.Logic
{
    public static class CapaLogica
    {
        private static string _urlJsonRates = ConfigurationManager.AppSettings["Rates"];
        private static string _urlJsonOrders = ConfigurationManager.AppSettings["Orders"];

        public static string GetJsonRates()
        {
            var json = DownloadJson<RateEntity>(_urlJsonRates);
            CapaDAL.InsertRates(json);
            return json;
        }

        public static string GetJsonOrders()
        {
            string json = DownloadJson<OrderEntity>(_urlJsonOrders);
            CapaDAL.InsertOrders(json);
            return json;
        }

        public static Tuple<string, decimal> CalculateTip(string sku, string currency)
        {
            List<RateEntity> ratesList = GetRates();
            List<OrderEntity> ordersList = GetOrders().FindAll(o => o.Sku == sku);
            var json = CapaDAL.Serialize(ordersList);
            var totalTip = GetTip(ratesList, ordersList, currency);

            return Tuple.Create(json, totalTip);
        }

        private static List<RateEntity> GetRates()
        {
            string json = GetJsonRates();
            return CapaDAL.GetRates();
        }

        private static List<OrderEntity> GetOrders()
        {
            string json = DownloadJson<OrderEntity>(_urlJsonOrders);
            return CapaDAL.GetOrders();
        }

        private static decimal GetTip(List<RateEntity> ratesList, List<OrderEntity> ordersList, string currency)
        {
            decimal totalAmount = 0M;

            foreach (var order in ordersList)
            {
                totalAmount += GetOrderAmount(ratesList, order, currency);
            }

            return totalAmount * 0.05M;
        }

        private static decimal GetOrderAmount(List<RateEntity> ratesList, OrderEntity order, string currency)
        {
            var amount = order.Amount;
            var betterWay = new List<RateEntity>();
            if (order.Currency != currency)
                betterWay = FindPossibleChangeRecursive(order.Currency, currency, ratesList).OrderBy(r => r.Count).First();

            foreach (var rate in betterWay)
            {
                amount = amount * rate.Rate;
            }

            return amount;
        }

        private static List<List<RateEntity>> FindPossibleChangeRecursive(string from, string to, List<RateEntity> ratesList)
        {
            var results = new List<List<RateEntity>>();

            List<RateEntity> listWithFrom = ratesList.FindAll(f => f.From == from);
            List<RateEntity> listWithFromAndTo = listWithFrom.FindAll(ft => ft.To == to);
            if (listWithFromAndTo.Count > 0)
            {
                listWithFrom.RemoveAll(listWithFromAndTo.Contains);
                results.AddRange(listWithFromAndTo.Select(lft => new List<RateEntity> { lft }));
            }

            List<RateEntity> newListToSee = ratesList.FindAll(s => !listWithFrom.Contains(s));
            foreach (RateEntity possibleRate in listWithFrom)
            {
                List<List<RateEntity>> conversions = FindPossibleChangeRecursive(possibleRate.To, to, newListToSee);
                RateEntity rate = possibleRate;
                conversions.ForEach(result => result.Insert(0, rate));
                results.AddRange(conversions);
            }
            return results;
        }

        private static string DownloadJson<T>(string Url)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadString(Url);
            }
        }
    }
}
