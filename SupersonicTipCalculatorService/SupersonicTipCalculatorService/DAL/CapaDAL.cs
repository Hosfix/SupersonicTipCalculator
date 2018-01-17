using FileHelpers;
using SupersonicTipCalculatorService.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupersonicTipCalculatorService.DAL
{
    public static class CapaDAL
    {
        private static string _ratesFile = ConfigurationManager.AppSettings["RatesFile"];
        private static string _ordersFile = ConfigurationManager.AppSettings["OrdersFile"];

        public static void GetRates()
        {
            var engine = new FileHelperEngine<RateEntity>();
            var records = engine.ReadFile(_ratesFile).ToList();
        }

        public static void GetPedido()
        {
            var engine = new FileHelperEngine<OrderEntity>();
            var records = engine.ReadFile(_ordersFile).ToList();
        }

        public static void InsertRates(List<RateEntity> listRates)
        {
            var engine = new FileHelperEngine<RateEntity>();
            engine.WriteFile(_ratesFile, listRates);
        }

        public static void InsertOrders(List<OrderEntity> orderList)
        {
            var engine = new FileHelperEngine<OrderEntity>();
            engine.WriteFile(_ordersFile, orderList);
        }
    }
}
