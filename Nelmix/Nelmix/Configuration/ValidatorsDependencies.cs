using FluentValidation;
using Nelmix.DTO;
using Nelmix.Validations;
using static Nelmix.DTO.BankAccountDTO;

namespace Nelmix.Configuration
{
    public static class ValidatorsDependencies
    {
        public static void ValidatorsInjections(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateBankAccountRequestDto>, CreateBankAccountValidator>();
            services.AddScoped<IValidator<AddBankAccountBalanceRequestDto>, UpdateBankAccountValidator>();
            services.AddScoped<IValidator<DeleteBankAccountRequestDto>, DeleteBankAccountValidator>();

        }
    }
}
