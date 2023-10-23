using System;
using System.Collections.Generic;

namespace Nelmix.Models
{
    /// <summary>
    /// Representa una cuenta bancaria asociada a un usuario en una moneda específica.
    /// </summary>
    public partial class CuentasBancaria
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la cuenta bancaria.
        /// </summary>
        /// <example>1</example>
        public int CuentaId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del usuario al que está asociada la cuenta bancaria.
        /// </summary>
        /// <example>3</example>
        public int? UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la moneda en la que se mantiene el saldo de la cuenta.
        /// </summary>
        /// <example>2</example>
        public int? MonedaId { get; set; }

        /// <summary>
        /// Obtiene o establece el saldo actual de la cuenta bancaria en la moneda especificada.
        /// </summary>
        /// <example>1500.50</example>
        public decimal? Saldo { get; set; }
    }
}

