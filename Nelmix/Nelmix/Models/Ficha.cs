using System;
using System.Collections.Generic;

namespace Nelmix.Models
{
    /// <summary>
    /// Representa una ficha utilizada en el sistema.
    /// </summary>
    public partial class Ficha
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la ficha.
        /// </summary>
        public int FichaId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del tipo de ficha asociado.
        /// </summary>
        public int TipoFichaId { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad disponible de esta ficha.
        /// </summary>
        public int CantidadDisponible { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del usuario al que pertenece esta ficha.
        /// </summary>
        public int UsuarioId { get; set; }
    }
}
