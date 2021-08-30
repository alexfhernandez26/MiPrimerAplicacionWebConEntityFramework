using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class RolPaginaController : Controller
    {
        // GET: RolPagina
        public ActionResult Index()
        {
            llenarRolPagina();
            List<RolPaginaCLS> listaRpagina = null;
            using (var bd = new BDPasajeEntities())
            {
                listaRpagina = (from rolpagina in bd.RolPagina
                                join rol in bd.Rol
                                on rolpagina.IIDROL equals rol.IIDROL
                                join pagina in bd.Pagina
                                on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                                where rolpagina.BHABILITADO == 1
                                select new RolPaginaCLS
                                {
                                    iidrolpagina = rolpagina.IIDROLPAGINA,
                                    nombrerol= rol.NOMBRE,
                                     nombrepagina = pagina.MENSAJE
                                }).ToList();
            }
                return View(listaRpagina);
        }

        public ActionResult Filtrar(RolPaginaCLS rolPaginaCLS)
        {
            List<RolPaginaCLS> listaRpagina = null;
            using (var bd = new BDPasajeEntities())
            {
                
                if (rolPaginaCLS.iidrol == 0)
                {
                    listaRpagina = (from rolpagina in bd.RolPagina
                                    join rol in bd.Rol
                                    on rolpagina.IIDROL equals rol.IIDROL
                                    join pagina in bd.Pagina
                                    on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                                    where rolpagina.BHABILITADO == 1
                                    select new RolPaginaCLS
                                    {
                                        iidrolpagina = rolpagina.IIDROLPAGINA,
                                        nombrerol = rol.NOMBRE,
                                        nombrepagina = pagina.MENSAJE
                                    }).ToList();
                }
                else
                {
                    listaRpagina = (from rolpagina in bd.RolPagina
                                    join rol in bd.Rol
                                    on rolpagina.IIDROL equals rol.IIDROL
                                    join pagina in bd.Pagina
                                    on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                                    where rolpagina.BHABILITADO == 1
                                    && rol.IIDROL.Equals(rolPaginaCLS.iidrol)
                                    select new RolPaginaCLS
                                    {
                                        iidrolpagina = rolpagina.IIDROLPAGINA,
                                        nombrerol = rol.NOMBRE,
                                        nombrepagina = pagina.MENSAJE
                                    }).ToList();
                }
            }
                
            return PartialView("_tablaRolPagina",listaRpagina);
        }
        public void llenarRolPagina()
        {
            List<SelectListItem> listarolP = null;
            using (var bd = new BDPasajeEntities())
            {
                listarolP = (from rol in bd.Rol
                             select new SelectListItem
                             {
                                 Text = rol.NOMBRE,
                                 Value = rol.IIDROL.ToString()
                             }).ToList();
                listarolP.Insert(0, new SelectListItem { Text = "--Seleccione", Value = "" });
                ViewBag.listaRol = listarolP;
            }
        }
    }
}