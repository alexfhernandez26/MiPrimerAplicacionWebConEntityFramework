using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        List<ClienteCLS> listaCliente = null;
        public ActionResult Index()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaCliente = (from cliente in bd.Cliente
                              select new ClienteCLS
                              {
                                  iidCliente = cliente.IIDCLIENTE,
                                  NOMBRE = cliente.NOMBRE,
                                  APPATERNO = cliente.APPATERNO,
                                  APMATERNO = cliente.APMATERNO,
                                  TELEFONOCELULAR = cliente.TELEFONOCELULAR
                              }).ToList();
            }

            return View(listaCliente);
        }
    }
}