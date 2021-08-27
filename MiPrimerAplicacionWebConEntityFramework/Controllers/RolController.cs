﻿using MiPrimerAplicacionWebConEntityFramework.Models;
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

        public ActionResult Filtro(string nombre)
        {
            List<RolCLS> listaRol = null;

            using (var bd = new BDPasajeEntities())
            {
                if (nombre == null)
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
                else
                {
                    listaRol = (from rol in bd.Rol
                                where rol.BHABILITADO == 1
                                && rol.NOMBRE.Contains(nombre)
                                select new RolCLS
                                {
                                    iidrol = rol.IIDROL,
                                    nombre = rol.NOMBRE,
                                    descripcion = rol.DESCRIPCION
                                }).ToList();
                }
               
            }
            return PartialView("_tablaRol",listaRol);
        }

        public int Guardar(RolCLS rolCLS, int titulo)
        {
            int respuesta = 0;

            using (var bd = new BDPasajeEntities())
            {
                Rol rol = new Rol();
                rol.NOMBRE = rolCLS.nombre;
                rol.DESCRIPCION = rolCLS.descripcion;
                rol.BHABILITADO = 1;
                bd.Rol.Add(rol);
                //Si se guarda en la BD respuesta sera igual a 1, sino sera cero
                respuesta=   bd.SaveChanges();
            }
                return respuesta;
        }
    }
}