using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        List<EmpleadoCLS> listaEmpelados = null;
        public ActionResult Index()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaEmpelados = (from empleados in bd.Empleado
                                  join tipousuario in bd.TipoUsuario
                                  on empleados.IIDTIPOUSUARIO equals tipousuario.IIDTIPOUSUARIO
                                  join tipocontrato in bd.TipoContrato
                                  on empleados.IIDTIPOCONTRATO equals tipocontrato.IIDTIPOCONTRATO
                                  select new EmpleadoCLS
                                  {
                                      iidempleado = empleados.IIDEMPLEADO,
                                      nombre = empleados.NOMBRE,
                                      apPaterno = empleados.APPATERNO,
                                      NombreTipoUsuario = tipousuario.NOMBRE,
                                      NombreTipoContrato = tipocontrato.NOMBRE
                                  }).ToList();
            }
               
            return View(listaEmpelados);
        }
    }
}