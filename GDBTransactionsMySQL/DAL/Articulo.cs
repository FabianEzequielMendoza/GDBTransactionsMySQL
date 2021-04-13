using System;
using System.Collections.Generic;

#nullable disable

namespace GDBTransactionsMySQL.DAL
{
    public partial class Articulo
    {
        public Articulo()
        {
            RemitosDetalles = new HashSet<RemitosDetalle>();
        }

        public int IdArticulo { get; set; }
        public string Articulo1 { get; set; }
        public int? Precio { get; set; }

        public virtual ICollection<RemitosDetalle> RemitosDetalles { get; set; }
    }
}
