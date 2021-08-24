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
        public ActionResult Index(MarcaCLS marcaCLS)
        {
            List<MarcaCLS> listamarca = null;
            string nombre = marcaCLS.nombre;
            using (var bd = new BDPasajeEntities())
            {
                if (marcaCLS.nombre == null)
                {
                    listamarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS
                                  {
                                      iidmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                }
                else
                {
                    listamarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  && marca.NOMBRE.Contains(nombre)
                                  select new MarcaCLS
                                  {
                                      iidmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                }
               
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
            string nombreMarca = marcaCLS.nombre;
            int numeroRepite = 0;
         
            using (var bd = new BDPasajeEntities())
            {
                numeroRepite = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca)).Count();
            }
                if (!ModelState.IsValid || numeroRepite >=1)
                {
                if (numeroRepite >= 1) marcaCLS.mensajeError = "El nombre que esta ingresando ya existe";
                    return View(marcaCLS);
                }
                else
                {
                    using (var bd = new BDPasajeEntities())
                    {
                        Marca marca = new Marca();
                        marca.NOMBRE = marcaCLS.nombre;
                        marca.DESCRIPCION = marcaCLS.descripcion;
                        marca.BHABILITADO = 1;
                        bd.Marca.Add(marca);
                        bd.SaveChanges();
                    }
                }
            return RedirectToAction("index");
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

            int numeroRegistrosEncontrados =0;
            string nombremarca = marcaCLS.nombre;
            int id = marcaCLS.iidmarca;
            using (var bd = new BDPasajeEntities())
            {
                numeroRegistrosEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombremarca) && !p.IIDMARCA.Equals(id)).Count();
            }

                if (!ModelState.IsValid || numeroRegistrosEncontrados >=1)
                {
                if (numeroRegistrosEncontrados >= 1) marcaCLS.mensajeError = "El nombre ya existe";
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

        

        public ActionResult Eliminar(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Marca marca = new Marca();
                marca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                marca.BHABILITADO = 0;
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }
    }
}