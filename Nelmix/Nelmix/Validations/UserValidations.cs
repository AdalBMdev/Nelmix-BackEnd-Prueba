using FluentValidation;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Validations
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequestDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 100);
            RuleFor(x => x.Age).InclusiveBetween(16, 150);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }

    public class LoginUserValidator : AbstractValidator<LoginUserRequestDto>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).NotEqual(x => x.Password);
        }       
    }

    public class AssignAdultValidator : AbstractValidator<AssignAdultResponsableRequestDto>
    {
        public AssignAdultValidator()
        {
            RuleFor(x => x.MailUserAdult).NotEmpty().EmailAddress();
            RuleFor(x => x.MailUserMinor).NotEmpty().EmailAddress();

        }
    }

    public class DesactivateUserValidator : AbstractValidator<DesactivateUserRequestDto>
    {
        public DesactivateUserValidator()
        {
           RuleFor(x => x.UserId).NotEmpty().GreaterThan(0);
        }
    }
}

