using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class RolController : Controller
    {
        // GET: Rol
        public ActionResult Index()
        {
            List<RolCLS> listaRol = null;
            using (var bd = new BDPasajeEntities())
            {
                listaRol = (from rol in bd.Rol
                            where rol.BHABILITADO == 1
                            select new RolCLS
                            {
                                iidrol = rol.IIDROL,
                                nombre = rol.NOMBRE,
                                descripcion = rol.DESCRIPCION
                            }).ToList();
            }
                return View(listaRol);
        }
    }
}