using System.ComponentModel.DataAnnotations.Schema;

namespace Nelmix.Models
{
    /// <summary>
    /// Representa las ganancias y pérdidas por juego de un usuario.
    /// </summary>
    public class GananciasYPérdidasPorJuego
    {
        /// <summary>
        /// Obtiene o establece el nombre del juego.
        /// </summary>
        [Column("nombre_juego")]
        public string NombreJuego { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de ganancias en el juego.
        /// </summary>
        public decimal Ganancias { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de pérdidas en el juego.
        /// </summary>
        public decimal Pérdidas { get; set; }
    }
}
