using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupersonicTipCalculatorService.Entity
{
    [DelimitedRecord("|")]
    public class OrderEntity
    {
        public string Sku { get; set; }

        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal Tip { get { return Amount * 0.05M; } }
    }
}
