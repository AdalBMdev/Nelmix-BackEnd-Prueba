﻿using Microsoft.AspNetCore.Authorization;
using Nelmix.Interfaces;
using Nelmix.Services;
using Nelmix.Validations;


namespace Nelmix.Configuration
{
    public static class DepencyInjection
    {
        public static void GetDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrenciesServices, CurrenciesService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IFinanceService, FinanceService>();
            services.AddScoped<IValidationsManager, ValidationsManager>();
            services.ValidatorsInjections();
        }
    }
}
