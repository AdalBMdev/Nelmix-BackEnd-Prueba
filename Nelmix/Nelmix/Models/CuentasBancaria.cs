using System;
using System.Collections.Generic;

namespace Nelmix.Models
{

    public partial class CuentasBancaria
    {
        public int CuentaId { get; set; }
        public int UserId { get; set; }
        public int MonedaId { get; set; }
        public decimal Saldo { get; set; }
    }
}

