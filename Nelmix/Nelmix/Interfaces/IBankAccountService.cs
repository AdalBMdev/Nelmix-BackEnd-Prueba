namespace Nelmix.Interfaces
{
    public interface IBankAccountService
    {
        Task<bool> CreateBankAccount(int userId, int currencyId);
        Task<bool> DeleteBankAccount(int accountId, int userId);
        Task<bool> AddBankAccountBalance(int userId, int currencyId, decimal balance);

    }
}
