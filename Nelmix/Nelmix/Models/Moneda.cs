namespace Nelmix.Models
{
    /// <summary>
    /// Representa una moneda en la aplicación.
    /// </summary>
    public partial class Moneda
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Moneda"/>.
        /// </summary>
        public Moneda()
        {
            TasasDeCambios = new HashSet<TasasDeCambio>();
        }

        /// <summary>
        /// Obtiene o establece el identificador único de la moneda.
        /// </summary>
        public int MonedaId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la moneda.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el símbolo de la moneda.
        /// </summary>
        public string Símbolo { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de tasas de cambio asociadas a esta moneda.
        /// </summary>
        public virtual ICollection<TasasDeCambio> TasasDeCambios { get; set; }
    }
}
