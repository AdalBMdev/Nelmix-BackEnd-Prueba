using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;
using Nelmix.Services;
using Nelmix.Validations;
using static Nelmix.DTOs.CurrenciesDTO;

namespace Nelmix.Controllers
{

    public class CurrenciesController : Controller
    {
        
        private readonly ICurrenciesServices _currenciesServices;
        private readonly IValidationsManager _validationsManager;


        public CurrenciesController(ICurrenciesServices currenciesServices, IValidationsManager validationsManager)
        {
            _currenciesServices = currenciesServices;
            _validationsManager = validationsManager;
        }


        /// <summary>
        /// Convierte la moneda de una cuenta a dólares.
        /// </summary>
        /// <param name="convertCurrencyDollarsRequestDto">Objeto con el Identificador de la cuenta.</param>
        /// <returns>Un ActionResult que indica si la conversión a dólares se realizó con éxito. </returns>
        [HttpPost("convertirMonedaDolares")]
        public async Task<IActionResult> ConvertCurrencyDollars(ConvertCurrencyDollarsRequestDto convertCurrencyDollarsRequestDto)
        {
            var validation = await _validationsManager.ValidateAsync(convertCurrencyDollarsRequestDto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateBankAccountExistAsync(convertCurrencyDollarsRequestDto.AccountId);

            if (!accountExist)
            {
                return BadRequest("No existe la cuenta de banco proporcionada");
            }

            try
            {
                decimal result = await _currenciesServices.ConvertCurrencyDollars(convertCurrencyDollarsRequestDto);
                return Ok("La conversión a dólares se realizó con éxito. Saldo: " + result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Compra fichas solo en dólares.
        /// </summary>
        /// <param name="buyChipsInDollarsRequestDto">Objeto con UserId, TypeFieldId y Quantity.</param>
        /// <returns>Un ActionResult que indica si la compra de fichas en dólares se realizó con éxito.</returns>
        [HttpPost("ComprarFichasEnDolares")]
        public async Task<IActionResult> BuyChipsInDollars(BuyChipsInDollarsRequestDto buyChipsInDollarsRequestDto)
        {

            var validation = await _validationsManager.ValidateAsync(buyChipsInDollarsRequestDto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateUserBankAccountExistAsync(buyChipsInDollarsRequestDto.UserId);

            if (!accountExist)
            {
                return BadRequest("No existe la cuenta de banco asignada al usuario proporcionada");
            }

            var result= await _validationsManager.ValidateBankAccountCurrencyIsDolarAndSufficientBalance(buyChipsInDollarsRequestDto);

            if (!result)
            {
                return BadRequest("La cuenta bancaria no tiene suficiente saldo en dólares para realizar la compra de fichas");
            }

            try
            {
                await _currenciesServices.BuyChipsInDollars(buyChipsInDollarsRequestDto);
                return Ok("Se han comprado las fichas correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Se produjo un error en el servidor al Comprar Fichas En Dolares: " + ex.Message);
            }
        }


        /// <summary>
        /// Realiza el intercambio de fichas por una moneda específica menos dolares.
        /// </summary>
        /// <param name="exchangeChips">Objeto con UserId, TypeFieldId, Quantity y CurrencyDestinationId.</param>
        /// <returns>Un ActionResult que indica si el intercambio de fichas a moneda se realizó con éxito.</returns>
        [HttpPost("CambioFichasAMoneda")]
        public async Task<IActionResult> ExchangeChipsToCurrency(ExchangeChipsToCurrencyRequestDto exchangeChips)
        {
            var validation = await _validationsManager.ValidateAsync(exchangeChips);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateUserBankAccountExistAsync(exchangeChips.UserId);

            if (!accountExist)
            {
                return BadRequest("No existe la cuenta de banco asignada al usuario proporcionada");
            }

            var chipsSuficient = await _validationsManager.ValidateChipsSuficientAsync(exchangeChips.UserId, exchangeChips.TypeFileId, exchangeChips.Quantity);

            if (!chipsSuficient)
            {
                return BadRequest("El usuario no tiene suficientes fichas para realizar el intercambio");
            }

            try
            {
                var resultado = await _currenciesServices.ExchangeChipsToCurrency(exchangeChips);
                return Ok(resultado);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Se produjo un error en el servidor al Comprar Fichas En Dolares." + ex.Message);
            }
        }


    }
}
   
