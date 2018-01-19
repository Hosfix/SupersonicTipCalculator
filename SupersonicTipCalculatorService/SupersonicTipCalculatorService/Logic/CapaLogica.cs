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
        private static string _urlJsonRates = ConfigurationManager.AppSettings["Rates"];
        private static string _urlJsonOrders = ConfigurationManager.AppSettings["Orders"];
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private ICapaDAL capaDal = new CapaDAL();

        public String GetJsonRates()
        {
            var json = capaDal.DownloadJson(_urlJsonRates);
            capaDal.InsertRates(json);
            return json;
        }

        public String GetJsonOrders()
        {
            string json = capaDal.DownloadJson(_urlJsonOrders);
            capaDal.InsertOrders(json);
            return json;
        }

        public Tuple<string, decimal> CalculateTip(string sku, string currency)
        {
            List<RateEntity> ratesList = GetRates();
            List<OrderEntity> ordersList = GetOrders().FindAll(o => o.Sku == sku);
            var json = capaDal.Serialize(ordersList);
            var totalTip = GetTip(ratesList, ordersList, currency);

            return Tuple.Create(json, totalTip);
        }

        private List<RateEntity> GetRates()
        {
            List<RateEntity> result = new List<RateEntity>();
            string json = GetJsonRates();

            if (!string.IsNullOrEmpty(json))
                result = capaDal.Deserialize<RateEntity>(json);
            else
                result = capaDal.GetRates();

            return result;
        }

        private List<OrderEntity> GetOrders()
        {
            List<OrderEntity> result = new List<OrderEntity>();
            string json = GetJsonOrders();

            if (!string.IsNullOrEmpty(json))
                result = capaDal.Deserialize<OrderEntity>(json);
            else
                result = capaDal.GetOrders();

            return result;
        }

        private Decimal GetTip(List<RateEntity> ratesList, List<OrderEntity> ordersList, string currency)
        {
            decimal totalAmount = 0M;

            foreach (var order in ordersList)
            {
                totalAmount += GetOrderAmount(ratesList, order, currency);
            }

            return totalAmount * 0.05M;
        }

        private Decimal GetOrderAmount(List<RateEntity> ratesList, OrderEntity order, string currency)
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
            }
            catch (Exception ex)
            {
                _logger.Error(ex, String.Format("Error al calcular el cambio de divisa From: {0}, To: {1}", from, to));
            }

            return results;
        }
    }
}
