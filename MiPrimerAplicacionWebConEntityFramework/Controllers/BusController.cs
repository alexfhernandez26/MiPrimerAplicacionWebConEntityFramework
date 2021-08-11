using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class BusController : Controller
    {
        // GET: Bus
        List<BusCLS> listaBus = null;
        public ActionResult Index()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaBus = (from bus in bd.Bus
                            join sucursal in bd.Sucursal
                            on bus.IIDSUCURSAL equals sucursal.IIDSUCURSAL
                            join tipobus in bd.TipoBus
                            on bus.IIDTIPOBUS equals tipobus.IIDTIPOBUS
                            join modelo in bd.Modelo
                            on bus.IIDMODELO equals modelo.IIDMODELO
                            join marca in bd.Marca
                            on bus.IIDMARCA equals marca.IIDMARCA
                            select new BusCLS
                            {
                                iidbus = bus.IIDBUS,
                                NombreSucursal = sucursal.NOMBRE,
                                NombreTipoBus = tipobus.NOMBRE,
                                placa = bus.PLACA,
                                fechacompra = (DateTime)bus.FECHACOMPRA,
                                NombreModelo = modelo.NOMBRE,
                                NombreMarca = marca.NOMBRE,
                                descripcion = bus.DESCRIPCION

                            }).ToList();
            }
               

            return View(listaBus);
        }

        public ActionResult Agregar()
        {
            TodosLosCombos();
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(BusCLS busCLS)
        {
            if (!ModelState.IsValid)
            {
                TodosLosCombos();
                return View(busCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Bus bus = new Bus();
                bus.IIDSUCURSAL = busCLS.iidsucursal;
                bus.IIDTIPOBUS = busCLS.iidtipobus;
                bus.PLACA = busCLS.placa;
                bus.FECHACOMPRA = busCLS.fechacompra;
                bus.IIDMODELO = busCLS.iidmodelo;
                bus.NUMEROFILAS = busCLS.numeroFilas;
                bus.NUMEROCOLUMNAS = busCLS.numeroColumnas;
                bus.DESCRIPCION = busCLS.descripcion;
                bus.IIDMARCA = busCLS.iidmarca;
                bus.BHABILITADO = 1;
                bd.Bus.Add(bus);
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

       
        public void llenarComboNombreSucursal()
        {
            List<SelectListItem> listanombreSucursal = null;
            using (var bd = new BDPasajeEntities())
            {
                listanombreSucursal = (from sucursal in bd.Sucursal
                                       where sucursal.BHABILITADO == 1
                                       select new SelectListItem
                                       {
                                           Text = sucursal.NOMBRE,
                                           Value = sucursal.IIDSUCURSAL.ToString()
                                       }).ToList();
                listanombreSucursal.Insert(0,new SelectListItem { Text="--Seleccione--",Value="" });
            }
            ViewBag.listanombreSucursal = listanombreSucursal;
               
        }

       
        public void llenarComboNombreTipobus()
        {
            List<SelectListItem> listanombreTipobus = null;
            using (var bd = new BDPasajeEntities())
            {
                listanombreTipobus = (from tipobus in bd.TipoBus
                                       where tipobus.BHABILITADO == 1
                                       select new SelectListItem
                                       {
                                           Text = tipobus.NOMBRE,
                                           Value = tipobus.IIDTIPOBUS.ToString()
                                       }).ToList();
                listanombreTipobus.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
            ViewBag.listanombretipobus = listanombreTipobus;

        }

        
        public void llenarComboNombreModelo()
        {
            List<SelectListItem> listanombreModelo = null;
            using (var bd = new BDPasajeEntities())
            {
                listanombreModelo = (from modelo in bd.Modelo
                                      where modelo.BHABILITADO == 1
                                      select new SelectListItem
                                      {
                                          Text = modelo.NOMBRE,
                                          Value = modelo.IIDMODELO.ToString()
                                      }).ToList();
                listanombreModelo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
            ViewBag.listanombretimodelo = listanombreModelo;

        }

       
        public void llenarComboNombreMarca()
        {
            List<SelectListItem> listanombreMarca = null;
            using (var bd = new BDPasajeEntities())
            {
                listanombreMarca = (from marca in bd.Marca
                                     where marca.BHABILITADO == 1
                                     select new SelectListItem
                                     {
                                         Text = marca.NOMBRE,
                                         Value = marca.IIDMARCA.ToString()
                                     }).ToList();
                listanombreMarca.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
            ViewBag.listanombremarca = listanombreMarca;

        }
        public void TodosLosCombos()
        {
            llenarComboNombreSucursal();
            llenarComboNombreModelo();
            llenarComboNombreMarca();
            llenarComboNombreTipobus();
        }

    }
}