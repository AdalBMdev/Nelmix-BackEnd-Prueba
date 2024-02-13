using FluentValidation.Results;

namespace Nelmix.Interfaces
{
    public interface IValidationsManager
    {
        Task<ValidationResult> ValidateAsync<T>(T entity);
        Task<bool> ValidateBankAccountExistAsync(int userId);
        Task<bool> ValidateEmailExistAsync(string email);
        Task<bool> ValidateAdultExistAsync(string email);
        Task<bool> ValidateUserIsMinorExistAsync(string email);


    }
}
