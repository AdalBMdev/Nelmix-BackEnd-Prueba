using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Services;

namespace Nelmix.Controllers
{
    public class GameController : Controller
    {

        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
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
        public async Task<IActionResult> PlayCraps(int userId, int redChips, int yellowChips, int greenChips, int blackChips)
        {
            int gameId = 1;

            var verificationResult = await _gameService.VerifyPlay(userId, redChips, yellowChips, greenChips, blackChips, gameId);

            if (!verificationResult)
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = _gameService.PlayCraps();
            bool finalVictory = victory ?? false; 

            if (victory == null)
            {
                return BadRequest("No se ha determinado una victoria o derrota.");
            }

            await _gameService.ManageUserGame(userId, redChips, yellowChips, greenChips, blackChips, finalVictory, gameId);

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
        public async Task<IActionResult> PlayTragaperras(int userId, int redChips, int yellowChips, int greenChips, int blackChips )
        {
            int gameId = 2;

            var verificationResult = await _gameService.VerifyPlay(userId, redChips, yellowChips, greenChips, blackChips, gameId);
        
            if (!verificationResult)
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = _gameService.PlayTragaperras();
            await _gameService.ManageUserGame(userId, redChips, yellowChips, greenChips, blackChips, victory, gameId);

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
        public async Task<IActionResult> PlayBlackjack(int userId, int redChips, int yellowChips, int greenChips, int blackChips)
        {

            int gameId = 3;

            var verificationResult = await _gameService.VerifyPlay(userId, redChips, yellowChips, greenChips, blackChips, gameId);

            if (!verificationResult)
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = _gameService.PlayBlackjack();
            await _gameService.ManageUserGame(userId, redChips, yellowChips, greenChips, blackChips, victory, gameId);

            return Ok(resultMessage);
        }

    }
}
