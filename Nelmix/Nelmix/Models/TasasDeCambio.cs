namespace Nelmix.Models
{
    /// <summary>
    /// Representa una tasa de cambio entre dos monedas en la aplicación.
    /// </summary>
    public partial class TasasDeCambio
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la tasa de cambio.
        /// </summary>
        public int TasaId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la moneda asociada a la tasa de cambio.
        /// </summary>
        public int? MonedaId { get; set; }

        /// <summary>
        /// Obtiene o establece el valor de la tasa de cambio entre monedas.
        /// </summary>
        public decimal? Tasa { get; set; }

        /// <summary>
        /// Obtiene o establece la moneda asociada a la tasa de cambio.
        /// </summary>
        public virtual Moneda? Moneda { get; set; }
    }
}
