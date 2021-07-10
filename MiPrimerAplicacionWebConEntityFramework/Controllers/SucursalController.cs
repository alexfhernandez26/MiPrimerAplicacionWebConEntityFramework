using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class SucursalController : Controller
    {
        // GET: Sucursal
        List<SucursalCLS> listaSucursal = null;
        public ActionResult Index()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaSucursal = (from sucursal in bd.Sucursal
                                 where sucursal.BHABILITADO == 1
                                 select new SucursalCLS
                                 {
                                     iidsucursal = sucursal.IIDSUCURSAL,
                                     nombre = sucursal.NOMBRE,
                                     telefono =sucursal.TELEFONO,
                                     email =sucursal.EMAIL
                                 }).ToList();
            }
                return View(listaSucursal);
        }

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(SucursalCLS sucursalCLS)
        {
            
            if (!ModelState.IsValid)
            {
                return View(sucursalCLS);
            }
            else
            {
                using (var bd = new BDPasajeEntities())
                {
                    Sucursal sucursal = new Sucursal();

                    sucursal.NOMBRE = sucursalCLS.nombre;
                    sucursal.DIRECCION = sucursalCLS.direccion;
                    sucursal.TELEFONO = sucursalCLS.telefono;
                    sucursal.EMAIL = sucursalCLS.email;
                    sucursal.FECHAAPERTURA = sucursalCLS.fechaApertura;
                    sucursal.BHABILITADO = 1;
                    bd.Sucursal.Add(sucursal);
                    bd.SaveChanges();
                }
            }
           
            return RedirectToAction("Index");
        }
    }
}