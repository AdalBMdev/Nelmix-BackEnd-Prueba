using Microsoft.AspNetCore.Mvc;
using static Nelmix.DTO.BankAccountDTO;

namespace Nelmix.Interfaces
{
    public interface IBankAccountService
    {
        Task<bool> CreateBankAccount(CreateBankAccountRequestDto createBankAccount);
        Task<bool> DeleteBankAccount(int accountId, int userId);
        Task<bool> AddBankAccountBalance(int userId, int currencyId, decimal balance);

    }
}
