using FluentValidation;
using Nelmix.DTO;
using Nelmix.Models;
using static Nelmix.DTO.BankAccountDTO;

namespace Nelmix.Validations
{
    public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountRequestDto>
    {
        public CreateBankAccountValidator()
        {
            RuleFor(account => account.UserId).NotEmpty();
            RuleFor(account => account.CurrencyId).NotEmpty();
        }
    }

    public class UpdateBankAccountValidator : AbstractValidator<AddBankAccountBalanceRequestDto>
    {
        public UpdateBankAccountValidator()
        {
            RuleFor(account => account.UserId).NotEmpty();
            RuleFor(account => account.CurrencyId).NotEmpty();
            RuleFor(account => account.Saldo).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }

}
