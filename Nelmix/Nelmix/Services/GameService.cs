using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using System.Data;
using System.Data.SqlClient;


namespace Nelmix.Services
{
    /// <summary>
    /// Clase que gestiona operaciones relacionadas con juegos, como la verificación de elegibilidad para jugar, la disponibilidad de fichas y los límites de pérdida y ganancia.
    /// </summary>
    public class GameService
    {

        private readonly CasinoContext _context;

        public GameService(CasinoContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Verifica si un usuario es elegible para jugar.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        public async Task<bool> VerifyEligibilityToPlay(int userId)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync();

                if (usuario != null)
                {
                    int edad = usuario.Edad;
                    int adultoAsignadoId = usuario.AdultoAsignadoId;

                    bool esElegible = edad >= 21 || adultoAsignadoId != 0;

                    return esElegible;
                }
                else
                {
                    throw new Exception("Usuario no encontrado");
                }
            }
            catch (Exception ex)
            {
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
        public async Task<bool> VerifyAvailabilityFiches(int userId, int redChips, int yellowChips, int greenChips, int blackChips)
        {
            try
            {
                var fichasDisponibles = await _context.Fichas
                    .Where(f => f.UsuarioId == userId &&
                                (f.TipoFichaId == 1 && redChips > 0 ||
                                 f.TipoFichaId == 2 && yellowChips > 0 ||
                                 f.TipoFichaId == 3 && greenChips > 0 ||
                                 f.TipoFichaId == 4 && blackChips > 0))
                    .GroupBy(f => f.TipoFichaId)
                    .Select(g => new { TipoFichaId = g.Key, CantidadDisponible = g.Sum(f => f.CantidadDisponible) })
                    .ToDictionaryAsync(f => f.TipoFichaId, f => f.CantidadDisponible);

                bool fichasSuficientes =
                    (!fichasDisponibles.ContainsKey(1) || (fichasDisponibles.ContainsKey(1) && redChips <= fichasDisponibles[1])) &&
                    (!fichasDisponibles.ContainsKey(2) || (fichasDisponibles.ContainsKey(2) && yellowChips <= fichasDisponibles[2])) &&
                    (!fichasDisponibles.ContainsKey(3) || (fichasDisponibles.ContainsKey(3) && greenChips <= fichasDisponibles[3])) &&
                    (!fichasDisponibles.ContainsKey(4) || (fichasDisponibles.ContainsKey(4) && blackChips <= fichasDisponibles[4]));


                return fichasSuficientes;
            }
            catch (Exception ex)
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
        public async Task<bool> VerifyLoseLimit(int userId, int juegoId)
        {
            try
            {
                decimal limitePerdida = 10000.00M;

                var perdidas = await _context.ApuestasUsuarios
                    .Where(a => a.UsuarioId == userId && a.JuegoId == juegoId)
                    .SumAsync(a => (decimal?)a.CantidadPerdida) ?? 0;

                bool excedido = perdidas >= limitePerdida;

                return excedido;
            }
            catch (Exception ex)
            {
                // Manejar y registrar el error aquí
                throw new Exception("Error al verificar límite de pérdida en el juego", ex);
            }
        }


        /// <summary>
        /// Verifica si un usuario ha excedido el límite de ganancia en un juego específico.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>True si el usuario ha excedido el límite de ganancia, de lo contrario, False.</returns>
        public async Task<bool> VerifyGainLimit(int userId)
        {
            try
            {
                decimal limiteGanancia = 25000.00M;

                var ganancias = await _context.ApuestasUsuarios
                    .Where(a => a.UsuarioId == userId)
                    .SumAsync(a => (decimal?)a.CantidadGanada) ?? 0;

                bool limiteExcedido = ganancias >= limiteGanancia;

                return limiteExcedido;
            }
            catch (Exception ex)
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
        public async Task<bool> VerifyPlay(int usuarioId, int redChips, int yellowChips, int greenChips, int blackChips, int juegoId)
        {
            try
            {
                if (!await VerifyEligibilityToPlay(usuarioId))
                {
                    return false;
                }

                if (!await VerifyAvailabilityFiches(usuarioId, redChips, yellowChips, greenChips, blackChips))
                {
                    return false;
                }

                if (!await VerifyGainLimit(usuarioId))
                {
                    return false;
                }

                if (!await VerifyLoseLimit(usuarioId, juegoId))
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
