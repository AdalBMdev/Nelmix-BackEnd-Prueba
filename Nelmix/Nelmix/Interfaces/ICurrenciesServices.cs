namespace Nelmix.Interfaces
{
    public interface ICurrenciesServices
    {
        Task<decimal> ConvertCurrencyDollars(int accountId);

        Task<string> BuyChipsInDollars(int userId, int typeFileId, int quantity);

        Task<string> ExchangeChipsToCurrency(int userId, int typeFileId, int currencyDestinationId, int quantityFichas);

    }
}
