using FluentValidation.Results;
using static Nelmix.DTOs.CurrenciesDTO;

namespace Nelmix.Interfaces
{
    public interface IValidationsManager
    {
        Task<ValidationResult> ValidateAsync<T>(T entity);
        Task<bool> ValidateUserBankAccountExistAsync(int userId);
        Task<bool> ValidateBankAccountExistAsync(int userId);
        Task<bool> ValidateEmailExistAsync(string email);
        Task<bool> ValidateAdultExistAsync(string email);
        Task<bool> ValidateUserIsMinorExistAsync(string email);
        Task<bool> ValidateUserExistAsync(int id);
        Task<bool> ValidateBankAccountCurrencyIsDolarAndSufficientBalance(BuyChipsInDollarsRequestDto buyChipsInDollarsRequestDto);

        Task<bool> ValidateChipsSuficientAsync(int userId, int typeFileId, int quantity);


    }
}
