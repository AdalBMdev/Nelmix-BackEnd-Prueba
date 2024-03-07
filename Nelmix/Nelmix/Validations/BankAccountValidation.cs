using FluentValidation;
using Nelmix.DTOs;
using Nelmix.Models;
using static Nelmix.DTOs.BankAccountDTO;

namespace Nelmix.Validations
{
    public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountRequestDto>
    {
        public CreateBankAccountValidator()
        {
            RuleFor(account => account.UserId).NotEmpty().GreaterThan(0);
            RuleFor(account => account.CurrencyId).NotEmpty().InclusiveBetween(1, 8);
        }
    }

    public class UpdateBankAccountValidator : AbstractValidator<AddBankAccountBalanceRequestDto>
    {
        public UpdateBankAccountValidator()
        {
            RuleFor(account => account.UserId).NotEmpty().GreaterThan(0);
            RuleFor(account => account.CurrencyId).NotEmpty().InclusiveBetween(1, 8);
            RuleFor(account => account.Saldo).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }

    public class DeleteBankAccountValidator : AbstractValidator<DeleteBankAccountRequestDto>
    {
        public DeleteBankAccountValidator()
        {
            RuleFor(account => account.UserId).NotEmpty().GreaterThan(0);
            RuleFor(account => account.BankAccountId).NotEmpty().GreaterThan(0);
        }
    }
}
