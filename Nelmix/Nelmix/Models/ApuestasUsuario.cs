using System;
using System.Collections.Generic;

namespace Nelmix.Models
{

    public partial class ApuestasUsuario
    {
        public int ApuestaId { get; set; }
        public int UsuarioId { get; set; }
        public int JuegoId { get; set; }
        public decimal CantidadGanada { get; set; }
        public decimal CantidadPerdida { get; set; }
    }
}

