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

    public class BuyChipsInDollarsDollarsValidator : AbstractValidator<BuyChipsInDollarsRequestDto>
    {
        public BuyChipsInDollarsDollarsValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.TypeFileId).NotEmpty().GreaterThan(0).InclusiveBetween(1, 4);
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        }
    }

}

