using Microsoft.AspNetCore.Mvc;
using Nelmix.Models;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    /// <summary>
    /// Controlador para operaciones relacionadas con divisas
    /// </summary>
    public class CurrenciesController : Controller
    {
        private readonly CurrenciesService divisasService;

        /// <summary>
        /// Constructor del controlador CurrenciesController.
        /// </summary>
        public CurrenciesController()
        {
            divisasService = new CurrenciesService();
        }


        /// <summary>
        /// Convierte la moneda de una cuenta a dólares.
        /// </summary>
        /// <param name="accountId">Identificador de la cuenta de banco. Ejemplo: 1</param>
        /// <returns>Un ActionResult que indica si la conversión a dólares se realizó con éxito. </returns>
        [HttpPost("convertirMonedaDolares")]
        public IActionResult ConvertCurrencyDollars(int accountId)
        {
            
                var result = divisasService.ConvertCurrencyDollars(accountId);

                if (result != 0)
                {
                    return Ok("La conversión a dólares se realizó con éxito.");
                }
                else
                {
                    return BadRequest("No se pudo realizar la conversión a dólares.");
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
        public IActionResult BuyChipsInDollars(int userId, int typeFileId, int quantity)
        {
            try
            {
                var resultado = divisasService.BuyChipsInDollars(userId, typeFileId, quantity);

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
                // Maneja y registra el error aquí
                return StatusCode(500, "Se produjo un error en el servidor al Comprar Fichas En Dolares.");
            }
        }

        /// <summary>
        /// Realiza el intercambio de fichas por una moneda específica menos dolares.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="typeFileId">Identificador del tipo de ficha. Ejemplo: 1</param>
        /// <param name="currencyDestination">Moneda de destino para el intercambio(Nombre especifico de la moneda). Ejemplo: Peso dominicano</param>
        /// <param name="quantityFichas">Cantidad de fichas a intercambiar. Ejemplo: 10</param>
        /// <returns>Un ActionResult que indica si el intercambio de fichas a moneda se realizó con éxito.</returns>
        [HttpPost("CambioFichasAMoneda")]
        public IActionResult ExchangeChipsToCurrency(int userId, int typeFileId, string currencyDestination, int quantityFichas)
        {
            try
            {
                var resultado = divisasService.ExchangeChipsToCurrency(userId, typeFileId, currencyDestination, quantityFichas);
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
