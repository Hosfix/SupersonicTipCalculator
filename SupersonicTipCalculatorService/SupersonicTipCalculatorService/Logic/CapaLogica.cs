using SupersonicTipCalculatorService.DAL;
using SupersonicTipCalculatorService.Entity;
using System.Collections.Generic;
using System.Linq;

namespace SupersonicTipCalculatorService.Logic
{
    public static class CapaLogica
    {
        public static List<RateEntity> GetRates()
        {
            return CapaDAL.GetRates();
        }

        public static List<OrderEntity> GetOrders()
        {
            return CapaDAL.GetOrders();
        }

        public static decimal CalculateTip(string sku, string currency)
        {
            List<RateEntity> ratesList = GetRates();
            List<OrderEntity> ordersList = GetOrders().FindAll(o => o.Sku == sku);
            return GetTip(ratesList, ordersList, currency);
        }

        private static decimal GetTip(List<RateEntity> ratesList, List<OrderEntity> ordersList, string currency)
        {
            decimal totalTip = 0M;

            foreach (var order in ordersList)
            {
                totalTip += GetTip(ratesList, order, currency);
            }

            return totalTip;
        }

        private static decimal GetTip(List<RateEntity> ratesList, OrderEntity order, string currency)
        {
            var amount = order.Amount;
            var camino = new List<RateEntity>();
            if (order.Currency != currency)
                camino = GetBetterWay(ratesList, order, currency);

            foreach (var rate in camino)
            {
                amount = amount * rate.Rate;
            }

            return amount * 0.05M;
        }

        private static List<RateEntity> GetBetterWay(List<RateEntity> ratesList, OrderEntity order, string currency)
        {
            return Rates(order.Currency, currency, ratesList).OrderBy(r => r.Count).First();
        }

        public static List<List<RateEntity>> Rates(string baseCode, string targetCode, List<RateEntity> toSee)
        {
            var results = new List<List<RateEntity>>();

            List<RateEntity> possible = toSee.FindAll(r => r.From == baseCode);
            List<RateEntity> hits = possible.FindAll(p => p.To == targetCode);
            if (hits.Count > 0)
            {
                possible.RemoveAll(hits.Contains);
                results.AddRange(hits.Select(hit => new List<RateEntity> { hit }));
            }

            List<RateEntity> newToSee = toSee.FindAll(item => !possible.Contains(item));
            foreach (RateEntity posRate in possible)
            {
                List<List<RateEntity>> otherConversions = Rates(posRate.To, targetCode, newToSee);
                RateEntity rate = posRate;
                otherConversions.ForEach(result => result.Insert(0, rate));
                results.AddRange(otherConversions);
            }
            return results;
        }
    }
}
