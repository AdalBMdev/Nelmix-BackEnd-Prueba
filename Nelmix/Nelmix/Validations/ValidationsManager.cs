using FluentValidation;
using FluentValidation.Results;
using Nelmix.Interfaces;
using Nelmix.Context;
using static Nelmix.DTOs.BankAccountDTO;
using Microsoft.EntityFrameworkCore;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Validations
{
    public class ValidationsManager : IValidationsManager
    {

        private readonly CasinoContext _context;
        private readonly Dictionary<Type, IValidator> _dictionary;

        public ValidationsManager(
            CasinoContext context, 
            IValidator<CreateBankAccountRequestDto> validatorBankAccountCreate,
            IValidator<AddBankAccountBalanceRequestDto> validatorBankAccountSaldoUpdate,
            IValidator<DeleteBankAccountRequestDto> validatorBankAccountDelete,
            IValidator<RegisterUserRequestDto> validatorRegisterUser,
            IValidator<LoginUserRequestDto> validatorLoginUser




            )
        {
            _context = context;
            _dictionary = new()
            {
                { typeof(CreateBankAccountRequestDto), validatorBankAccountCreate },
                { typeof(AddBankAccountBalanceRequestDto), validatorBankAccountSaldoUpdate },
                { typeof(DeleteBankAccountRequestDto), validatorBankAccountDelete },
                { typeof(RegisterUserRequestDto), validatorRegisterUser },
                { typeof(LoginUserRequestDto), validatorLoginUser }

            };
        }

        public async Task<ValidationResult> ValidateAsync<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }

            if (_dictionary.TryGetValue(typeof(T), out var value) && value is IValidator<T> validator)
            {
                var result = await validator.ValidateAsync(entity);
                return result;
            }

            throw new InvalidOperationException($"Validator not registered for type {typeof(T)}. Please register a validator for this type.");
        }

        public async Task<bool> ValidateBankAccountExistAsync(int userId)
        {
            var accountExists = await _context.CuentasBancarias.AnyAsync(account => account.UserId == userId);

            return accountExists;
        }

        public async Task<bool> ValidateEmailExistAsync(string email)
        {
            var emailExists = await _context.Usuarios.AnyAsync(account => account.Email == email);

            return emailExists;
        }

    }
}
