using GDBTransactionsMySQL.DAL;
using GDBTransactionsMySQL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GDBTransactionsMySQL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly remitosContext _remitoscontext;
        private readonly string myConnectionString;
        public HomeController(ILogger<HomeController> logger, remitosContext remitosContext,IConfiguration Configuration)
        {
            _logger = logger;
            _remitoscontext = remitosContext;
            myConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();

            try
            {

                model.clientes = _remitoscontext.Clientes.ToList();
                    
                model.articulos = _remitoscontext.Articulos.ToList();
               
                
            }
            catch (Exception)
            {
                model.Error = "Se ha producido un error inesperado al cargar los datos";

                return View(model);
            }


            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public JsonResult GetPrecioArticulo(int IdArticulo)
        {
            var model = new HomeViewModel();

            try
            {

                model.precioUnidad = (int)_remitoscontext.Articulos.Where(a => a.IdArticulo == IdArticulo).Select(a => a.Precio).FirstOrDefault();
             
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Message: {0}", ex.Message);

            }
            return Json(model);
        }

        public JsonResult Save(int IdArticulo, int cantidadUnidad, int IdCliente)
        {
            int montoItem=0;
            decimal montoTotal=0;

            var model = new HomeViewModel();

            try
            {

                montoItem = (int)_remitoscontext.Articulos.Where(a => a.IdArticulo == IdArticulo).Select(a => a.Precio).FirstOrDefault();
               
            }

            catch (Exception ex)
            {
                Console.WriteLine("  Message: {0}", ex.Message);

            }
           
            montoTotal = montoItem * cantidadUnidad;

            if (ExecuteSqlTransaction(IdCliente, montoTotal, IdArticulo, cantidadUnidad, montoItem))
            {
                model.isCorrect = true;
                model.precioTotal = montoTotal;
            }
            else
            {
                model.isCorrect = false;
            }
            
            return Json(model);
            
        }

        private bool ExecuteSqlTransaction(int idCliente,decimal montoTotal, int idArticulo, int cantidad, int montoItem)
        {
            
            MySqlConnection myConnection = new MySqlConnection(this.myConnectionString);
            myConnection.Open();

            MySqlCommand myCommand = myConnection.CreateCommand();
            MySqlTransaction myTrans;

            // Esta en repeteable read
            myTrans = myConnection.BeginTransaction();
     
            myCommand.Connection = myConnection;
            myCommand.Transaction = myTrans;

            try
            {
                myCommand.CommandText = "INSERT INTO remitos (FECHA,ID_CLIENTE,MONTO_TOTAL) VALUES (@fecha,@idCliente,@montoTotal)";
                myCommand.Parameters.Add("@fecha", MySqlDbType.DateTime).Value = DateTime.Now;
                myCommand.Parameters.Add("@idCliente", MySqlDbType.Int24).Value=idCliente;
                myCommand.Parameters.Add("@montoTotal", MySqlDbType.Decimal).Value=montoTotal;
                myCommand.ExecuteNonQuery();

                myCommand.CommandText = "SELECT last_insert_id() from remitos";

                int lastId = Convert.ToInt32(myCommand.ExecuteScalar());

                myCommand.CommandText = "INSERT INTO remitos_detalle (ID_REMITO,ID_ARTICULO,CANTIDAD,MONTO_ITEM) VALUES (@idRemito,@idArticulo,@cantidad,@montoItem)";
                myCommand.Parameters.Add("@idRemito", MySqlDbType.Int24).Value = lastId;
                myCommand.Parameters.Add("@idArticulo", MySqlDbType.Int24).Value = idArticulo;
                myCommand.Parameters.Add("@cantidad", MySqlDbType.Int24).Value = cantidad;
                myCommand.Parameters.Add("@montoItem", MySqlDbType.Int24).Value = montoItem;
                myCommand.ExecuteNonQuery();

                myTrans.Commit();
                Console.WriteLine("Both records are written to database.");
            }
            catch (Exception e)
            { 
                try
                {
                    myTrans.Rollback();
                }
                catch (Exception ex)
                {
                    if (myTrans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                        " was encountered while attempting to roll back the transaction.");
                    }
                    
                }

                Console.WriteLine("An exception of type " + e.GetType() +
                " was encountered while inserting the data.");
                Console.WriteLine("Neither record was written to database.");

                return false;
            }
            finally
            {
                myConnection.Close();
                
            }

            return true;

        }
    }
}