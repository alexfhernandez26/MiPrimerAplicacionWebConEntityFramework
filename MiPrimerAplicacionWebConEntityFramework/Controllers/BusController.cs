using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class BusController : Controller
    {
        // GET: Bus
        List<BusCLS> listaBus = null;
        public ActionResult Index()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaBus = (from bus in bd.Bus
                            join sucursal in bd.Sucursal
                            on bus.IIDSUCURSAL equals sucursal.IIDSUCURSAL
                            join tipobus in bd.TipoBus
                            on bus.IIDTIPOBUS equals tipobus.IIDTIPOBUS
                            join modelo in bd.Modelo
                            on bus.IIDMODELO equals modelo.IIDMODELO
                            join marca in bd.Marca
                            on bus.IIDMARCA equals marca.IIDMARCA
                            select new BusCLS
                            {
                                iidbus = bus.IIDBUS,
                                NombreSucursal = sucursal.NOMBRE,
                                NombreTipoBus = tipobus.NOMBRE,
                                placa = bus.PLACA,
                                fechacompra = (DateTime)bus.FECHACOMPRA,
                                NombreModelo = modelo.NOMBRE,
                                NombreMarca = marca.NOMBRE

                            }).ToList();
            }
               

            return View(listaBus);
        }
    }
}