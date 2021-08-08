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

        public ActionResult Agregar()
        {
            listarTodoslosCombos();
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(EmpleadoCLS empleadoCLS)
        {
            listarTodoslosCombos();
            if (!ModelState.IsValid)
            {
                listarTodoslosCombos();
                return View(empleadoCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Empleado empleado = new Empleado();
                empleado.NOMBRE = empleadoCLS.nombre;
                empleado.APPATERNO = empleadoCLS.apPaterno;
                empleado.APMATERNO = empleadoCLS.apMaterno;
                empleado.FECHACONTRATO = empleadoCLS.fechaContrato;
                empleado.IIDTIPOUSUARIO = empleadoCLS.iidtipousuario;
                empleado.IIDTIPOCONTRATO = empleadoCLS.iidtipocontrato;
                empleado.IIDSEXO = empleadoCLS.iidsexo;
                empleado.BHABILITADO = 1;

                bd.Empleado.Add(empleado);
                bd.SaveChanges();
            }
                         
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            listarTodoslosCombos();
            EmpleadoCLS empleadoCLS= new EmpleadoCLS();

            using (var bd = new BDPasajeEntities())
            {
                Empleado empleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(id)).First();
                empleadoCLS.iidempleado = empleado.IIDEMPLEADO;
                empleadoCLS.nombre = empleado.NOMBRE;
                empleadoCLS.apPaterno = empleado.APPATERNO;
                empleadoCLS.apMaterno = empleado.APMATERNO;
                empleadoCLS.fechaContrato = (DateTime)empleado.FECHACONTRATO;
                empleadoCLS.iidtipousuario = (int)empleado.IIDTIPOUSUARIO;
                empleadoCLS.iidtipocontrato = (int)empleado.IIDTIPOCONTRATO;
                empleadoCLS.iidsexo = (int)empleado.IIDSEXO;
            }
                return View(empleadoCLS);
        }
        
        [HttpPost]
        public ActionResult Editar(EmpleadoCLS empleadoCLS)
        {
            listarTodoslosCombos();
            Empleado empleado = new Empleado();

            if (!ModelState.IsValid)
            {
                return View(empleadoCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                empleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(empleadoCLS.iidempleado)).First();
                empleado.NOMBRE = empleadoCLS.nombre;
                empleado.APPATERNO = empleadoCLS.apPaterno;
                empleado.APMATERNO = empleadoCLS.apMaterno;
                empleado.FECHACONTRATO = empleadoCLS.fechaContrato;
                empleado.IIDTIPOUSUARIO = empleadoCLS.iidtipousuario;
                empleado.IIDTIPOCONTRATO = empleadoCLS.iidtipocontrato;
                empleado.IIDSEXO = empleadoCLS.iidsexo;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        List<SelectListItem> listaTipoUsuario = null;
        public void llenarComboTipoUsuario()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaTipoUsuario = (from tipousuario in bd.TipoUsuario
                                    where tipousuario.BHABILITADO == 1
                                    select new SelectListItem
                                    {
                                        Text = tipousuario.NOMBRE,
                                        Value = tipousuario.IIDTIPOUSUARIO.ToString()
                                    }).ToList();
                listaTipoUsuario.Insert(0,new SelectListItem {Text="--Seleccione--",Value="" });
            }
            ViewBag.tipoUsuario = listaTipoUsuario;
        }

        List<SelectListItem> listaTipoCpontrato = null;
        public void llenarComboTipoContrato()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaTipoCpontrato = (from tipocontrato in bd.TipoContrato
                                    where tipocontrato.BHABILITADO == 1
                                    select new SelectListItem
                                    {
                                        Text = tipocontrato.NOMBRE,
                                        Value = tipocontrato.IIDTIPOCONTRATO.ToString()
                                    }).ToList();
                listaTipoCpontrato.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
            ViewBag.tipocontrato = listaTipoCpontrato;
        }

        List<SelectListItem> listaTipoSexo = null;
        public void llenarComboTiposexo()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaTipoSexo = (from tiposexo in bd.Sexo
                                      where tiposexo.BHABILITADO == 1
                                      select new SelectListItem
                                      {
                                          Text = tiposexo.NOMBRE,
                                          Value = tiposexo.IIDSEXO.ToString()
                                      }).ToList();
                listaTipoSexo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
            ViewBag.tiposexo = listaTipoSexo;
        }

        public void listarTodoslosCombos()
        {
            llenarComboTipoUsuario();
            llenarComboTipoContrato();
            llenarComboTiposexo();
        }
    }
}