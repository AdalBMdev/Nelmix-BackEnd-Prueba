using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;
using System.Data;
using static Nelmix.DTOs.CurrenciesDTO;

namespace Nelmix.Services
{
    public class CurrenciesService : ICurrenciesServices
    {

        private readonly CasinoContext _context;

        public CurrenciesService(CasinoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Convierte el saldo de una cuenta a dólares.
        /// </summary>
        /// <param name="convertCurrencyDollarsRequestDto">Objeto con el Identificador de la cuenta.</param>
        /// <returns>El saldo convertido a dólares.</returns>
        public async Task<decimal> ConvertCurrencyDollars(ConvertCurrencyDollarsRequestDto convertCurrencyDollarsRequestDto)
        {
            try
            {
                var cuentaBancaria = await _context.CuentasBancarias.FindAsync(convertCurrencyDollarsRequestDto.AccountId);

                var monedaId = cuentaBancaria.MonedaId;

                var tasaConversion = await _context.TasasDeCambios
                    .Where(tc => tc.MonedaId == monedaId)
                    .Select(tc => tc.Tasa)
                    .FirstOrDefaultAsync();

                if (tasaConversion > 0)
                {
                    var nuevoSaldo = cuentaBancaria.Saldo * tasaConversion;

                    cuentaBancaria.MonedaId = await _context.Monedas
                        .Where(m => m.Nombre == "Dólar estadounidense")
                        .Select(m => m.MonedaId)
                        .FirstOrDefaultAsync();

                    cuentaBancaria.Saldo = nuevoSaldo;

                    await _context.SaveChangesAsync();

                    return (decimal)nuevoSaldo;
                }
                

                throw new Exception("tasa de cambio no válida.");
            }
            catch (Exception ex)
            {

                throw new Exception("Error durante la conversión a dólares." + ex.Message);
            }
        }

        /// <summary>
        /// Compra fichas en dólares para un usuario.
        /// </summary>
        /// <param name="buyChipsInDollarsRequestDto">Objeto con UserId, TypeFieldId y Quantity.</param>
        /// <returns>Un mensaje indicando si la compra de fichas fue exitosa o un mensaje de error.</returns>
        public async Task BuyChipsInDollars(BuyChipsInDollarsRequestDto buyChipsInDollarsRequestDto)
        {
            try
            {
                var user = await _context.Usuarios.FindAsync(buyChipsInDollarsRequestDto.UserId);
                var chipType = await _context.TiposDeFichas.FindAsync(buyChipsInDollarsRequestDto.TypeFileId);

                
                decimal chipValueInDollars = (decimal)chipType.Valor;
                decimal totalCostInDollars = chipValueInDollars * buyChipsInDollarsRequestDto.Quantity;

                var userDollarsAccount = await _context.CuentasBancarias
                    .FirstOrDefaultAsync(account => account.UserId == buyChipsInDollarsRequestDto.UserId && account.MonedaId == 1);


                userDollarsAccount.Saldo -= totalCostInDollars;

                var chipRecord = await _context.Fichas
                    .FirstOrDefaultAsync(record => record.UsuarioId == buyChipsInDollarsRequestDto.UserId && record.TipoFichaId == buyChipsInDollarsRequestDto.TypeFileId);

                if (chipRecord != null)
                {
                    chipRecord.CantidadDisponible += buyChipsInDollarsRequestDto.Quantity;
                }
                else
                {
                    _context.Fichas.Add(new Ficha
                    {
                        TipoFichaId = buyChipsInDollarsRequestDto.TypeFileId,
                        CantidadDisponible = buyChipsInDollarsRequestDto.Quantity,
                        UsuarioId = buyChipsInDollarsRequestDto.UserId
                    });
                }

                await _context.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error durante la conversión a dólares." + ex.Message);
            }
        }


        /// <summary>
        /// Intercambia fichas por una moneda específica para un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="typeFileId">Identificador del tipo de ficha.</param>
        /// <param name="currencyDestinationId">Moneda de destino para la conversión.</param>
        /// <param name="quantityFichas">Cantidad de fichas a convertir.</param>
        /// <returns>Un mensaje indicando si la conversión de fichas fue exitosa o un mensaje de error.</returns>
        public async Task<string> ExchangeChipsToCurrency(int userId, int typeFileId, int currencyDestinationId, int quantityFichas)
        {
            try
            {
                var tipoFicha = await _context.TiposDeFichas.FindAsync(typeFileId);
                var cuentaBancaria = await _context.CuentasBancarias
                    .FirstOrDefaultAsync(cb => cb.UserId == userId);

                if (tipoFicha != null && cuentaBancaria != null)
                {
                    int valorFicha = tipoFicha.Valor;
                    int monedaActual = cuentaBancaria.MonedaId;  
                    decimal saldoUsuario = cuentaBancaria.Saldo;

                    decimal tasaConversion = await _context.TasasDeCambios
                        .Where(tc => tc.MonedaId == currencyDestinationId)
                        .Select(tc => tc.Tasa)
                        .FirstOrDefaultAsync();

                    int cantidadFichasDisponibles = await _context.Fichas
                        .Where(f => f.UsuarioId == userId && f.TipoFichaId == typeFileId)
                        .Select(f => f.CantidadDisponible)
                        .FirstOrDefaultAsync();

                    if (cantidadFichasDisponibles >= quantityFichas && currencyDestinationId != 1)
                    {
                        decimal valorTotalDestino = valorFicha * quantityFichas;

                        if (monedaActual != currencyDestinationId)
                        {
                            valorTotalDestino = (valorFicha * quantityFichas + saldoUsuario) / tasaConversion;
                        }

                        else valorTotalDestino = (valorFicha * quantityFichas + saldoUsuario);

                        cuentaBancaria.Saldo = valorTotalDestino;
                        cuentaBancaria.MonedaId = currencyDestinationId; 

                        var fichasUsuario = await _context.Fichas
                            .FirstOrDefaultAsync(f => f.UsuarioId == userId && f.TipoFichaId == typeFileId);

                        if (fichasUsuario != null)
                        {
                            fichasUsuario.CantidadDisponible -= quantityFichas;
                        }

                        await _context.SaveChangesAsync();

                        return $"Cambio de fichas exitoso. Nuevo saldo en {currencyDestinationId}: {valorTotalDestino}";
                    }
                    else
                    {
                        return "No tienes suficientes fichas para realizar el cambio o no puedes cambiar a dólares.";
                    }
                }
                else
                {
                    return "Error al cambiar las fichas de moneda.";
                }
            }
            catch (Exception ex)
            {
                return $"Se produjo un error al realizar el cambio de fichas a moneda: {ex.Message}";
            } 
        }




    }
}
