using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Nelmix.Models;

namespace Nelmix.Services
{
    /// <summary>
    /// Clase que gestiona operaciones relacionadas con usuarios, como el registro, inicio de sesión y cambio de contraseña.
    /// </summary>
    /// 
    public class UserService
    {
        
        /// <summary>
        /// Registra un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="usuario">Objeto Usuario que contiene los datos del usuario a registrar.</param>
        /// <returns>True si el registro fue exitoso, de lo contrario, False.</returns>
        public bool RegisterUser(Usuario usuario)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();
                    usuario.Contraseña = ConvertSha256(usuario.Contraseña);

                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@edad", usuario.Edad);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);
                    cmd.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                    cmd.Parameters.AddWithValue("@estado_id", 1);
                    cmd.Parameters.AddWithValue("@adulto_asignado_id", 0);

                    SqlParameter Registrado = new SqlParameter("@Registrado", SqlDbType.Bit);
                    Registrado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(Registrado);

                    SqlParameter Mensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 100);
                    Mensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(Mensaje);


                    cmd.ExecuteNonQuery();

                    bool register = Convert.ToBoolean(Registrado.Value);
                    return register;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el usuario.", ex);
            }
        }

        /// <summary>
        /// Inicia sesión de un usuario utilizando su correo electrónico y contraseña.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>True si la sesión se inicia con éxito, de lo contrario, False.</returns>
        public bool Login(string email, string password)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();

                    password = ConvertSha256(password);

                    SqlCommand cmd = new SqlCommand("sp_IniciarSesionUsuario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@contraseña", password);

                    SqlParameter UsuarioEncontrado = new SqlParameter("@UsuarioEncontrado", SqlDbType.Bit);
                    UsuarioEncontrado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(UsuarioEncontrado);

                    SqlParameter Mensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 100);
                    Mensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(Mensaje);

                    cmd.ExecuteNonQuery();

                    bool login = Convert.ToBoolean(UsuarioEncontrado.Value);
                    return login;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesión del usuario.", ex);
            }
        }

        /// <summary>
        /// Cambia la contraseña de un usuario.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña actual del usuario.</param>
        /// <param name="newPassword">Nueva contraseña que se asignará al usuario.</param>
        /// <returns>True si la contraseña se cambia con éxito, de lo contrario, False.</returns>
        public bool ChangePassword(string email, string password, string newPassword)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();
                    password = ConvertSha256(password);
                    newPassword = ConvertSha256(newPassword);

                    SqlCommand cmd = new SqlCommand("sp_CambiarContraseña", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@contraseñaActual", password);
                    cmd.Parameters.AddWithValue("@nuevaContraseña", newPassword);

                    SqlParameter ChangePassword = new SqlParameter("@ContraseñaCambiada", SqlDbType.Bit);
                    ChangePassword.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(ChangePassword);


                    SqlParameter Mensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 100);
                    Mensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(Mensaje);

                cmd.ExecuteNonQuery();

                    bool changePassword = Convert.ToBoolean(ChangePassword.Value);
                    return changePassword;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar la contraseña del usuario.", ex);
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
            try
            {
                bool register = false;
                string message = "";

                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    SqlCommand cmd = new SqlCommand("sp_AsignarAdultoResponsable", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CorreoUsuarioMenor", mailUserMinor);
                    cmd.Parameters.AddWithValue("@NombreAdulto", nameAdult);

                    SqlParameter Registrado = new SqlParameter("@Registrado", SqlDbType.Bit);
                    Registrado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(Registrado);

                    SqlParameter Mensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 100);
                    Mensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(Mensaje);

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    register = Convert.ToBoolean(Registrado.Value);
                    message = Mensaje.Value.ToString();
                }

                return (register, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al asignar adulto responsable.", ex);
            }
        }
        
        /// <summary>
        /// Cambia el estado de un usuario a inactivo.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        public void ChangeUserStatusInactive(int userId)
        {
            try
            {
                var chain = new Connection();

                using (SqlConnection cn = new SqlConnection(chain.getCadenaSQL()))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("CambiarEstadoUsuarioAInactivo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@usuario_id", userId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar el estado del usuario a inactivo.", ex);
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
