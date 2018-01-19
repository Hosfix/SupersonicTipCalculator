using FileHelpers;

namespace SupersonicTipCalculatorService.Entity
{
    [DelimitedRecord("|")]
    public class RateEntity
    {
        public string From { get; set; }

        public string To { get; set; }

        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal Rate { get; set; }
    }
}
