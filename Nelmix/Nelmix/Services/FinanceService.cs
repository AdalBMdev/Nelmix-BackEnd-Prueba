﻿using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;

namespace Nelmix.Services
{
    public class FinanceService : IFinanceService
    {

        private readonly CasinoContext _context;

        public FinanceService(CasinoContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Obtiene el estado financiero de un usuario, incluyendo su saldo actual, ganancias, pérdidas y cantidad de fichas.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>True si se obtiene el estado financiero con éxito, de lo contrario, False.</returns>
        public async Task<EstadoFinanciero> GetFinancialStatusUser(int userId)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@usuario_id", userId),
                    new SqlParameter("@saldo_actual", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 10, Scale = 2 },
                    new SqlParameter("@ganancias", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 10, Scale = 2 },
                    new SqlParameter("@perdidas", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 10, Scale = 2 },
                    new SqlParameter("@fichas", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC ObtenerEstadoFinancieroUsuario @usuario_id, @saldo_actual OUT, @ganancias OUT, @perdidas OUT, @fichas OUT", parameters);

                var financialStatus = new EstadoFinanciero
                {
                    Balance = (decimal)parameters[1].Value,
                    Earnings = (decimal)parameters[2].Value,
                    Losses = (decimal)parameters[3].Value,
                    Chips = (int)parameters[4].Value
                };

                return financialStatus;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el estado financiero del usuario: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene las ganancias y pérdidas por juego para un usuario específico.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>Una lista de objetos de tipo GananciasYPérdidasPorJuego que contiene información sobre ganancias y pérdidas por juego.</returns>
        public async Task<List<GananciasYPérdidasPorJuego>> GetProfitAndLossFromGaming(int userId)
        {
            try
            {
                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@usuario_id", userId)
        };

                var results = await _context.Set<GananciasYPérdidasPorJuego>()
                    .FromSqlRaw("EXEC ObtenerGananciasYPérdidasPorJuego @usuario_id", parameters.ToArray())
                    .ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ganancias y pérdidas por juego: " + ex.Message);
            }
        }





    }
}
