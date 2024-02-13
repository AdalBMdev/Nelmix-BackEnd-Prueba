using Microsoft.AspNetCore.Mvc;
using static Nelmix.DTOs.BankAccountDTO;

namespace Nelmix.Interfaces
{
    public interface IBankAccountService
    {
        Task<bool> CreateBankAccount(CreateBankAccountRequestDto createBankAccount);
        Task<bool> DeleteBankAccount(DeleteBankAccountRequestDto deleteBankAccountRequestDto);
        Task<bool> AddBankAccountBalance(AddBankAccountBalanceRequestDto addBankAccountBalanceRequestDto);

    }
}
