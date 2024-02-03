using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nelmix.Context;
using Nelmix.Models;

namespace Nelmix.Services
{
    /// <summary>
    /// Clase que gestiona operaciones relacionadas con usuarios, como el registro, inicio de sesión y cambio de contraseña.
    /// </summary>
    /// 
    public class UserService
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
        public async Task<bool> RegisterUser(Usuario usuario)
        {
                usuario.Contraseña = ConvertSha256(usuario.Contraseña);

                bool emailExists = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);

                if (!emailExists)
                {
                    var newUser = new Usuario
                    {
                        Nombre = usuario.Nombre,
                        Edad = usuario.Edad,
                        Email = usuario.Email,
                        Contraseña = usuario.Contraseña,
                        EstadoId = 1,
                        AdultoAsignadoId = 0
                    };

                    _context.Usuarios.Add(newUser);
                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            }

        /// <summary>
        /// Inicia sesión de un usuario utilizando su correo electrónico y contraseña.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>True si la sesión se inicia con éxito, de lo contrario, False.</returns>
        public bool Login(string email, string password)
        {

            password = ConvertSha256(password);

            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Contraseña == password);

            return user != null; // Si se encuentra el usuario, retorna true; de lo contrario, retorna false
        }

        /// <summary>
        /// Cambia la contraseña de un usuario.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña actual del usuario.</param>
        /// <param name="newPassword">Nueva contraseña que se asignará al usuario.</param>
        /// <returns>True si la contraseña se cambia con éxito, de lo contrario, False.</returns>
        public async Task<(bool, string)> ChangePassword(string email, string password, string newPassword)
        {
            bool success = false;
            string message = "";

            password = ConvertSha256(password);
            newPassword = ConvertSha256(newPassword);

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Contraseña == password);

            if (user != null)
            {
                if (password == newPassword)
                {
                    return (success, message = "La contraseña es igual a la anterior");
                }

                user.Contraseña = newPassword;

                await _context.SaveChangesAsync();

                return (success = true, message);
            }
            else
            {
                return (success, message = "No se ha podido cambiar la contraseña");
            }
        }


        /// <summary>
        /// Asigna un adulto responsable a un usuario menor.
        /// </summary>
        /// <param name="mailUserMinor">Correo electrónico del usuario menor.</param>
        /// <param name="nameAdult">Nombre del adulto responsable.</param>
        /// <returns>Una tupla que contiene un valor booleano (True si la asignación fue exitosa) y un mensaje de texto.</returns>
        public (bool, string) AssignAdultResponsible(string mailUserMinor, string nameAdult)
        {
            bool register = false;
            string message = "Se ha asignado el adulto correctamente";

            var userMinor = _context.Usuarios.FirstOrDefault(u => u.Email == mailUserMinor);

            if (userMinor == null || userMinor.Edad >= 21)
            {
                message = "El usuario menor no existe o es mayor de edad.";
                return (register, message);
            }

            var adult = _context.Usuarios.FirstOrDefault(u => u.Nombre == nameAdult);

            if (adult == null || adult.Edad < 21)
            {
                message = "El adulto responsable no existe o es menor de edad.";
                return (register, message);

            }

            userMinor.AdultoAsignadoId = adult.UserId;

            _context.SaveChanges();

            return (register = true, message);
        }
        
        /// <summary>
        /// Cambia el estado de un usuario a inactivo.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        public void ChangeUserStatusInactive(int userId)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                user.EstadoId = 0;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Usuario no encontrado.");
            }
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
