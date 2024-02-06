using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    /// <summary>
    /// Controlador para operaciones relacionadas con divisas
    /// </summary>
    public class CurrenciesController : Controller
    {
        
        private readonly ICurrenciesServices _currenciesServices;

        /// <summary>
        /// Constructor del controlador CurrenciesController.
        /// </summary>
        public CurrenciesController(ICurrenciesServices currenciesServices)
        {
            _currenciesServices = currenciesServices;
        }


        /// <summary>
        /// Convierte la moneda de una cuenta a dólares.
        /// </summary>
        /// <param name="accountId">Identificador de la cuenta de banco. Ejemplo: 1</param>
        /// <returns>Un ActionResult que indica si la conversión a dólares se realizó con éxito. </returns>
        [HttpPost("convertirMonedaDolares")]
        public async Task<IActionResult> ConvertCurrencyDollars(int accountId)
        {
            try
            {
                decimal result = await _currenciesServices.ConvertCurrencyDollars(accountId);

                if (result != 0)
                {
                    return Ok("La conversión a dólares se realizó con éxito.");
                }
                else
                {
                    return BadRequest("No se pudo realizar la conversión a dólares.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Compra fichas solo en dólares.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="typeFileId">Identificador del tipo de ficha. Ejemplo: 1</param>
        /// <param name="quantity">Cantidad de fichas a comprar. Ejemplo: 10</param>
        /// <returns>Un ActionResult que indica si la compra de fichas en dólares se realizó con éxito.</returns>
        [HttpPost("ComprarFichasEnDolares")]
        public async Task<IActionResult> BuyChipsInDollars(int userId, int typeFileId, int quantity)
        {
            try
            {
                var resultado = await _currenciesServices.BuyChipsInDollars(userId, typeFileId, quantity);

                if (resultado == "Compra de fichas exitosa.")
                {
                    return Ok(resultado);
                }
                else
                {
                    return BadRequest(resultado);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Se produjo un error en el servidor al Comprar Fichas En Dolares: " + ex.Message);
            }
        }


        /// <summary>
        /// Realiza el intercambio de fichas por una moneda específica menos dolares.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="typeFileId">Identificador del tipo de ficha. Ejemplo: 1</param>
        /// <param name="currencyDestinationId">Id de la moneda de destino para el intercambio. Ejemplo: 2</param>
        /// <param name="quantityFichas">Cantidad de fichas a intercambiar. Ejemplo: 10</param>
        /// <returns>Un ActionResult que indica si el intercambio de fichas a moneda se realizó con éxito.</returns>
        [HttpPost("CambioFichasAMoneda")]
        public async Task<IActionResult> ExchangeChipsToCurrency(int userId, int typeFileId, int currencyDestinationId, int quantityFichas)
        {
            try
            {
                var resultado = await _currenciesServices.ExchangeChipsToCurrency(userId, typeFileId, currencyDestinationId, quantityFichas);
                return Ok(resultado);
            }

            catch (Exception ex)
            {
                // Maneja y registra el error aquí
                return StatusCode(500, "Se produjo un error en el servidor al Comprar Fichas En Dolares.");
            }
        }


    }
}
   
