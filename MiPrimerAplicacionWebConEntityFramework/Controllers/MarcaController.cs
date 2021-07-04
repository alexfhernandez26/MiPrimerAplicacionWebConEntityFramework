using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class MarcaController : Controller
    {
        // GET: Marca
        public ActionResult Index()
        {
            List<MarcaCLS> listamarca = null;
            using (var bd = new BDPasajeEntities())
            {
                listamarca = (from marca in bd.Marca
                              select new MarcaCLS
                              {
                                  iidmarca = marca.IIDMARCA,
                                  nombre = marca.NOMBRE,
                                  descripcion = marca.DESCRIPCION
                              }).ToList();
            }
                return View(listamarca);
        }

        public ActionResult Agregar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Agregar(MarcaCLS marcaCLS)
        {
            if (!ModelState.IsValid)
            {
                return View(marcaCLS);
            }
            else
            {
                using (var bd = new BDPasajeEntities())
                {
                    Marca marca = new Marca();
                    marca.NOMBRE = marcaCLS.nombre;
                    marca.DESCRIPCION= marcaCLS.descripcion;
                    marca.BHABILITADO = 1;
                    bd.Marca.Add(marca);
                    bd.SaveChanges();
                }
            }
            return RedirectToAction("index");
        }
    }
}