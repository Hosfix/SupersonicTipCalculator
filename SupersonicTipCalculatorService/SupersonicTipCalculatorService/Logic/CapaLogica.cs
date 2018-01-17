using FileHelpers;
using Newtonsoft.Json;
using SupersonicTipCalculatorService.Entity;
using SupersonicTipCalculatorService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

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
            List<OrderEntity> ordersList = GetOrders().FindAll(o => o.Sku == sku && o.Currency == currency);
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
            decimal tip = 0;

            if (order.Currency == currency)
                tip = order.Amount * 0.5M;
            else
            {
                foreach (var rate in ratesList.FindAll(r => r.From == order.Currency))
                {
                    if (rate.To == currency)
                    {
                        tip = (order.Amount * rate.Rate) * 0.5M;
                    }
                }
            }

            return tip;
        }

        private static void PruebaAlgoritmo(List<RateEntity> ratesList, OrderEntity order, string currency)
        {
            decimal FIN = 0;
            //CALCULAR EL MEJOR CAMINO
            string orderCurrency = order.Currency;

            if (order.Currency == currency)
                FIN = 0;
            else
            {
                var listaRatesCurrencyFinal = ratesList.FindAll(r => r.To == currency);
                if (listaRatesCurrencyFinal.FindAll(r => r.From == order.Currency).Count == 1)
                {
                    FIN = 0;
                }
                else
                {
                    var siguiente = ratesList.FindAll(r => listaRatesCurrencyFinal.Contains(r));
                    if (siguiente.FindAll(r => r.From == order.Currency).Count == 1)
                    {
                        FIN = 0;
                    }
                    else
                    {
                        var siguiente2 = ratesList.FindAll(r => listaRatesCurrencyFinal.Contains(r));
                        if (siguiente2.FindAll(r => r.From == order.Currency).Count == 1)
                        {
                            FIN = 0;
                        }
                        else
                        {
                            //Y ASI AHI TENEMOS EL METODO
                        }
                    }
                }
            }

            //CALCULAR EL MEJOR CAMINO

                //METODO RECURSIVO
                //METODO RECURSIVO
        }
    }
}
