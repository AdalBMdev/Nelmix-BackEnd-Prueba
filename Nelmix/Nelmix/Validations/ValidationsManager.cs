using FluentValidation;
using FluentValidation.Results;
using Nelmix.Interfaces;
using Nelmix.Context;
using static Nelmix.DTOs.BankAccountDTO;
using Microsoft.EntityFrameworkCore;
using static Nelmix.DTOs.UserDTO;
using static Nelmix.DTOs.CurrenciesDTO;
using static Nelmix.DTOs.GameDTO;

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
            IValidator<BuyChipsInDollarsRequestDto> validatorBuyChipsInDollars,
            IValidator<ExchangeChipsToCurrencyRequestDto> validatorExchangeChipsToCurrency,
            IValidator<ManageUserGameRequestDto> validatorManageUserGame









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
                { typeof(BuyChipsInDollarsRequestDto), validatorBuyChipsInDollars },
                { typeof(ExchangeChipsToCurrencyRequestDto), validatorExchangeChipsToCurrency },
                { typeof(ManageUserGameRequestDto), validatorManageUserGame }
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

        public async Task<bool> ValidateChipsSuficientAsync(int userId, int typeFileId, int quantity)
        {
            int cantidadFichasDisponibles = await _context.Fichas
            .Where(f => f.UsuarioId == userId && f.TipoFichaId == typeFileId)
            .Select(f => f.CantidadDisponible)
            .FirstOrDefaultAsync();

            if (cantidadFichasDisponibles >= quantity)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> VerifyEligibilityToPlay(int userId)
        {
                var usuario = await _context.Usuarios
                    .Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync();


                    int edad = usuario.Edad;
                    int adultoAsignadoId = usuario.AdultoAsignadoId;

                    bool esElegible = edad >= 21 || adultoAsignadoId != 0;

                    return esElegible;    
        }

        public async Task<bool> VerifyAvailabilityFiches(int userId, int redChips, int yellowChips, int greenChips, int blackChips)
        {
            bool fichasSuficientes = true;

            if (redChips > 0)
            {
                int redChipsDisponibles = await _context.Fichas
                    .Where(f => f.UsuarioId == userId && f.TipoFichaId == 1)
                    .SumAsync(f => f.CantidadDisponible);

                fichasSuficientes &= (redChipsDisponibles >= redChips);
            }

            if (yellowChips > 0)
            {
                int yellowChipsDisponibles = await _context.Fichas
                    .Where(f => f.UsuarioId == userId && f.TipoFichaId == 2)
                    .SumAsync(f => f.CantidadDisponible);

                fichasSuficientes &= (yellowChipsDisponibles >= yellowChips);
            }

            if (greenChips > 0)
            {
                int greenChipsDisponibles = await _context.Fichas
                    .Where(f => f.UsuarioId == userId && f.TipoFichaId == 3)
                    .SumAsync(f => f.CantidadDisponible);

                fichasSuficientes &= (greenChipsDisponibles >= greenChips);
            }

            if (blackChips > 0)
            {
                int blackChipsDisponibles = await _context.Fichas
                    .Where(f => f.UsuarioId == userId && f.TipoFichaId == 4)
                    .SumAsync(f => f.CantidadDisponible);

                fichasSuficientes &= (blackChipsDisponibles >= blackChips);
            }

            return fichasSuficientes;
        }


        public async Task<bool> VerifyLoseLimit(int userId, int juegoId)
        {
                decimal limitePerdida = 10000.00M;

                var perdidas = await _context.ApuestasUsuarios
                    .Where(a => a.UsuarioId == userId && a.JuegoId == juegoId)
                    .SumAsync(a => (decimal?)a.CantidadPerdida) ?? 0;

                bool excedido = perdidas >= limitePerdida;

                return excedido;
        }
        public async Task<bool> VerifyGainLimit(int userId)
        {
                decimal limiteGanancia = 25000.00M;

                var ganancias = await _context.ApuestasUsuarios
                    .Where(a => a.UsuarioId == userId)
                    .SumAsync(a => (decimal?)a.CantidadGanada) ?? 0;

                bool limiteExcedido = ganancias >= limiteGanancia;

                return limiteExcedido;
        }

        public async Task<bool> VerifyPlay(ManageUserGameRequestDto request)
        {

            if (!await VerifyEligibilityToPlay(request.UserId))
            {
                return false;
            }

            if (!await VerifyAvailabilityFiches(request.UserId, request.RedChips, request.YellowChips, request.GreenChips, request.BlackChips))
            {
                return false;
            }

            if (await VerifyGainLimit(request.UserId))
            {
                return false;
            }

            if (await VerifyLoseLimit(request.UserId, request.GameId))
            {
                return false;
            }

            return true;

        }

    }
}
