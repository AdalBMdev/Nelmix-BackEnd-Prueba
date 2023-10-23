using Microsoft.AspNetCore.Mvc;
using Nelmix.Models;
using Nelmix.Services;

namespace Nelmix.Controllers
{

    /// <summary>
    /// Controlador para operaciones relacionadas con Usuario.
    /// </summary>
    public class UserController : Controller
    {
        private readonly UserService usuarioService;

        /// <summary>
        /// Constructor del controlador UserController.
        /// </summary>
        public UserController()
        {
            usuarioService = new UserService();
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="oUsuario">Objeto de tipo Usuario que contiene los datos del usuario a registrar.</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPost("Register")]
        public IActionResult Register(Usuario oUsuario)
        {

            try
            {
                if (usuarioService.RegisterUser(oUsuario))
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
        public IActionResult Login(string email, string password) // Obtiene un objeto tipo UsuarioLogin que contiene email y contraseña
        {

            try
            {
                if (usuarioService.Login(email, password))
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
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
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
        public IActionResult ChangePassword(string email, string password, string newPassword) // Obtiene un objeto tipo CambiarContraseñaRequest
        {
            try
            {
                if (usuarioService.ChangePassword(email, password, newPassword))
                {
                    return Ok("Contraseña cambiada exitosamente.");
                }
                else
                {
                    return BadRequest("La contraseña actual es incorrecta. Por favor, inténtalo de nuevo.");
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
        /// <param name="nameAdult">Nombre del adulto responsable.  Ejemplo: john</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPut("AsignarAdulto")]
        public IActionResult AsignarAdultoResponsable(string mailUserMinor, string nameAdult)
        {
            try
            {
                (bool registrado, string mensaje) = usuarioService.AssignAdultResponsible(mailUserMinor, nameAdult);

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
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }

        }

        /// <summary>
        /// Desactiva un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario a desactivar. Ejemplo: 1</param>
        /// <returns>Un ActionResult que indica el resultado de la operación.</returns>
        [HttpPut("desactivar-usuario")]
        public IActionResult DesactivateUser(int userId)
        {
            try
            {
                usuarioService.ChangeUserStatusInactive(userId);
                return Ok("Usuario desactivado exitosamente.");
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }

        }
    }
}
