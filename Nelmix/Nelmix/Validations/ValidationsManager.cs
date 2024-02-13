using FluentValidation;
using FluentValidation.Results;
using Nelmix.Interfaces;
using Nelmix.Context;
using static Nelmix.DTOs.BankAccountDTO;
using Microsoft.EntityFrameworkCore;
using static Nelmix.DTOs.UserDTO;
using static Nelmix.DTOs.CurrenciesDTO;

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
            IValidator<LoginUserRequestDto> validatorLoginUser,
            IValidator<ChangePasswordRequestDto> validatorChangePasswordUser,
            IValidator<AssignAdultResponsableRequestDto> validatorAssignAdultValidator,
            IValidator<DesactivateUserRequestDto> validatorDesactivateUser,
            IValidator<ConvertCurrencyDollarsRequestDto> validatorConvertCurrencyDollars,
            IValidator<BuyChipsInDollarsRequestDto> validatorBuyChipsInDollars








            )
        {
            _context = context;
            _dictionary = new()
            {
                { typeof(CreateBankAccountRequestDto), validatorBankAccountCreate },
                { typeof(AddBankAccountBalanceRequestDto), validatorBankAccountSaldoUpdate },
                { typeof(DeleteBankAccountRequestDto), validatorBankAccountDelete },
                { typeof(RegisterUserRequestDto), validatorRegisterUser },
                { typeof(LoginUserRequestDto), validatorLoginUser },
                { typeof(ChangePasswordRequestDto), validatorChangePasswordUser },
                { typeof(AssignAdultResponsableRequestDto), validatorAssignAdultValidator },
                { typeof(DesactivateUserRequestDto), validatorDesactivateUser },
                { typeof(ConvertCurrencyDollarsRequestDto), validatorConvertCurrencyDollars },
                { typeof(BuyChipsInDollarsRequestDto), validatorBuyChipsInDollars }

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
        public async Task<bool> ValidateUserBankAccountExistAsync(int userId)
        {
            var accountExists = await _context.CuentasBancarias.AnyAsync(account => account.UserId == userId);

            return accountExists;
        }

        public async Task<bool> ValidateBankAccountExistAsync(int accountId)
        {
            var accountExists = await _context.CuentasBancarias.AnyAsync(account => account.CuentaId == accountId);

            return accountExists;
        }

        public async Task<bool> ValidateEmailExistAsync(string email)
        {
            var emailExists = await _context.Usuarios.AnyAsync(account => account.Email == email);

            return emailExists;
        }

        public async Task<bool> ValidateAdultExistAsync(string email)
        {
            var adultExists = await _context.Usuarios.AnyAsync(account => account.Email == email && account.Edad > 21);

            return adultExists;
        }

        public async Task<bool> ValidateUserIsMinorExistAsync(string email)
        {
            var userMinorExists = await _context.Usuarios.AnyAsync(account => account.Email == email && account.Edad < 21);

            return userMinorExists;
        }

        public async Task<bool> ValidateUserExistAsync(int id)
        {
            var userExists = await _context.Usuarios.AnyAsync(account => account.UserId == id);

            return userExists;
        }

        public async Task<bool> ValidateBankAccountCurrencyIsDolarAndSufficientBalance(BuyChipsInDollarsRequestDto buyChipsInDollarsRequestDto)
        {
            var chipType = await _context.TiposDeFichas.FindAsync(buyChipsInDollarsRequestDto.TypeFileId);

            decimal chipValueInDollars = (decimal)chipType.Valor;
            decimal totalCostInDollars = chipValueInDollars * buyChipsInDollarsRequestDto.Quantity;

            var userDollarsAccount = await _context.CuentasBancarias
                .FirstOrDefaultAsync(account => account.UserId == buyChipsInDollarsRequestDto.UserId && account.MonedaId == 1);

            if (userDollarsAccount != null && userDollarsAccount.Saldo >= totalCostInDollars)
            {
                return true;
            }

            else return false;
        }

    }
}
