namespace Nelmix.Models
{
    public partial class TasasDeCambio
    {
        public int TasaId { get; set; }

        public int MonedaId { get; set; }

        public decimal Tasa { get; set; }

        public virtual Moneda? Moneda { get; set; }
    }
}
