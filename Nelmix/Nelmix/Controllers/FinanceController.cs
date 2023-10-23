using Microsoft.AspNetCore.Mvc;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    /// <summary>
    /// Controlador para operaciones relacionadas con la economia del usuario
    /// </summary>
    public class FinanceController : Controller
    {
        private readonly FinanceService finanzasService;

        /// <summary>
        /// Constructor del controlador FinanceController.
        /// </summary>
        public FinanceController()
        {
            finanzasService = new FinanceService();
        }

        /// <summary>
        /// Obtiene el estado financiero de un usuario, incluyendo saldo actual, ganancias, pérdidas y fichas.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <returns>Un ActionResult que contiene el estado financiero del usuario. </returns>
        [HttpGet("obtener-estado-financiero")]
        public IActionResult GetFinancialStatement(int userId)
        {
            decimal balance;
            decimal earnings;
            decimal losses;
            int chips;

            try
            {
                if (finanzasService.GetFinancialStatusUser(userId, out balance, out earnings, out losses, out chips))
                {
                    var statusFinancial = new
                    {
                        SaldoActual = balance,
                        Ganancias = earnings,
                        Perdidas = losses,
                        Fichas = chips
                    };

                    return Ok(statusFinancial);
                }
                return NotFound("No se encontró información financiera para el usuario.");
            }
            catch (Exception ex)
            {
                // Maneja y registra el error aquí
                return StatusCode(500, "Se produjo un error en el servidor al obtener el estado financiero.");
            }
        }

        /// <summary>
        /// Obtiene las ganancias y pérdidas por juego de un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <returns>Un ActionResult que contiene una lista de ganancias y pérdidas por juego.</returns>
        [HttpGet("ganancias-y-perdidas-por-juego")]
        public IActionResult ObtenerGananciasYPérdidasPorJuego(int userId)
        {

            try
            {
                var resultados = finanzasService.GetProfitAndLossFromGaming(userId);

                if (resultados == null || resultados.Count == 0)
                {
                    return NotFound();
                }

                return Ok(resultados);
            }

            catch (Exception ex)
            {
                // Maneja y registra el error aquí
                return StatusCode(500, "Se produjo un error en el servidor al obtener ganancias y perdidas por juego.");
            }

        }
    }
}
