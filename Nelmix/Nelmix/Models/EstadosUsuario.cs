using System;
using System.Collections.Generic;

namespace Nelmix.Models
{
    /// <summary>
    /// Representa un estado asociado a un usuario.
    /// </summary>
    public partial class EstadosUsuario
    {
        /// <summary>
        /// Obtiene o establece el identificador único del estado.
        /// </summary>
        public int EstadoId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del estado.
        /// </summary>
        public string? Nombre { get; set; }
    }
}
