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

        public ActionResult Editar( int id)
        {
            MarcaCLS marcaCLS = new MarcaCLS();

            using (var bd = new BDPasajeEntities())
            {
                Marca marca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                marcaCLS.iidmarca = marca.IIDMARCA;
                marcaCLS.nombre = marca.NOMBRE;
                marcaCLS.descripcion= marca.DESCRIPCION;
            }

                return View(marcaCLS);
        }
        [HttpPost]
        public ActionResult Editar(MarcaCLS marcaCLS)
        {
            Marca marca = new Marca();

            if (!ModelState.IsValid)
            {
                return View(marcaCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                marca = bd.Marca.Where(p => p.IIDMARCA.Equals(marcaCLS.iidmarca)).First();
                marca.NOMBRE = marcaCLS.nombre;
                marca.DESCRIPCION = marcaCLS.descripcion;
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
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