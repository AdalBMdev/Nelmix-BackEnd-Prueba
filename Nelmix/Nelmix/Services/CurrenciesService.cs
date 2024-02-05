using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using System.Data;
using System.Data.SqlClient;

namespace Nelmix.Services
{
    /// <summary>
    /// Clase que gestiona operaciones relacionadas con conversiones de moneda y fichas.
    /// </summary>
    public class CurrenciesService 
    {

        private readonly CasinoContext _context;

        public CurrenciesService(CasinoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Convierte el saldo de una cuenta a dólares.
        /// </summary>
        /// <param name="accountId">Identificador de la cuenta.</param>
        /// <returns>El saldo convertido a dólares.</returns>
        public async Task<decimal> ConvertCurrencyDollars(int accountId)
        {
            try
            {
                var cuentaBancaria = await _context.CuentasBancarias.FindAsync(accountId);

                if (cuentaBancaria != null)
                {
                    var monedaId = cuentaBancaria.MonedaId;

                    var tasaConversion = await _context.TasasDeCambios
                        .Where(tc => tc.MonedaId == monedaId)
                        .Select(tc => tc.Tasa)
                        .FirstOrDefaultAsync();

                    if (tasaConversion > 0)
                    {
                        var nuevoSaldo = cuentaBancaria.Saldo * tasaConversion;

                        cuentaBancaria.MonedaId = await _context.Monedas
                            .Where(m => m.Nombre == "Dólar estadounidense")
                            .Select(m => m.MonedaId)
                            .FirstOrDefaultAsync();

                        cuentaBancaria.Saldo = nuevoSaldo;

                        await _context.SaveChangesAsync();

                        return (decimal)nuevoSaldo;
                    }
                }

                throw new Exception("Cuenta bancaria no encontrada o tasa de cambio no válida.");
            }
            catch (Exception ex)
            {
                // Manejar excepciones aquí
                throw new Exception("Error durante la conversión a dólares.", ex);
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
