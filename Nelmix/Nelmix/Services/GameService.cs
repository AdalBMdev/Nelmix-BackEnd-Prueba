using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using Nelmix.Interfaces;
using static Nelmix.DTOs.GameDTO;


namespace Nelmix.Services
{
    public class GameService : IGameService
    {

        private readonly CasinoContext _context;

        public GameService(CasinoContext context)
        {
            _context = context;
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
        public async Task<bool> ManageUserGame(ManageUserGameRequestDto request)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC GestionarJuegoUsuario @usuario_id={0}, @fichas_rojas={1}, @fichas_amarillas={2}, @fichas_verdes={3}, @fichas_negras={4}, @victoria={5}, @juego_id={6}",
                    request.UserId, request.RedChips, request.YellowChips, request.GreenChips, request.BlackChips, request.Victory ? 1 : 0, request.GameId);


                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al gestionar el juego del usuario" + ex.Message);
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
