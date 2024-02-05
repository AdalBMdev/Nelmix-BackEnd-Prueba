using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;
using System.Data;
using System.Data.SqlClient;

namespace Nelmix.Services
{
    /// <summary>
    /// Clase que proporciona servicios relacionados con cuentas bancarias, como la creación, eliminación y adición de saldo a las cuentas bancarias de los usuarios.
    /// </summary>
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
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="currencyId">Identificador de la moneda.</param>
        /// <param name="balance">Saldo inicial de la cuenta.</param>
        /// <returns>True si la cuenta bancaria se crea con éxito, de lo contrario, False.</returns>
        public async Task<bool> CreateBankAccount(int userId, int currencyId)
        {
            var newBankAccount = new CuentasBancaria
            {
                UserId = userId,
                MonedaId = currencyId,
                Saldo = 0
            };

            _context.CuentasBancarias.Add(newBankAccount);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Elimina una cuenta bancaria de un usuario.
        /// </summary>
        /// <param name="cuentaId">Identificador de la cuenta bancaria.</param>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>True si la cuenta bancaria se elimina con éxito, de lo contrario, False.</returns>
        public async Task<bool> DeleteBankAccount(int cuentaId, int userId)
        {
            var bankAccountToDelete = await _context.CuentasBancarias
                .FirstOrDefaultAsync(account => account.CuentaId == cuentaId && account.UserId == userId);

            if (bankAccountToDelete != null)
            {
                _context.CuentasBancarias.Remove(bankAccountToDelete);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }


        /// <summary>
        /// Agrega saldo a una cuenta bancaria de un usuario en una moneda específica.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="currencyId">Identificador de la moneda.</param>
        /// <param name="balance">Saldo a agregar a la cuenta bancaria.</param>
        /// <returns>True si se agrega saldo con éxito, de lo contrario, False.</returns>
        public async Task<bool> AddBankAccountBalance(int userId, int currencyId, decimal balance)
        {
            var bankAccountToUpdate = await _context.CuentasBancarias
                .FirstOrDefaultAsync(account => account.UserId == userId && account.MonedaId == currencyId);

            if (bankAccountToUpdate != null)
            {
                bankAccountToUpdate.Saldo += balance;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

    }
}


