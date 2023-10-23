using System.Data;
using System.Data.SqlClient;

namespace Nelmix.Services
{
    /// <summary>
    /// Clase que proporciona servicios relacionados con cuentas bancarias, como la creación, eliminación y adición de saldo a las cuentas bancarias de los usuarios.
    /// </summary>
    public class BankAccountService
    {
        /// <summary>
        /// Crea una nueva cuenta bancaria para un usuario con un saldo inicial y una moneda específica.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="currencyId">Identificador de la moneda.</param>
        /// <param name="balance">Saldo inicial de la cuenta.</param>
        /// <returns>True si la cuenta bancaria se crea con éxito, de lo contrario, False.</returns>
        public bool CreateBankAccount(int userId, int currencyId, decimal balance)
        {
            var chain = new Connection();

            using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
            {
                SqlCommand cmd = new SqlCommand("sp_CrearCuentaBancaria", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@moneda_id", currencyId);
                cmd.Parameters.AddWithValue("@saldo", balance);

                cn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        /// <summary>
        /// Elimina una cuenta bancaria de un usuario.
        /// </summary>
        /// <param name="cuentaId">Identificador de la cuenta bancaria.</param>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>True si la cuenta bancaria se elimina con éxito, de lo contrario, False.</returns>
        public bool DeleteBankAccount(int cuentaId, int userId)
        {
            var chain = new Connection();

            using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarCuentaBancaria", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cuenta_id", cuentaId);
                cmd.Parameters.AddWithValue("@user_id", userId);

                cn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
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
            var chain = new Connection();

            using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
            {

                SqlCommand cmd = new SqlCommand("AñadirSaldoACuentaBancaria", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuario_id", userId);
                cmd.Parameters.AddWithValue("@moneda_id", currencyId);
                cmd.Parameters.AddWithValue("@monto", balance);

                cn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;

            }
           
        }
    }
}


