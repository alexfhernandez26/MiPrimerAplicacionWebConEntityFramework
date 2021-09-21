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
            llenarRol();
            llenarPagina();
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

        public ActionResult Filtrar(int? iidrolFiltro)
        {
            List<RolPaginaCLS> listaRpagina = null;
            using (var bd = new BDPasajeEntities())
            {
                
                if (iidrolFiltro == null)
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
                                    && rolpagina.IIDROL == iidrolFiltro
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

        public string Guardar(RolPaginaCLS rolPaginaCLS, int titulo)
        {
            
            string respuesta = "";
            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                respuesta += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    respuesta += "<li class='list-group-item'>" + item + "</li>";
                }

                return respuesta += "</ul>";
            }
            else
            {
                using (var bd = new BDPasajeEntities())
                {
                    if (titulo == -1)
                    {
                        RolPagina rolPagina = new RolPagina();
                        rolPagina.IIDROL = rolPaginaCLS.iidrol;
                        rolPagina.IIDPAGINA = rolPaginaCLS.iidpagina;
                        rolPagina.BHABILITADO = 1;
                        bd.RolPagina.Add(rolPagina);
                        respuesta = bd.SaveChanges().ToString();
                    }
                    else
                    {
                        RolPagina rolpagina = bd.RolPagina.Where(p => p.IIDROLPAGINA.Equals(titulo)).First();
                        rolpagina.IIDROL = rolPaginaCLS.iidrol;
                        rolpagina.IIDPAGINA = rolPaginaCLS.iidpagina;
                        respuesta = bd.SaveChanges().ToString();
                     }


                }
            }
               

                return respuesta;
        }

        public JsonResult RecuperarDatos(int parametro)
        {
            RolPaginaCLS rolPaginaCLS = new RolPaginaCLS();
            using (var bd = new BDPasajeEntities())
            {
                RolPagina rolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA.Equals(parametro)).First();
                rolPaginaCLS.iidrol = (int)rolPagina.IIDROL;
                rolPaginaCLS.iidpagina = (int)rolPagina.IIDPAGINA;

            }

                return Json(rolPaginaCLS,JsonRequestBehavior.AllowGet);
        }
        public void llenarRol()
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
        public void llenarPagina()
        {
            List<SelectListItem> listapagina = null;
            using (var bd = new BDPasajeEntities())
            {
                listapagina = (from pagina in bd.Pagina
                               select new SelectListItem
                               {
                                   Text = pagina.MENSAJE,
                                   Value = pagina.IIDPAGINA.ToString()
                               }).ToList();
                listapagina.Insert(0, new SelectListItem { Text = "--Seleccione", Value = "" });
                ViewBag.listapagina = listapagina;
            }
        }

    }
}