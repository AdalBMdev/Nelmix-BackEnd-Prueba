using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Services;
using static Nelmix.DTOs.BankAccountDTO;

namespace Nelmix.Controllers
{

    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly IValidationsManager _validationsManager;

        public BankAccountController(IBankAccountService bankAccountService, IValidationsManager validationsManager)
        {
            _bankAccountService = bankAccountService;
            _validationsManager = validationsManager;
        }

        /// <summary>
        /// Crea una nueva cuenta bancaria para un usuario.
        /// </summary>
        /// <param name="createBankAccount">DTO que contiene la información para la creación de la cuenta bancaria.</param>
        /// <returns>Un ActionResult que indica si la cuenta bancaria se creó con éxito.</returns>
        [HttpPost("CrearCuentaBancaria")]
        public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequestDto createBankAccount)
        {

            var validation = await _validationsManager.ValidateAsync(createBankAccount);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateBankAccountExistAsync(createBankAccount.UserId);

            if(accountExist)
            {
                return BadRequest("Ya existe una cuenta bancaria asignada a su usuario");
            }

            try
            {
                bool result = await _bankAccountService.CreateBankAccount(createBankAccount);


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
                return StatusCode(500, "Se produjo un error en el servidor al CrearCuentaBancaria." + ex.Message);
            }
        }

        /// <summary>
        /// Elimina una cuenta bancaria perteneciente a un usuario.
        /// </summary>
        /// <param name="deleteBankAccountRequestDto">DTO que contiene la información para la eliminacion de la cuenta bancaria.</param>
        /// <returns>Un ActionResult que indica si la cuenta bancaria se creó con éxito.</returns>
        [HttpDelete("EliminarCuentaBancaria")]
        public async Task<IActionResult> DeleteBankAccount(DeleteBankAccountRequestDto deleteBankAccountRequestDto)
        {
            var validation = await _validationsManager.ValidateAsync(deleteBankAccountRequestDto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateBankAccountExistAsync(deleteBankAccountRequestDto.UserId);

            if (!accountExist)
            {
                return BadRequest("No existe la cuenta bancaria solicitada");
            }


            try
            {
                bool result = await _bankAccountService.DeleteBankAccount(deleteBankAccountRequestDto);

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
                return StatusCode(500, "Se produjo un error en el servidor al EliminarCuentaBancaria." + ex.Message);
            }
        }

        /// <summary>
        /// Añade saldo a una cuenta bancaria para un usuario.
        /// </summary>
        /// <param name="addBankAccountBalanceRequestDto">DTO que contiene la información para añadir saldo de la cuenta bancaria.</param>
        /// <returns>Un ActionResult que indica si la cuenta bancaria se creó con éxito.</returns>
        [HttpPost("añadir-saldo")]
        public async Task<IActionResult> AddBankAccountBalance(AddBankAccountBalanceRequestDto addBankAccountBalanceRequestDto)
        {

            var validation = await _validationsManager.ValidateAsync(addBankAccountBalanceRequestDto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var accountExist = await _validationsManager.ValidateBankAccountExistAsync(addBankAccountBalanceRequestDto.UserId);

            if (!accountExist)
            {
                return BadRequest("No existe la cuenta bancaria solicitada");
            }

            try
            {
                bool result = await _bankAccountService.AddBankAccountBalance(addBankAccountBalanceRequestDto);

                if (result)
                {
                    return Ok("Saldo añadido con éxito.");
                }

                return BadRequest("Error al añadir saldo a la cuenta bancaria.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Se produjo un error en el servidor al añadir saldo a la cuenta." + ex.Message);
            }

        }
    }
}
