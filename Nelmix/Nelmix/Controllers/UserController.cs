using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;
using Nelmix.Services;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidationsManager _validationsManager;

        public UserController(IUserService userService, IValidationsManager validationsManager)
        {
            _userService = userService;
            _validationsManager = validationsManager;
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="registerUsuario">Objeto de tipo Usuario que contiene los datos del usuario a registrar.</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserRequestDto registerUsuario)
        {

            var validation = await _validationsManager.ValidateAsync(registerUsuario);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var emailExist = await _validationsManager.ValidateEmailExistAsync(registerUsuario.Email);

            if (emailExist)
            {
                return BadRequest("Ya existe un usuario creado con este email");
            }

            try
            {
                await _userService.RegisterUser(registerUsuario);
                return Ok("Usuario registrado exitosamente.");
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Inicia sesión de un usuario.
        /// </summary>
        /// <param name="loginUsuario">Un objeto con informacion para el login</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserRequestDto loginUsuario)
        {

            var validation = await _validationsManager.ValidateAsync(loginUsuario);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var emailExist = await _validationsManager.ValidateEmailExistAsync(loginUsuario.Email);

            if (!emailExist)
            {
                return BadRequest("El email ingresado no tiene una cuenta en nuestro sistema");
            }

            try
            {
                bool loginResult = await _userService.Login(loginUsuario);

                if (loginResult)
                {
                    return Ok("Inicio de sesión exitoso.");
                }
                else
                {
                    return BadRequest("Correo o contraseña incorrectos. Por favor, inténtalo de nuevo.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message );
            }
        }


        /// <summary>
        /// Cambia la contraseña de un usuario.
        /// </summary>
        /// <param name="changePasswordUsuario">Un objeto con informacion para cambiar contraseña</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto changePasswordUsuario)
        {

            var validation = await _validationsManager.ValidateAsync(changePasswordUsuario);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var emailExist = await _validationsManager.ValidateEmailExistAsync(changePasswordUsuario.Email);

            if (!emailExist)
            {
                return BadRequest("El email ingresado no tiene una cuenta en nuestro sistema");
            }

            try
            {
                await _userService.ChangePassword(changePasswordUsuario);
                return Ok("Contraseña cambiada exitosamente.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }


        /// <summary>
        /// Asigna un adulto responsable a un usuario menor.
        /// </summary>
        /// <param name="usersEmails">Un objeto con 2 emails, el email del usuario mayor y menor</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPut("AsignarAdulto")]
        public async Task<IActionResult> AsignarAdultoResponsable(AssignAdultResponsableRequestDto usersEmails)
        {

            var validation = await _validationsManager.ValidateAsync(usersEmails);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var adultExist = await _validationsManager.ValidateAdultExistAsync(usersEmails.MailUserAdult);

            if (!adultExist)
            {
                return BadRequest("El usuario ingresado no tiene una cuenta en nuestro sistema o es menor de edad");
            }

            var minorExist = await _validationsManager.ValidateUserIsMinorExistAsync(usersEmails.MailUserMinor);

            if (!minorExist)
            {
                return BadRequest("El usuario ingresado no tiene una cuenta en nuestro sistema o no es menor de edad por lo que no necesita adulto asignado");
            }

            try
            {
                await _userService.AssignAdultResponsible(usersEmails);
                return Ok("Se asigno correctamente el adulto");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message );
            }
        }


        /// <summary>
        /// Desactiva un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario a desactivar. Ejemplo: 1</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPut("desactivar-usuario")]
        public async Task<IActionResult> DesactivateUser(int userId)
        {
            try
            {
                await _userService.ChangeUserStatusInactiveAsync(userId);
                return Ok("Usuario desactivado exitosamente.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

    }
}
