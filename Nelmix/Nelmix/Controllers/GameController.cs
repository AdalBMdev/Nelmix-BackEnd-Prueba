using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Services;
using static Nelmix.DTOs.CurrenciesDTO;
using static Nelmix.DTOs.GameDTO;

namespace Nelmix.Controllers
{
    public class GameController : Controller
    {

        private readonly IGameService _gameService;
        private readonly IValidationsManager _validationsManager;


        public GameController(IGameService gameService, IValidationsManager validationsManager)
        {
            _gameService = gameService;
            _validationsManager = validationsManager;
        }

        //// <summary>
        /// Permite a un usuario jugar a Craps.
        /// </summary>
        /// <param name="request">DTO que contiene los datos necesarios para gestionar el juego del usuario.</param>
        /// <returns>Un ActionResult que indica el resultado del juego.</returns>
        [HttpPost("PlayCraps")]
        public async Task<IActionResult> PlayCraps(ManageUserGameRequestDto request)
        {
            request.GameId = 1;

            var validation = await _validationsManager.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateBankAccountExistAsync(request.UserId);

            if (!accountExist)
            {
                return BadRequest("No existe la cuenta de banco asignada a este usuario");
            }

            var verificationResult = await _validationsManager.VerifyPlay(request);

            if (!verificationResult)
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = _gameService.PlayCraps();
            request.Victory = victory ?? false; 

            if (victory == null)
            {
                return BadRequest("No se ha determinado una victoria o derrota.");
            }

            await _gameService.ManageUserGame(request);

            return Ok(resultMessage);
        }

        /// <summary>
        /// Permite a un usuario jugar a Tragaperras.
        /// </summary>
        /// <param name="request">DTO que contiene los datos necesarios para gestionar el juego del usuario.</param>
        /// <returns>Un ActionResult que indica el resultado del juego.</returns>
        [HttpPost("PlayTragaperras")]
        public async Task<IActionResult> PlayTragaperras(ManageUserGameRequestDto request)
        {
            request.GameId = 2;

            var validation = await _validationsManager.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateBankAccountExistAsync(request.UserId);

            if (!accountExist)
            {
                return BadRequest("No existe la cuenta de banco asignada a este usuario");
            }

            var verificationResult = await _validationsManager.VerifyPlay(request);

            if (!verificationResult)
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = _gameService.PlayTragaperras();
            await _gameService.ManageUserGame(request);

            return Ok(resultMessage);
        }

        /// <summary>
        /// Permite a un usuario jugar a Blackjack.
        /// </summary>
        /// <param name="request">DTO que contiene los datos necesarios para gestionar el juego del usuario.</param>
        /// <returns>Un ActionResult que indica el resultado del juego.</returns>
        [HttpPost("PlayBlackjack")]
        public async Task<IActionResult> PlayBlackjack(ManageUserGameRequestDto request)
        {

            request.GameId = 3;

            var validation = await _validationsManager.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateBankAccountExistAsync(request.UserId);

            if (!accountExist)
            {
                return BadRequest("No existe la cuenta de banco asignada a este usuario");
            }

            var verificationResult = await _validationsManager.VerifyPlay(request);

            if (!verificationResult)
            {
                return BadRequest("No cumples con los requisitos o has excedido los límites.");
            }

            var (victory, resultMessage) = _gameService.PlayBlackjack();
            await _gameService.ManageUserGame(request);

            return Ok(resultMessage);
        }

    }
}
