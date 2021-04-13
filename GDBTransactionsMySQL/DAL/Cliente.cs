using System;
using System.Collections.Generic;

#nullable disable

namespace GDBTransactionsMySQL.DAL
{
    public partial class Cliente
    {
        public Cliente()
        {
            Remitos = new HashSet<Remito>();
        }

        public int IdCliente { get; set; }
        public string NombreApellido { get; set; }
        public int? Dni { get; set; }
        public string Telefono { get; set; }

        public virtual ICollection<Remito> Remitos { get; set; }
    }
}
