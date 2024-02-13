using FluentValidation;
using static Nelmix.DTOs.CurrenciesDTO;

namespace Nelmix.Validations
{
    public class ConvertCurrencyDollarsValidator : AbstractValidator<ConvertCurrencyDollarsRequestDto>
    {
        public ConvertCurrencyDollarsValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty().GreaterThan(0);
        }
    }


}

