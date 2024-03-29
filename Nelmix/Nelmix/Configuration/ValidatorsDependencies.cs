﻿using FluentValidation;
using Nelmix.DTOs;
using Nelmix.Validations;
using static Nelmix.DTOs.BankAccountDTO;
using static Nelmix.DTOs.CurrenciesDTO;
using static Nelmix.DTOs.GameDTO;
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
            services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterUserValidator>();
            services.AddScoped<IValidator<LoginUserRequestDto>, LoginUserValidator>();
            services.AddScoped<IValidator<ChangePasswordRequestDto>, ChangePasswordValidator>();
            services.AddScoped<IValidator<AssignAdultResponsableRequestDto>, AssignAdultValidator>();
            services.AddScoped<IValidator<DesactivateUserRequestDto>, DesactivateUserValidator>();
            //Currencies
            services.AddScoped<IValidator<ConvertCurrencyDollarsRequestDto>, ConvertCurrencyDollarsValidator>();
            services.AddScoped<IValidator<BuyChipsInDollarsRequestDto>, BuyChipsInDollarsDollarsValidator>();
            services.AddScoped<IValidator<ExchangeChipsToCurrencyRequestDto>, ExchangeChipsToCurrencyValidator>();
            //Game
            services.AddScoped<IValidator<ManageUserGameRequestDto>, ManageUserGameValidator>();











        }
    }
}
