﻿using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    public class FinanceController : Controller
    {
        private readonly IFinanceService _finanzasService;

        public FinanceController(IFinanceService finanzasService)
        {
            _finanzasService = finanzasService;
        }

        /// <summary>
        /// Obtiene el estado financiero de un usuario, incluyendo saldo actual, ganancias, pérdidas y fichas.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <returns>Un ActionResult que contiene el estado financiero del usuario. </returns>
        [HttpGet("obtener-estado-financiero")]
        public async Task<IActionResult> GetFinancialStatement(int userId)
        {
            try
            {
                var financialStatus = await _finanzasService.GetFinancialStatusUser(userId);

                if (financialStatus != null)
                {
                    var statusFinancial = new
                    {
                        SaldoActual = financialStatus.Balance,
                        Ganancias = financialStatus.Earnings,
                        Perdidas = financialStatus.Losses,
                        Fichas = financialStatus.Chips
                    };

                    return Ok(statusFinancial);
                }

                return NotFound("No se encontró información financiera para el usuario.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Se produjo un error en el servidor al obtener el estado financiero." + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene las ganancias y pérdidas por juego de un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario. Ejemplo: 1</param>
        /// <returns>Un ActionResult que contiene una lista de ganancias y pérdidas por juego.</returns>
        [HttpGet("ganancias-y-perdidas-por-juego")]
        public async Task<IActionResult> ObtenerGananciasYPérdidasPorJuego(int userId)
        {
            try
            {
                var resultados = await _finanzasService.GetProfitAndLossFromGaming(userId);

                if (resultados == null || resultados.Count == 0)
                {
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Se produjo un error en el servidor al obtener ganancias y pérdidas por juego: " + ex.Message);
            }
        }

    }
}
