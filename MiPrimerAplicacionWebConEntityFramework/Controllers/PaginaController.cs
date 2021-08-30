﻿using MiPrimerAplicacionWebConEntityFramework.Models;
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
                if (paginaCLS.mensaje ==null)
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
                                   && pagina.MENSAJE.Contains(paginaCLS.mensaje)
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

        public int Guardar(PaginaCLS paginaCLS, int titulo)
        {
            int resultado = 0;

            using (var bd = new BDPasajeEntities())
            {
                Pagina pagina = new Pagina();
                pagina.MENSAJE = paginaCLS.mensaje;
                pagina.ACCION = paginaCLS.accion;
                pagina.CONTROLADOR = paginaCLS.controlador;
                pagina.BHABILITADO = 1;
                bd.Pagina.Add(pagina);
             resultado =   bd.SaveChanges();
            }
                return resultado;
        }
    }
}