using FluentValidation;
using static Nelmix.DTOs.GameDTO;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Validations
{
    public class ManageUserGameValidator : AbstractValidator<ManageUserGameRequestDto>
    {
        public ManageUserGameValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.RedChips).GreaterThanOrEqualTo(0);
            RuleFor(x => x.YellowChips).GreaterThanOrEqualTo(0);
            RuleFor(x => x.GreenChips).GreaterThanOrEqualTo(0);
            RuleFor(x => x.BlackChips).GreaterThanOrEqualTo(0);
            RuleFor(x => x.GameId).NotEmpty().InclusiveBetween(1, 3);
        }
    }

}

