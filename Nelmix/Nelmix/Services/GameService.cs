﻿using System.Data;
using System.Data.SqlClient;


namespace Nelmix.Services
{
    /// <summary>
    /// Clase que gestiona operaciones relacionadas con juegos, como la verificación de elegibilidad para jugar, la disponibilidad de fichas y los límites de pérdida y ganancia.
    /// </summary>
    public class GameService
    {
        /// <summary>
        /// Verifica si un usuario es elegible para jugar.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        public bool VerifyEligibilityToPlay(int userId)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("VerificarElegibilidadParaJugar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuario_id", userId);

                        SqlParameter elegible = new SqlParameter("@esElegible", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(elegible);

                        cmd.ExecuteNonQuery();

                        bool esElegible = Convert.ToBoolean(elegible.Value);
                        return esElegible;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Aquí puedes registrar la excepción en un sistema de registro o lanzar una excepción personalizada
                // para manejarla en un nivel superior si es necesario.
                throw new Exception("Error al verificar elegibilidad para jugar", ex);
            }
        }

        /// <summary>
        /// Verifica si un usuario tiene suficientes fichas disponibles para jugar.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="redChips">Cantidad de fichas rojas.</param>
        /// <param name="yellowChips">Cantidad de fichas amarillas.</param>
        /// <param name="greenChips">Cantidad de fichas verdes.</param>
        /// <param name="blackChips">Cantidad de fichas negras.</param>
        /// <returns>True si el usuario tiene suficientes fichas, de lo contrario, False.</returns>
        public bool VerifyAvailabilityFiches(int userId, int redChips, int yellowChips, int greenChips, int blackChips)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("VerificarDisponibilidadFichas", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuario_id", userId);
                        cmd.Parameters.AddWithValue("@fichas_rojas", redChips);
                        cmd.Parameters.AddWithValue("@fichas_amarillas", yellowChips);
                        cmd.Parameters.AddWithValue("@fichas_verdes", greenChips);
                        cmd.Parameters.AddWithValue("@fichas_negras", blackChips);

                        SqlParameter chipsFound = new SqlParameter("@fichasEncontradas", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(chipsFound);

                        cmd.ExecuteNonQuery();

                        bool chipsSufficient = Convert.ToBoolean(chipsFound.Value);
                        return chipsSufficient;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al verificar disponibilidad de fichas", ex);
            }
        }

        /// <summary>
        /// Verifica si un usuario ha excedido el límite de pérdida en un juego específico.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="juegoId">Identificador del juego.</param>
        /// <returns>True si el usuario ha excedido el límite de pérdida, de lo contrario, False.</returns>
        public bool VerifyLoseLimit(int userId, int juegoId)
        {
            try
            {
                var chain = new Connection();
                double limitLost = 10000.00;

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("VerificarLimitePerdidaEnJuego", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuario_id", userId);
                        cmd.Parameters.AddWithValue("@juego_id", juegoId);
                        cmd.Parameters.AddWithValue("@limite_perdida", limitLost);

                        SqlParameter limitePerdidaExcedido = new SqlParameter("@excedido", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(limitePerdidaExcedido);

                        cmd.ExecuteNonQuery();

                        bool Limit = Convert.ToBoolean(limitePerdidaExcedido.Value);
                        return Limit;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al verificar límite de pérdida en el juego", ex);
            }
        }

        /// <summary>
        /// Verifica si un usuario ha excedido el límite de pérdida en un juego específico.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="juegoId">Identificador del juego.</param>
        /// <returns>True si el usuario ha excedido el límite de pérdida, de lo contrario, False.</returns>
        public bool VerifyGainLimit(int userId)
        {
            try
            {
                var chain = new Connection();
                double limitGain = 25000.00;

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("VerificarLimiteGanancia", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuario_id", userId);
                        cmd.Parameters.AddWithValue("@limite_ganancia", limitGain);

                        SqlParameter limitePerdidaExcedido = new SqlParameter("@excedido", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(limitePerdidaExcedido);

                        cmd.ExecuteNonQuery();

                        bool Limit = Convert.ToBoolean(limitePerdidaExcedido.Value);
                        return Limit;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al verificar límite de ganancia", ex);
            }
        }

        /// <summary>
        /// Gestiona el juego de un usuario, actualizando las fichas y el estado de victoria en la base de datos.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="redChips">Cantidad de fichas rojas.</param>
        /// <param name="yellowChips">Cantidad de fichas amarillas.</param>
        /// <param name="greenChips">Cantidad de fichas verdes.</param>
        /// <param name="blackChips">Cantidad de fichas negras.</param>
        /// <param name="victory">Indica si el usuario ha ganado el juego.</param>
        /// <param name="gameId">Identificador del juego.</param>
        /// <returns>True si la gestión del juego se realiza con éxito, de lo contrario, False.</returns>
        public bool ManageUserGame(int userId, int redChips, int yellowChips, int greenChips, int blackChips, bool victory, int gameId)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("GestionarJuegoUsuario", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuario_id", userId);
                        cmd.Parameters.AddWithValue("@fichas_rojas", redChips);
                        cmd.Parameters.AddWithValue("@fichas_amarillas", yellowChips);
                        cmd.Parameters.AddWithValue("@fichas_verdes", greenChips);
                        cmd.Parameters.AddWithValue("@fichas_negras", blackChips);
                        cmd.Parameters.AddWithValue("@victoria", victory);
                        cmd.Parameters.AddWithValue("@juego_id", gameId);

                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al gestionar el juego del usuario", ex);
            }
        }

        /// <summary>
        /// Verifica si un usuario cumple con los requisitos para jugar, incluyendo elegibilidad, disponibilidad de fichas y límites de ganancia y pérdida.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario.</param>
        /// <param name="redChips">Cantidad de fichas rojas.</param>
        /// <param name="yellowChips">Cantidad de fichas amarillas.</param>
        /// <param name="greenChips">Cantidad de fichas verdes.</param>
        /// <param name="blackChips">Cantidad de fichas negras.</param>
        /// <param name="juegoId">Identificador del juego.</param>
        /// <returns>True si el usuario cumple con los requisitos para jugar, de lo contrario, False.</returns>
        public bool VerifyPlay(int usuarioId, int redChips, int yellowChips, int greenChips, int blackChips, int juegoId)
        {
            try
            {
                if (!VerifyEligibilityToPlay(usuarioId))
                {
                    return false;
                }

                if (!VerifyAvailabilityFiches(usuarioId, redChips, yellowChips, greenChips, blackChips))
                {
                    return false;
                }

                if (VerifyGainLimit(usuarioId))
                {
                    return false;
                }

                if (VerifyLoseLimit(usuarioId, juegoId))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Simula un juego de Craps. Lanza dos dados y calcula la suma de los valores.
        /// Determina si el jugador ganó o perdió según las reglas del juego.
        /// </summary>
        /// <returns>
        /// Un valor booleano que indica si el jugador ganó (true), perdió (false) o si el resultado aún no se ha determinado (null).
        /// Un mensaje descriptivo del resultado del juego.
        /// </returns>
        public (bool?, string) PlayCraps()
        {
            Random random = new Random();
            int dice1 = random.Next(1, 7);
            int dice2 = random.Next(1, 7);
            int sumDice = dice1 + dice2;

            bool? victory = null;
            string resultMessage;

            if (sumDice == 7 || sumDice == 11)
            {
                victory = true;
                resultMessage = $"Ganaste en craps, resultado de dados {dice1} y {dice2} resultado {sumDice}";
            }
            else if (sumDice == 2 || sumDice == 3 || sumDice == 12)
            {
                victory = false;
                resultMessage = $"Perdiste en craps, resultado de dados {dice1} y {dice2} resultado {sumDice}";
            }
            else
            {
                resultMessage = $"Continua el juego, resultado de dados {dice1} y {dice2} resultado {sumDice} para ganar es necesario 7 u 11";
            }

            return (victory, resultMessage);
        }


        /// <summary>
        /// Simula un juego de tragaperras con tres símbolos. Genera aleatoriamente tres símbolos y determina si el jugador ha ganado si los tres símbolos son iguales.
        /// </summary>
        /// <returns>
        /// Un valor booleano que indica si el jugador ganó (true) o perdió (false).
        /// Un mensaje descriptivo del resultado del juego, incluyendo los símbolos obtenidos.
        /// </returns>
        public (bool, string) PlayTragaperras()
        {
            Random random = new Random();
            int symbol1 = random.Next(1, 4);
            int symbol2 = random.Next(1, 4);
            int symbol3 = random.Next(1, 4);

            if (symbol1 == symbol2 && symbol2 == symbol3)
            {
                // El jugador ganó
                return (true, $"Ganaste en tragaperras. ¡Has obtenido un premio! Resultado: {symbol1}-{symbol2}-{symbol3}");
            }
            else
            {
                // El jugador perdió
                return (false, $"No has ganado en tragaperras. ¡Inténtalo de nuevo! Resultado: {symbol1}-{symbol2}-{symbol3}");
            }
        }

        /// <summary>
        /// Simula un juego de Blackjack. Genera dos cartas aleatorias y calcula la suma de sus valores.
        /// Determina si el jugador ganó si la suma es igual a 21.
        /// </summary>
        /// <returns>
        /// Un valor booleano que indica si el jugador ganó (true) o perdió (false).
        /// Un mensaje descriptivo del resultado del juego, incluyendo la suma de cartas obtenida.
        /// </returns>
        public (bool, string) PlayBlackjack()
        {
            Random random = new Random();
            int card1 = random.Next(1, 11); // Valores de cartas de 1 a 10
            int card2 = random.Next(1, 11);

            int sumCards = card1 + card2;

            if (sumCards == 21)
            {
                // El jugador ganó
                return (true, "Ganaste en Blackjack. ¡Has obtenido 21!");
            }
            else
            {
                // El jugador perdió
                return (false, $"No has ganado en Blackjack. Tu suma de cartas es {sumCards}. ¡Inténtalo de nuevo!");
            }
        }
    }
}
