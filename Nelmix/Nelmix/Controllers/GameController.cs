using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    /// <summary>
    /// Controlador para operaciones relacionadas con juegos de casino.
    /// </summary>
    public class GameController : Controller
    {
        private readonly GameService gameService;
        private readonly CasinoContext _context;

        /// <summary>
        /// Constructor del controlador GameController.
        /// </summary>
        public GameController(CasinoContext context)
        {
            _context = context;
            gameService = new GameService(_context);
        }

        /// <summary>
        /// Permite a un usuario jugar a Craps.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="redChips">Cantidad de fichas rojas apostadas. Ejemplo: 10</param>
        /// <param name="yellowChips">Cantidad de fichas amarillas apostadas. Ejemplo: 0</param>
        /// <param name="greenChips">Cantidad de fichas verdes apostadas. Ejemplo: 0</param>
        /// <param name="blackChips">Cantidad de fichas negras apostadas. Ejemplo: 0</param>
        /// <returns>Un ActionResult que indica el resultado del juego.</returns>
        [HttpPost("PlayCraps")]
        public IActionResult PlayCraps(int userId, int redChips, int yellowChips, int greenChips, int blackChips)
        {
            int gameId = 1;

            if (!gameService.VerifyPlay(userId, redChips, yellowChips, greenChips, blackChips, gameId))
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = gameService.PlayCraps();
            bool finalVictory = victory ?? false; 

            if (victory == null)
            {
                return BadRequest("No se ha determinado una victoria o derrota.");
            }

            gameService.ManageUserGame(userId, redChips, yellowChips, greenChips, blackChips, finalVictory, gameId);

            return Ok(resultMessage);
        }

        /// <summary>
        /// Permite a un usuario jugar a Tragaperras.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="redChips">Cantidad de fichas rojas apostadas. Ejemplo: 10</param>
        /// <param name="yellowChips">Cantidad de fichas amarillas apostadas. Ejemplo: 0</param>
        /// <param name="greenChips">Cantidad de fichas verdes apostadas. Ejemplo: 0</param>
        /// <param name="blackChips">Cantidad de fichas negras apostadas. Ejemplo: 0</param>
        /// <returns>Un ActionResult que indica el resultado del juego.</returns>
        [HttpPost("PlayTragaperras")]
        public IActionResult PlayTragaperras(int userId, int redChips, int yellowChips, int greenChips, int blackChips )
        {
            int gameId = 2;

            if (!gameService.VerifyPlay(userId, redChips, yellowChips, greenChips, blackChips, gameId))
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = gameService.PlayTragaperras();
            gameService.ManageUserGame(userId, redChips, yellowChips, greenChips, blackChips, victory, gameId);

            return Ok(resultMessage);
        }

        /// <summary>
        /// Permite a un usuario jugar a Blackjack.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="redChips">Cantidad de fichas rojas apostadas. Ejemplo: 10</param>
        /// <param name="yellowChips">Cantidad de fichas amarillas apostadas. Ejemplo: 0</param>
        /// <param name="greenChips">Cantidad de fichas verdes apostadas. Ejemplo: 0</param>
        /// <param name="blackChips">Cantidad de fichas negras apostadas. Ejemplo: 0</param>
        /// <returns>Un ActionResult que indica el resultado del juego.</returns>
        [HttpPost("PlayBlackjack")]
        public IActionResult PlayBlackjack(int userId, int redChips, int yellowChips, int greenChips, int blackChips)
        {

            int gameId = 3;

            if (!gameService.VerifyPlay(userId, redChips, yellowChips, greenChips, blackChips, gameId))
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = gameService.PlayBlackjack();
            gameService.ManageUserGame(userId, redChips, yellowChips, greenChips, blackChips, victory, gameId);

            return Ok(resultMessage);
        }

    }
}
