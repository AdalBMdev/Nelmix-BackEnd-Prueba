using Microsoft.AspNetCore.Mvc;
using static Nelmix.DTOs.BankAccountDTO;

namespace Nelmix.Interfaces
{
    public interface IBankAccountService
    {
        Task CreateBankAccount(CreateBankAccountRequestDto createBankAccount);
        Task DeleteBankAccount(DeleteBankAccountRequestDto deleteBankAccountRequestDto);
        Task AddBankAccountBalance(AddBankAccountBalanceRequestDto addBankAccountBalanceRequestDto);

    }
}
