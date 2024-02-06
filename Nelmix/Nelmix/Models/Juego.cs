namespace Nelmix.Models
{
    /// <summary>
    /// Representa un juego en la aplicación.
    /// </summary>
    public partial class Juego
    {
        /// <summary>
        /// Obtiene o establece el identificador único del juego.
        /// </summary>
        public int JuegoId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del juego.
        /// </summary>
        public string Nombre { get; set; }
    }
}
