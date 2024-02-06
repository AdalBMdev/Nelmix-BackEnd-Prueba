using System;
using System.Collections.Generic;

namespace Nelmix.Models
{
   
    public partial class Ficha
    {
        public int FichaId { get; set; }

        public int TipoFichaId { get; set; }

        public int CantidadDisponible { get; set; }

        public int UsuarioId { get; set; }
    }
}
