using System.ComponentModel.DataAnnotations.Schema;

namespace Nelmix.Models
{
    public class GananciasYPérdidasPorJuego
    {
        [Column("nombre_juego")]
        public string NombreJuego { get; set; }

        public decimal Ganancias { get; set; }

        public decimal Pérdidas { get; set; }
    }
}
