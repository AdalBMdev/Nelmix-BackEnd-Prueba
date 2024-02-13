using FluentValidation;
using Nelmix.DTOs;
using Nelmix.Validations;
using static Nelmix.DTOs.BankAccountDTO;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Configuration
{
    public static class ValidatorsDependencies
    {
        public static void ValidatorsInjections(this IServiceCollection services)
        {
            //Bank Account
            services.AddScoped<IValidator<CreateBankAccountRequestDto>, CreateBankAccountValidator>();
            services.AddScoped<IValidator<AddBankAccountBalanceRequestDto>, UpdateBankAccountValidator>();
            services.AddScoped<IValidator<DeleteBankAccountRequestDto>, DeleteBankAccountValidator>();
            //User
            services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterUsertValidator>();


        }
    }
}
