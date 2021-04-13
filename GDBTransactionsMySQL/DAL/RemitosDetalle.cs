using System;
using System.Collections.Generic;

#nullable disable

namespace GDBTransactionsMySQL.DAL
{
    public partial class RemitosDetalle
    {
        public int IdDetalle { get; set; }
        public int IdRemito { get; set; }
        public int IdArticulo { get; set; }
        public int? Cantidad { get; set; }
        public int? MontoItem { get; set; }

        public virtual Articulo IdArticuloNavigation { get; set; }
        public virtual Remito IdRemitoNavigation { get; set; }
    }
}
