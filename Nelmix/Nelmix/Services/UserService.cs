using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using Nelmix.Interfaces;
using Nelmix.Models;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Services
{

    public class UserService : IUserService
    {
         private readonly CasinoContext _context;

        public UserService(CasinoContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Registra un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="usuario">Objeto Usuario que contiene los datos del usuario a registrar.</param>
        /// <returns>True si el registro fue exitoso, de lo contrario, False.</returns>
        public async Task RegisterUser(RegisterUserRequestDto usuario)
        {
                usuario.Password = ConvertSha256(usuario.Password);

                var newUser = new Usuario
                {
                    Nombre = usuario.Name,
                    Edad = usuario.Age,
                    Email = usuario.Email,
                    Contraseña = usuario.Password,
                    EstadoId = 1,
                    AdultoAsignadoId = 0
                };

                _context.Usuarios.Add(newUser);
                await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Inicia sesión de un usuario utilizando su correo electrónico y contraseña.
        /// </summary>
        /// <param name="usuario">Objeto Usuario que contiene los datos del usuario a login.</param>
        /// <returns>True si la sesión se inicia con éxito, de lo contrario, False.</returns>
        public async Task<bool> Login(LoginUserRequestDto usuario)
        {
            usuario.Password = ConvertSha256(usuario.Password);

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Contraseña == usuario.Password);

            return user != null;
        }


        /// <summary>
        /// Cambia la contraseña de un usuario.
        /// </summary>
        /// <param name="changePasswordUsuario">Un objeto con informacion para cambiar contraseña</param>
        /// <returns>True si la contraseña se cambia con éxito, de lo contrario, False.</returns>
        public async Task ChangePassword(ChangePasswordRequestDto changePasswordUsuario)
        {
            changePasswordUsuario.Password = ConvertSha256(changePasswordUsuario.Password);
            changePasswordUsuario.NewPassword = ConvertSha256(changePasswordUsuario.NewPassword);

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == changePasswordUsuario.Email && u.Contraseña == changePasswordUsuario.Password);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            user.Contraseña = changePasswordUsuario.NewPassword;
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Asigna un adulto responsable a un usuario menor.
        /// </summary>
        /// <param name="usersEmails">Un objeto con 2 emails, el email del usuario mayor y menor</param>
        /// <returns>Una tupla que contiene un valor booleano (True si la asignación fue exitosa) y un mensaje de texto.</returns>
        public async Task AssignAdultResponsible(AssignAdultResponsableRequestDto usersEmails)
        {

            var userMinor = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usersEmails.MailUserMinor);       

            var adult = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usersEmails.MailUserAdult);

            userMinor.AdultoAsignadoId = adult.UserId;

            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Cambia el estado de un usuario a inactivo.
        /// </summary>
        /// <param name="usuario">Identificador del usuario.</param>
        public async Task ChangeUserStatusInactiveAsync(DesactivateUserRequestDto usuario)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.UserId == usuario.UserId);

            user.EstadoId = 0;
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Convierte una cadena en un valor hash SHA-256.
        /// </summary>
        /// <param name="inputString">Cadena de entrada que se convertirá en hash.</param>
        /// <returns>El hash SHA-256 de la cadena de entrada.</returns>
        private string ConvertSha256(string inputString)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
