using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;
using System.Data;
using System.Data.SqlClient;
using static Nelmix.DTOs.BankAccountDTO;

namespace Nelmix.Services
{
    public class BankAccountService : IBankAccountService
    {

        private readonly CasinoContext _context;

        public BankAccountService (CasinoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea una nueva cuenta bancaria para un usuario con un saldo inicial y una moneda específica.
        /// </summary>
        /// <param name="createBankAccount">DTO que contiene la información para la creación de la cuenta bancaria.</param>
        /// <returns>True si la cuenta bancaria se crea con éxito, de lo contrario, False.</returns>
        public async Task<bool> CreateBankAccount(CreateBankAccountRequestDto createBankAccount)
        {
            var newBankAccount = new CuentasBancaria
            {
                UserId = createBankAccount.UserId,
                MonedaId = createBankAccount.CurrencyId,
                Saldo = 0
            };

            _context.CuentasBancarias.Add(newBankAccount);
            await _context.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// Elimina una cuenta bancaria perteneciente a un usuario.
        /// </summary>
        /// <param name="deleteBankAccountRequestDto">DTO que contiene la información para la eliminacion de la cuenta bancaria.</param>
        /// <returns>True si la cuenta bancaria se elimina con éxito, de lo contrario, False.</returns>
        public async Task<bool> DeleteBankAccount(DeleteBankAccountRequestDto deleteBankAccountRequestDto)
        {
            var bankAccountToDelete = await _context.CuentasBancarias
                .FirstOrDefaultAsync(account => account.CuentaId == deleteBankAccountRequestDto.BankAccountId && account.UserId == deleteBankAccountRequestDto.UserId);

            if (bankAccountToDelete != null)
            {
                _context.CuentasBancarias.Remove(bankAccountToDelete);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }


        /// <summary>
        /// Añade saldo a una cuenta bancaria para un usuario.
        /// </summary>
        /// <param name="addBankAccountBalanceRequestDto">DTO que contiene la información para añadir saldo de la cuenta bancaria.</param>
        /// <returns>True si la cuenta bancaria se crea con éxito, de lo contrario, False.</returns>
        public async Task<bool> AddBankAccountBalance(AddBankAccountBalanceRequestDto addBankAccountBalanceRequestDto)
        {
            var bankAccountToUpdate = await _context.CuentasBancarias
                .FirstOrDefaultAsync(account => account.UserId == addBankAccountBalanceRequestDto.UserId && account.MonedaId == addBankAccountBalanceRequestDto.CurrencyId);

            if (bankAccountToUpdate != null)
            {
                bankAccountToUpdate.Saldo += addBankAccountBalanceRequestDto.Saldo;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

    }
}


