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
            RuleFor(account => account.MonedaId).NotEmpty();
        }
    }

    public class UpdateBankAccountValidator : AbstractValidator<CuentasBancaria>
    {
        public UpdateBankAccountValidator()
        {
            RuleFor(account => account.Saldo).GreaterThanOrEqualTo(0);
        }
    }

}
