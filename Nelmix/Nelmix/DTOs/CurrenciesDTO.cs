namespace Nelmix.DTOs
{
    public class CurrenciesDTO
    {
        public class ConvertCurrencyDollarsRequestDto
        {
            public int AccountId { get; set; }
        }

        public class BuyChipsInDollarsRequestDto
        {
            public int UserId { get; set; }
            public int TypeFileId { get; set; }
            public int Quantity { get; set; }
        }

        public class ExchangeChipsToCurrencyRequestDto : BuyChipsInDollarsRequestDto
        {
            public int CurrencyDestinationId { get; set; }
            
        }
    }
}
