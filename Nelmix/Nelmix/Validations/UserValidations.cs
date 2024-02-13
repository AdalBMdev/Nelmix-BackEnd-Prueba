using FluentValidation;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Validations
{
    public class RegisterUsertValidator : AbstractValidator<RegisterUserRequestDto>
    {
        public RegisterUsertValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 100);
            RuleFor(x => x.Age).InclusiveBetween(16, 150);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }

    public class LoginUsertValidator : AbstractValidator<LoginUserRequestDto>
    {
        public LoginUsertValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}
