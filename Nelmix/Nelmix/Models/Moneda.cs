namespace Nelmix.Models
{
    
    public partial class Moneda
    {
        public Moneda()
        {
            TasasDeCambios = new HashSet<TasasDeCambio>();
        }

        public int MonedaId { get; set; }

        public string Nombre { get; set; }

        public string Símbolo { get; set; }

        public virtual ICollection<TasasDeCambio> TasasDeCambios { get; set; }
    }
}
