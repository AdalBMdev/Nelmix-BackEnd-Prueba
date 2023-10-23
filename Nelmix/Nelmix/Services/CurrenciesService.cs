using System.Data;
using System.Data.SqlClient;

namespace Nelmix.Services
{
    /// <summary>
    /// Clase que gestiona operaciones relacionadas con conversiones de moneda y fichas.
    /// </summary>
    public class CurrenciesService
    {
        /// <summary>
        /// Convierte el saldo de una cuenta a dólares.
        /// </summary>
        /// <param name="accountId">Identificador de la cuenta.</param>
        /// <returns>El saldo convertido a dólares.</returns>
        public decimal ConvertCurrencyDollars(int accountId)
        { 
            var chain = new Connection();

            using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("ConvertirMonedaADolares", cn);
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cuenta_id", accountId));

                SqlParameter newBalanceParameter = new SqlParameter("@nuevoSaldo", SqlDbType.Decimal);
                newBalanceParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(newBalanceParameter);

                cmd.ExecuteNonQuery();

                
                return (decimal)newBalanceParameter.Value;
                
                
            }

          
        }

        /// <summary>
        /// Compra fichas en dólares para un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="typeFileId">Identificador del tipo de ficha.</param>
        /// <param name="quantity">Cantidad de fichas a comprar.</param>
        /// <returns>Un mensaje indicando si la compra de fichas fue exitosa o un mensaje de error.</returns>
        public string BuyChipsInDollars(int userId, int typeFileId, int quantity)
        {
            var chain = new Connection();

            using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("ComprarFichasEnDolares", cn);
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@usuario_id", userId));
                cmd.Parameters.Add(new SqlParameter("@tipo_ficha_id", typeFileId));
                cmd.Parameters.Add(new SqlParameter("@cantidad", quantity));

                cmd.ExecuteNonQuery();
                return "Compra de fichas exitosa.";
                
            }

            return "Error al comprar fichas en dólares.";
        }

        /// <summary>
        /// Intercambia fichas por una moneda específica para un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="typeFileId">Identificador del tipo de ficha.</param>
        /// <param name="currencyDestination">Moneda de destino para la conversión.</param>
        /// <param name="quantityFichas">Cantidad de fichas a convertir.</param>
        /// <returns>Un mensaje indicando si la conversión de fichas fue exitosa o un mensaje de error.</returns>
        public string ExchangeChipsToCurrency(int userId, int typeFileId, string currencyDestination, int quantityFichas)
        {

            var chain = new Connection();

            using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("CambiarFichasAMonedaUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@usuario_id", userId));
                cmd.Parameters.Add(new SqlParameter("@tipo_ficha_id", typeFileId));
                cmd.Parameters.Add(new SqlParameter("@moneda_destino", currencyDestination));
                cmd.Parameters.Add(new SqlParameter("@cantidad_fichas_a_convertir", quantityFichas));

                // Ejecuta el procedimiento almacenado.
                cmd.ExecuteNonQuery();

                return "Cambio de fichas exitoso.";
                
            }

            return "Error al cambiar las fichas de moneda.";
        }
    }
}
