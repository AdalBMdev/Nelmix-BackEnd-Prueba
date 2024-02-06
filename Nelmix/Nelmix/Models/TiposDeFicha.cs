namespace Nelmix.Models
{
    /// <summary>
    /// Representa un tipo de ficha utilizado en la aplicación.
    /// </summary>
    public partial class TiposDeFicha
    {
        /// <summary>
        /// Obtiene o establece el identificador único del tipo de ficha.
        /// </summary>
        public int TipoFichaId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del tipo de ficha.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el valor asociado al tipo de ficha.
        /// </summary>
        public int Valor { get; set; }
    }
}
