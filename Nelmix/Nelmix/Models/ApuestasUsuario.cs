using System;
using System.Collections.Generic;

namespace Nelmix.Models
{
    /// <summary>
    /// Representa una apuesta realizada por un usuario en un juego específico.
    /// </summary>
    public partial class ApuestasUsuario
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la apuesta.
        /// </summary>
        public int ApuestaId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del usuario que realizó la apuesta.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del juego en el que se realizó la apuesta.
        /// </summary>
        public int JuegoId { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad ganada en la apuesta. Puede ser nulo si no hubo ganancias.
        /// </summary>
        public decimal CantidadGanada { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad perdida en la apuesta. Puede ser nulo si no hubo pérdidas.
        /// </summary>
        public decimal CantidadPerdida { get; set; }
    }
}

