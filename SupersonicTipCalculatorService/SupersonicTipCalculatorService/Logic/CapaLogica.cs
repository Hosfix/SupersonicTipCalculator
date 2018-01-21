using NLog;
using SupersonicTipCalculatorService.DAL;
using SupersonicTipCalculatorService.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SupersonicTipCalculatorService.Logic
{
    public class CapaLogica : ICapaLogica
    {
        private static readonly string UrlJsonRates = ConfigurationManager.AppSettings["Rates"];
        private static readonly string UrlJsonOrders = ConfigurationManager.AppSettings["Orders"];
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICapaDal _capaDal = new CapaDal();

        public string GetJsonRates()
        {
            var json = _capaDal.DownloadJson(UrlJsonRates);
            _capaDal.InsertRates(json);
            return json;
        }

        public string GetJsonOrders()
        {
            string json = _capaDal.DownloadJson(UrlJsonOrders);
            _capaDal.InsertOrders(json);
            return json;
        }

        public Tuple<string, decimal> CalculateTip(string sku, string currency)
        {
            List<RateEntity> ratesList = GetRates();
            List<OrderEntity> ordersList = GetOrders().FindAll(o => o.Sku == sku);
            var json = _capaDal.Serialize(ordersList);
            var totalTip = GetTip(ratesList, ordersList, currency);

            return Tuple.Create(json, totalTip);
        }

        private List<RateEntity> GetRates()
        {
            var json = GetJsonRates();

            var result = !string.IsNullOrEmpty(json) ? _capaDal.Deserialize<RateEntity>(json) : _capaDal.GetRates();

            return result;
        }

        private List<OrderEntity> GetOrders()
        {
            var json = GetJsonOrders();

            var result = !string.IsNullOrEmpty(json) ? _capaDal.Deserialize<OrderEntity>(json) : _capaDal.GetOrders();

            return result;
        }

        private decimal GetTip(List<RateEntity> ratesList, List<OrderEntity> ordersList, string currency)
        {
            decimal totalAmount = 0M;

            foreach (var order in ordersList)
            {
                totalAmount += GetOrderAmount(ratesList, order, currency);
            }

            return totalAmount * 0.05M;
        }

        private decimal GetOrderAmount(List<RateEntity> ratesList, OrderEntity order, string currency)
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

        private List<List<RateEntity>> FindPossibleChangeRecursive(string from, string to, List<RateEntity> ratesList)
        {
            var results = new List<List<RateEntity>>();

            try
            {
                var listWithFrom = ratesList.FindAll(f => f.From == from);
                var listWithFromAndTo = listWithFrom.FindAll(ft => ft.To == to);
                if (listWithFromAndTo.Count > 0)
                {
                    listWithFrom.RemoveAll(listWithFromAndTo.Contains);
                    results.AddRange(listWithFromAndTo.Select(lft => new List<RateEntity> { lft }));
                }

                var newListToSee = ratesList.FindAll(s => !listWithFrom.Contains(s));
                foreach (RateEntity possibleRate in listWithFrom)
                {
                    var conversions = FindPossibleChangeRecursive(possibleRate.To, to, newListToSee);
                    var rate = possibleRate;
                    conversions.ForEach(result => result.Insert(0, rate));
                    results.AddRange(conversions);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al calcular el cambio de divisa From: {from}, To: {to}");
            }

            return results;
        }
    }
}
