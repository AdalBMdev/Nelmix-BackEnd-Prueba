using Nelmix.Context;
using Nelmix.Models;
using System.Data;
using System.Data.SqlClient;

namespace Nelmix.Services
{
    /// <summary>
    /// Clase que proporciona servicios relacionados con cuentas bancarias, como la creación, eliminación y adición de saldo a las cuentas bancarias de los usuarios.
    /// </summary>
    public class BankAccountService
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
        public bool CreateBankAccount(int userId, int currencyId, decimal balance)
        {
                var newBankAccount = new CuentasBancaria
                {
                    UserId = userId,
                    MonedaId = currencyId,
                    Saldo = balance
                };

                _context.CuentasBancarias.Add(newBankAccount);
                _context.SaveChanges();

                return true;
        }

        /// <summary>
        /// Elimina una cuenta bancaria de un usuario.
        /// </summary>
        /// <param name="cuentaId">Identificador de la cuenta bancaria.</param>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>True si la cuenta bancaria se elimina con éxito, de lo contrario, False.</returns>
        public bool DeleteBankAccount(int cuentaId, int userId)
        {
            var bankAccountToDelete = _context.CuentasBancarias.FirstOrDefault(account => account.CuentaId == cuentaId && account.UserId == userId);

            if (bankAccountToDelete != null)
            {
                _context.CuentasBancarias.Remove(bankAccountToDelete);
                _context.SaveChanges();

                return true;
            }

            else return false;
        }


        /// <summary>
        /// Agrega saldo a una cuenta bancaria de un usuario en una moneda específica.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="currencyId">Identificador de la moneda.</param>
        /// <param name="balance">Saldo a agregar a la cuenta bancaria.</param>
        /// <returns>True si se agrega saldo con éxito, de lo contrario, False.</returns>
        public bool AddBankAccountBalance(int userId, int currencyId, decimal balance)
        {           
            var bankAccountToUpdate = _context.CuentasBancarias.FirstOrDefault(account => account.UserId == userId && account.MonedaId == currencyId);

            if (bankAccountToUpdate != null)
            {
                bankAccountToUpdate.Saldo += balance;
                _context.SaveChanges();

                return true;
            }

            else return false;
        }

    }
}


