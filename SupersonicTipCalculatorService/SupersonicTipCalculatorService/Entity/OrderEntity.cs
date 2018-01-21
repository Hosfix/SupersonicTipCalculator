using FileHelpers;

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
        public decimal Tip => Amount * 0.05M;
    }
}
