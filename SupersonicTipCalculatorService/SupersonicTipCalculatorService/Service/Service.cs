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
            var records = engine.ReadFile(@"Input.txt").ToList();

            foreach (var record in records)
            {
                Console.WriteLine(record.From);
                Console.WriteLine(record.To);
                Console.WriteLine(record.Rate);
            }

            return $"You entered: {value}";
        }
    }
}
