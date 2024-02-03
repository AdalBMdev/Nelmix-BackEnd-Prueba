using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    /// <summary>
    /// Controlador para operaciones relacionadas con cuentas bancarias.
    /// </summary>
    public class BankAccountController : Controller
    {
        private readonly BankAccountService cuentaBancariaService;
        private readonly CasinoContext _context;

        /// <summary>
        /// Constructor del controlador BankAccountController.
        /// </summary>
        public BankAccountController(CasinoContext context)
        {
            _context = context;
            cuentaBancariaService = new BankAccountService(_context);
        }

        /// <summary>
        /// Crea una cuenta bancaria para un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <param name="monedaId">Identificador de la moneda. Ejemplo: 2</param>
        /// <param name="saldo">Saldo inicial de la cuenta. Ejemplo: 1000.50</param>
        /// <returns>Un ActionResult que indica si la cuenta bancaria se creó con éxito.</returns>
        [HttpPost("CrearCuentaBancaria")]
        public IActionResult CreateBankAccount(int userId, int monedaId, decimal saldo)
        {
            try
            {
                if (cuentaBancariaService.CreateBankAccount(userId, monedaId, saldo))
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
        public IActionResult DeleteBankAccount(int cuentaId, int userId)
        {
            try
            {
                if (cuentaBancariaService.DeleteBankAccount(cuentaId, userId))
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
        public IActionResult AddBankAccountBalance(int userId, int currencyId, decimal balance)
        {
            try
            {
                if (cuentaBancariaService.AddBankAccountBalance(userId, currencyId, balance))
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
