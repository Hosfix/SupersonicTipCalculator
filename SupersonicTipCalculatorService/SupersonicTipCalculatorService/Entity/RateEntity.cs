using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace SupersonicTipCalculatorService.Entity
{
    [DelimitedRecord("|")]
    public class RateEntity : IDisposable
    {
        public string From { get; set; }

        public string To { get; set; }

        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal Rate { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
