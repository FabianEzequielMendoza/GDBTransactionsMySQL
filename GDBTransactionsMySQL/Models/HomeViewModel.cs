using GDBTransactionsMySQL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GDBTransactionsMySQL.Models
{
    public class HomeViewModel
    {
        public List<Cliente> clientes { get; set; }
        public List<Articulo> articulos { get; set; }
        public int selectedClienteId { get; set; }
        public int selectedProductoId { get; set; }
        public int precioUnidad { get; set; }
        public decimal precioTotal { get; set; }
        public string Error { get; set; }
        public bool isCorrect { get; set; }
      
    }
}
