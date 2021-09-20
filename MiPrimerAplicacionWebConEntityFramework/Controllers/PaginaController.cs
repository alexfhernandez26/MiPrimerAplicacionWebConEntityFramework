using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class PaginaController : Controller
    {
        // GET: Pagina
        public ActionResult Index()
        {
            List<PaginaCLS> listaPagina = null;

            using (var bd = new BDPasajeEntities())
            {
                listaPagina = (from pagina in bd.Pagina
                               where pagina.BHABILITADO == 1
                               select new PaginaCLS
                               {
                                   iidpagina= pagina.IIDPAGINA,
                                   mensaje = pagina.MENSAJE,
                                   accion = pagina.ACCION,
                                   controlador = pagina.CONTROLADOR
                               }).ToList();
            }
                return View(listaPagina);
        }

        public ActionResult Filtrar(PaginaCLS paginaCLS)
        {
            List<PaginaCLS> listaPagina = null;

            using (var bd = new BDPasajeEntities())
            {
                if (paginaCLS.mensajePagina ==null)
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   select new PaginaCLS
                                   {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       accion = pagina.ACCION,
                                       controlador = pagina.CONTROLADOR
                                   }).ToList();

                }
                else
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   && pagina.MENSAJE.Contains(paginaCLS.mensajePagina)
                                   select new PaginaCLS
                                   {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       accion = pagina.ACCION,
                                       controlador = pagina.CONTROLADOR
                                   }).ToList();
                }
                
            }
            return PartialView("_tablaPagina",listaPagina);
        }

        public string Guardar(PaginaCLS paginaCLS, int titulo)
        {
            string resultado = "";
            if (!ModelState.IsValid)
            {


            
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                resultado += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    resultado += "<li class='list-group-item'>" + item + "</li>";
                }

                return resultado += "</ul>";

            }
            else
            {
                using (var bd = new BDPasajeEntities())
                {
                    if (titulo == -1)
                    {
                        Pagina pagina = new Pagina();
                        pagina.MENSAJE = paginaCLS.mensaje;
                        pagina.ACCION = paginaCLS.accion;
                        pagina.CONTROLADOR = paginaCLS.controlador;
                        pagina.BHABILITADO = 1;
                        bd.Pagina.Add(pagina);
                        resultado = bd.SaveChanges().ToString();
                        if (resultado == "0") resultado = "";
                    }
                    else
                    {
                        Pagina pagina = bd.Pagina.Where(p => p.IIDPAGINA.Equals(titulo)).First();
                        pagina.MENSAJE = paginaCLS.mensaje;
                        pagina.ACCION = paginaCLS.accion;
                        pagina.CONTROLADOR = paginaCLS.controlador;

                        resultado = bd.SaveChanges().ToString();
                    }

                }
            }

            
                return resultado;
        }

        public JsonResult RecuperarDatos(int titulo)
        {
            PaginaCLS paginaCLS = new PaginaCLS();
            using (var bd = new BDPasajeEntities())
            {
                Pagina pagina = bd.Pagina.Where(p => p.IIDPAGINA.Equals(titulo)).First();
                paginaCLS.mensaje = pagina.MENSAJE;
                paginaCLS.accion = pagina.ACCION;
                paginaCLS.controlador = pagina.CONTROLADOR;
            }
                return Json(paginaCLS,JsonRequestBehavior.AllowGet);
        }
    }
}