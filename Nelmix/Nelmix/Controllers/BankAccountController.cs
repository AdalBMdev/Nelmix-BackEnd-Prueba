using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    /// <summary>
    /// Controlador para operaciones relacionadas con cuentas bancarias.
    /// </summary>
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        /// <summary>
        /// Constructor del controlador BankAccountController.
        /// </summary>
        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        /// <summary>
        /// Crea una cuenta bancaria para un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="monedaId">Identificador de la moneda. Ejemplo: 2</param>
        /// <returns>Un ActionResult que indica si la cuenta bancaria se creó con éxito.</returns>
        [HttpPost("CrearCuentaBancaria")]
        public async Task<IActionResult> CreateBankAccount(int userId, int monedaId)
        {
            try
            {
                bool result = await _bankAccountService.CreateBankAccount(userId, monedaId);


                if (result)
                {
                    return Ok("Cuenta bancaria creada exitosamente.");
                }
                else
                {
                    return BadRequest("Error al crear la cuenta bancaria.");
                }
            }
            catch (Exception ex)
            {
                // Maneja y registra el error aquí
                return StatusCode(500, "Se produjo un error en el servidor al CrearCuentaBancaria.");
            }
        }

        /// <summary>
        /// Elimina una cuenta bancaria de un usuario.
        /// </summary>
        /// <param name="cuentaId">Identificador de la cuenta bancaria Ejemplo: 1.</param>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <returns>Un ActionResult que indica si la cuenta bancaria se eliminó con éxito.</returns>
        [HttpDelete("EliminarCuentaBancaria")]
        public async Task<IActionResult> DeleteBankAccount(int cuentaId, int userId)
        {
            try
            {
                bool result = await _bankAccountService.DeleteBankAccount(cuentaId, userId);

                if (result)
                {
                    return Ok("Cuenta bancaria eliminada exitosamente.");
                }
                else
                {
                    return BadRequest("Error al eliminar la cuenta bancaria.");
                }
            }
            catch (Exception ex)
            {
                // Maneja y registra el error aquí
                return StatusCode(500, "Se produjo un error en el servidor al EliminarCuentaBancaria.");
            }
        }

        /// <summary>
        /// Añade saldo a una cuenta bancaria.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="currencyId">Identificador de la moneda. Ejemplo: 2</param>
        /// <param name="balance">Saldo a añadir a la cuenta. Ejemplo: 50000</param>
        /// <returns>Un ActionResult que indica si el saldo se añadió con éxito a la cuenta bancaria.</returns>
        [HttpPost("añadir-saldo")]
        public async Task<IActionResult> AddBankAccountBalance(int userId, int currencyId, decimal balance)
        {
            try
            {
                bool result = await _bankAccountService.AddBankAccountBalance(userId, currencyId, balance);

                if (result)
                {
                    return Ok("Saldo añadido con éxito.");
                }

                return BadRequest("Error al añadir saldo a la cuenta bancaria.");
            }

            catch (Exception ex)
            {
                // Maneja y registra el error aquí
                return StatusCode(500, "Se produjo un error en el servidor al añadir saldo a la cuenta.");
            }

        }
    }
}
