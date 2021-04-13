using System;
using System.Collections.Generic;

#nullable disable

namespace GDBTransactionsMySQL.DAL
{
    public partial class Remito
    {
        public Remito()
        {
            RemitosDetalles = new HashSet<RemitosDetalle>();
        }

        public int IdRemito { get; set; }
        public DateTime? Fecha { get; set; }
        public int IdCliente { get; set; }
        public decimal? MontoTotal { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual ICollection<RemitosDetalle> RemitosDetalles { get; set; }
    }
}
