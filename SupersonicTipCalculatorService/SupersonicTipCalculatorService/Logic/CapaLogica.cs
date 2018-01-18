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
            var betterWay = new List<RateEntity>();
            if (order.Currency != currency)
                betterWay = FindPossibleChangeRecursive(order.Currency, currency, ratesList).OrderBy(r => r.Count).First();

            foreach (var rate in betterWay)
            {
                amount = amount * rate.Rate;
            }

            return amount * 0.05M;
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
    }
}
