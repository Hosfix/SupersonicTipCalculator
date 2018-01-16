using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace SupersonicTipCalculatorService.Entity
{
    [DelimitedRecord("|")]
    public class RateEntity
    {
        public int OrderID;

        public string CustomerID;

        [FieldConverter(ConverterKind.Date, "ddMMyyyy")]
        public DateTime OrderDate;

        [FieldConverter(ConverterKind.Decimal, ".")] // The decimal separator is .
        public decimal Freight;
    }
}
