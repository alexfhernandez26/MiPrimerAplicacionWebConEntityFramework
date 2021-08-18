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
            int numeroRegistrosEncontrados = 0;
            string nombresucursal = sucursalCLS.nombre;
            int id = sucursalCLS.iidsucursal;
            using (var bd = new BDPasajeEntities())
            {
                numeroRegistrosEncontrados = bd.Sucursal.Where(p => p.NOMBRE.Equals(nombresucursal)).Count();
            }

            if (!ModelState.IsValid || numeroRegistrosEncontrados >= 1)
            {
                if (numeroRegistrosEncontrados >= 1) sucursalCLS.mensajeError = "El nombre ya existe";
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
        public ActionResult Editar(int id)
        {
            SucursalCLS sucursalCLS = new SucursalCLS();
            using (var bd = new BDPasajeEntities())
            {
                Sucursal sucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(id)).First();
                sucursalCLS.iidsucursal= sucursal.IIDSUCURSAL;
                sucursalCLS.nombre = sucursal.NOMBRE;
                sucursalCLS.direccion = sucursal.DIRECCION;
                sucursalCLS.telefono = sucursal.TELEFONO;
                sucursalCLS.email = sucursal.EMAIL;
                sucursalCLS.fechaApertura = (DateTime)sucursal.FECHAAPERTURA;
            }
           
            return View(sucursalCLS);
        }

        [HttpPost]
        public ActionResult Editar(SucursalCLS sucursalCLS)
        {
            int numeroRegistrosEncontrados = 0;
            string nombresucursal = sucursalCLS.nombre;
            int id = sucursalCLS.iidsucursal;
            using (var bd = new BDPasajeEntities())
            {
                numeroRegistrosEncontrados = bd.Sucursal.Where(p => p.NOMBRE.Equals(nombresucursal) && !p.IIDSUCURSAL.Equals(id)).Count();
            }

            if (!ModelState.IsValid || numeroRegistrosEncontrados >= 1)
            {
                if (numeroRegistrosEncontrados >= 1) sucursalCLS.mensajeError = "El nombre ya existe";
            
                return View(sucursalCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                Sucursal sucursal = new Sucursal();
                sucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(sucursalCLS.iidsucursal)).First();
                sucursal.NOMBRE = sucursalCLS.nombre;
                sucursal.DIRECCION = sucursalCLS.direccion;
                sucursal.TELEFONO = sucursalCLS.telefono;
                sucursal.EMAIL= sucursalCLS.email;
                sucursal.FECHAAPERTURA = sucursalCLS.fechaApertura;
                bd.SaveChanges();
            }


                return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Sucursal sucursal = new Sucursal();
               sucursal= bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(id)).First();
               sucursal.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}