namespace Nelmix.Models
{
    /// <summary>
    /// Representa un usuario en la aplicación.
    /// </summary>
    public partial class Usuario
    {
        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        /// <example>1</example>
        public int UserId { get; set; }

        /// <summary>
        /// nombre del usuario.
        /// </summary>
        /// <example>John</example>
        public string? Nombre { get; set; }

        /// <summary>
        /// edad del usuario.
        /// </summary>
        /// <example>30</example>
        public int? Edad { get; set; }

        /// <summary>
        /// correo electrónico del usuario.
        /// </summary>
        /// <example>john@gmail.com</example>
        public string? Email { get; set; }

        /// <summary>
        /// la contraseña del usuario.
        /// </summary>
        /// <example>1234</example>
        public string? Contraseña { get; set; }

        /// <summary>
        /// el identificador del estado del usuario (Si aplica).
        /// </summary>
        /// <example>1</example>
        public int? EstadoId { get; set; }

        /// <summary>
        /// el identificador del adulto asignado al usuario (Si aplica).
        /// </summary>
        /// <example>0</example>
        public int? AdultoAsignadoId { get; set; }
    }
}
