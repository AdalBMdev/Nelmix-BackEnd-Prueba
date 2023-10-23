using System.Data;
using System.Data.SqlClient;
using Nelmix.Models;

namespace Nelmix.Services
{
    /// <summary>
    /// Clase que gestiona operaciones relacionadas con el estado financiero de los usuarios, como la obtención de saldos, ganancias, pérdidas y fichas.
    /// </summary>
    public class FinanceService
    {
        /// <summary>
        /// Obtiene el estado financiero de un usuario, incluyendo su saldo actual, ganancias, pérdidas y cantidad de fichas.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="balance">Saldo actual del usuario.</param>
        /// <param name="earnings">Ganancias totales del usuario.</param>
        /// <param name="losses">Pérdidas totales del usuario.</param>
        /// <param name="chips">Cantidad de fichas del usuario.</param>
        /// <returns>True si se obtiene el estado financiero con éxito, de lo contrario, False.</returns>
        public bool GetFinancialStatusUser(int userId, out decimal balance, out decimal earnings, out decimal losses, out int chips)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    SqlCommand cmd = new SqlCommand("ObtenerEstadoFinancieroUsuario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario_id", userId);

                    cmd.Parameters.Add("@saldo_actual", SqlDbType.Decimal, 10).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ganancias", SqlDbType.Decimal, 10).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@perdidas", SqlDbType.Decimal, 10).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@fichas", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    balance = (decimal)cmd.Parameters["@saldo_actual"].Value;
                    earnings = (decimal)cmd.Parameters["@ganancias"].Value;
                    losses = (decimal)cmd.Parameters["@perdidas"].Value;
                    chips = (int)cmd.Parameters["@fichas"].Value;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el estado financiero del usuario: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene las ganancias y pérdidas por juego para un usuario específico.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>Una lista de objetos de tipo GananciasYPérdidasPorJuego que contiene información sobre ganancias y pérdidas por juego.</returns>
        public List<GananciasYPérdidasPorJuego> GetProfitAndLossFromGaming(int userId)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    SqlCommand cmd = new SqlCommand("ObtenerGananciasYPérdidasPorJuego", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario_id", userId);

                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var results = new List<GananciasYPérdidasPorJuego>();

                        while (reader.Read())
                        {
                            var result = new GananciasYPérdidasPorJuego
                            {
                                NombreJuego = reader["nombre_juego"].ToString(),
                                Ganancias = (decimal)reader["ganancias"],
                                Pérdidas = (decimal)reader["pérdidas"]
                            };

                            results.Add(result);
                        }

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ganancias y pérdidas por juego: " + ex.Message);
            }
        }

    }
}
