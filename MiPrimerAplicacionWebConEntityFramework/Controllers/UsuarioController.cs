using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario


        public ActionResult Index()
        {
            ListaPersonas();
            listaRol();
            return View();
        }

        public void ListaPersonas()
        {
            List<SelectListItem> listaPersonas = new List<SelectListItem>();
            using (var bd = new BDPasajeEntities())
            {
                

                List<SelectListItem> listaCliente = (from cliente in bd.Cliente
                                                     where cliente.BHABILITADO == 1 && cliente.bTieneUsuario != 1
                                                     select new SelectListItem
                                                     {
                                                         Text = cliente.NOMBRE + " " + cliente.APPATERNO + "(C)",
                                                         Value = cliente.IIDCLIENTE.ToString()
                                                     }).ToList();

                List<SelectListItem> listaEmpleado = (from empleado in bd.Empleado
                                                      where empleado.BHABILITADO == 1 && empleado.bTieneUsuario != 1
                                                      select new SelectListItem
                                                      {
                                                          Text = empleado.NOMBRE + " " + empleado.APPATERNO + "(E)",
                                                          Value = empleado.IIDEMPLEADO.ToString()
                                                      }).ToList();

                listaPersonas.AddRange(listaCliente);
                listaPersonas.AddRange(listaEmpleado);

                listaPersonas.Insert(0, new SelectListItem { Text = "--Seleccione", Value = "" });
                ViewBag.listapersonas = listaPersonas;
            }          
        }

        public void listaRol()
        {
           
            using (var bd = new BDPasajeEntities())
            {


                List<SelectListItem> listaRol = (from rol in bd.Rol
                                                     where rol.BHABILITADO == 1 
                                                     select new SelectListItem
                                                     {
                                                         Text = rol.NOMBRE,
                                                         Value =rol.IIDROL.ToString()
                                                     }).ToList();

                listaRol.Insert(0, new SelectListItem { Text = "--Seleccione", Value = "" });
                ViewBag.listarol = listaRol;
            }

            
        }


    }
}