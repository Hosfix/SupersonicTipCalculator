using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FileHelpers;
using SupersonicTipCalculatorService.Entity;

namespace SupersonicTipCalculatorService.Service
{
    public class Service : IService
    {
        public string GetData(int value)
        {
            var engine = new FileHelperEngine<RateEntity>();
            var records = engine.ReadFile(@"Input.txt");

            foreach (var record in records)
            {
                Console.WriteLine(record.CustomerID);
                Console.WriteLine(record.OrderDate.ToString("dd/MM/yyyy"));
                Console.WriteLine(record.Freight);
            }
            return $"You entered: {value}";
        }
    }
}
