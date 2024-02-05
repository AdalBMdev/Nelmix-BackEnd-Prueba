﻿using Microsoft.AspNetCore.Mvc;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    /// <summary>
    /// Controlador para operaciones relacionadas con Usuario.
    /// </summary>
    public class UserController : Controller
    {
        private readonly IUserService _userService;


        /// <summary>
        /// Constructor del controlador UserController.
        /// </summary>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="oUsuario">Objeto de tipo Usuario que contiene los datos del usuario a registrar.</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(Usuario oUsuario)
        {

            try
            {

                bool registrationResult = await _userService.RegisterUser(oUsuario);

                if (registrationResult)
                {
                    return Ok("Usuario registrado exitosamente.");
                }
                else
                {
                    return BadRequest("Error al registrar usuario.");
                }
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Inicia sesión de un usuario.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario. Ejemplo: john@gmail.com</param>
        /// <param name="password">Contraseña del usuario. Ejemplo: 123</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                bool loginResult = await _userService.Login(email, password);

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
        /// <param name="email">Correo electrónico del usuario. Ejemplo: john@gmail.com</param>
        /// <param name="password">Contraseña del usuario. Ejemplo: 123</param>
        /// <param name="newPassword">Nueva contraseña para el usuario. Ejemplo: 1234</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string email, string password, string newPassword)
        {
            try
            {
                (bool success, string message) = await _userService.ChangePassword(email, password, newPassword);

                if (success)
                {
                    return Ok("Contraseña cambiada exitosamente.");
                }
                else
                {
                    return BadRequest(message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }


        /// <summary>
        /// Asigna un adulto responsable a un usuario menor.
        /// </summary>
        /// <param name="mailUserMinor">Correo electrónico del usuario menor.  Ejemplo: userMinor@gmail.com</param>
        /// <param name="mailUserAdult">Correo electrónico del adulto responsable.  Ejemplo: userAdult@gmail.com</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPut("AsignarAdulto")]
        public async Task<IActionResult> AsignarAdultoResponsable(string mailUserMinor, string mailUserAdult)
        {
            try
            {
                (bool registrado, string mensaje) = await _userService.AssignAdultResponsible(mailUserMinor, mailUserAdult);

                if (registrado)
                {
                    return Ok(mensaje);
                }
                else
                {
                    return BadRequest(mensaje);
                }
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
