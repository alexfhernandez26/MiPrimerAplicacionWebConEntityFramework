using iTextSharp.text;
using iTextSharp.text.pdf;
using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class ViajeController : Controller
    {
        
        // GET: Viaje
        List<ViajeCLS> listaViaje = null;
        public ActionResult Index()
        {
            llenarComboBus();
            llenarComboViaje();
            using (var bd = new BDPasajeEntities())
            {
                listaViaje = (from viaje in bd.Viaje
                              join lugarOrigen in bd.Lugar
                              on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                              join lugarDestino in bd.Lugar
                              on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                              join bus in bd.Bus
                              on viaje.IIDBUS equals bus.IIDBUS
                              where viaje.BHABILITADO ==1
                              select new ViajeCLS
                              {
                                  iidviaje = viaje.IIDVIAJE,
                                  NombrelugarOrigen =  lugarOrigen.NOMBRE,
                                  NombrelugarDestino = lugarDestino.NOMBRE,
                                  fechaViaje =(DateTime)viaje.FECHAVIAJE,
                                  nombreBus = bus.PLACA
                              }).ToList();
            }
                return View(listaViaje);
        }
        public ActionResult Agregar()
        {
            llenarComboBus();
            llenarComboViaje();
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(ViajeCLS viajeCLS)
        {
            if (!ModelState.IsValid)
            {
                return View(viajeCLS);
            }
            else
            {
                using (var bd = new BDPasajeEntities())
                {
                    Viaje viaje = new Viaje();
                    viaje.IIDVIAJE = viajeCLS.iidviaje;
                    viaje.IIDLUGARORIGEN = viajeCLS.iidlugarorigen;
                    viaje.IIDLUGARDESTINO = viajeCLS.iidlugardestino;
                    viaje.PRECIO = viajeCLS.precio;
                    viaje.FECHAVIAJE =(DateTime)viajeCLS.fechaViaje;
                    viaje.IIDBUS = viajeCLS.iidbus;
                   viaje.BHABILITADO = 1;
                    bd.Viaje.Add(viaje);
                    bd.SaveChanges();
                }
            }
            return RedirectToAction("index");
        }
/*
        public ActionResult Editar(int id)
        {
            llenarComboViaje();
            llenarComboBus();
            
            ViajeCLS viajeCLS = new ViajeCLS();

            using (var bd = new BDPasajeEntities())
            {
                Viaje viaje = bd.Viaje.Where(p => p.IIDVIAJE.Equals(id)).First();
                viajeCLS.iidviaje = (int)viaje.IIDVIAJE;
                viajeCLS.iidlugarorigen = (int)viaje.IIDLUGARORIGEN;
                viajeCLS.iidlugardestino = (int)viaje.IIDLUGARDESTINO;
                viajeCLS.precio = (int)viaje.PRECIO;
                viajeCLS.fechaViaje = (DateTime)viaje.FECHAVIAJE;
                viajeCLS.iidbus = (int)viaje.IIDBUS;
            }
            return View(viajeCLS);
        }

        [HttpPost]
        public ActionResult Editar(ViajeCLS viajeCLS)
        {

            Viaje viaje = new Viaje();
            if (!ModelState.IsValid)
            {
                return View(viajeCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                viaje = bd.Viaje.Where(p => p.IIDVIAJE.Equals(viajeCLS.iidviaje)).First();
                viaje.IIDLUGARORIGEN = viajeCLS.iidlugarorigen;
                viaje.IIDLUGARDESTINO = viajeCLS.iidlugardestino;
                viaje.PRECIO = viajeCLS.precio;
                viaje.FECHAVIAJE = viajeCLS.fechaViaje;
                viaje.IIDBUS = viajeCLS.iidbus;
                
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        
        */
        public void llenarComboViaje()
        {
            List<SelectListItem> llenarViaje = null;

            using (var bd = new BDPasajeEntities())
            {
                llenarViaje = (from lugar in bd.Lugar
                               where lugar.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = lugar.NOMBRE,
                                   Value = lugar.IIDLUGAR.ToString()
                               }).ToList();
                llenarViaje.Insert(0,new SelectListItem {Text="--Seleccione--",Value="" });
                ViewBag.viaje = llenarViaje;
            }

        }

        public void llenarComboBus()
        {
            List<SelectListItem> llenarBus = null;

            using (var bd = new BDPasajeEntities())
            {
                llenarBus = (from bus in bd.Bus
                             where bus.BHABILITADO == 1
                             select new SelectListItem()
                             {
                                 Text = bus.PLACA,
                                 Value = bus.IIDBUS.ToString()
                             }).ToList();
                llenarBus.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.bus = llenarBus;
            }

        }

        public string Guardar(ViajeCLS viajeCLS, HttpPostedFileBase foto, int titulo)
        {
            
            string mensaje = "";
            if (!ModelState.IsValid || (foto==null && titulo==-1))
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                if (foto == null) mensaje += "La foto es obligatoria";
                mensaje += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    mensaje += "<li class='list-group-item'>" + item + "</li>";
                }
            }
            else
            {
                //obteniendo la foto que recibimos por parametro
                byte[] fotobd;

                BinaryReader lectofoto = new BinaryReader(foto.InputStream);
                fotobd = lectofoto.ReadBytes((int)foto.ContentLength) ;
                using (var bd = new BDPasajeEntities())
                {
                    if (titulo == -1)
                    {
                        Viaje viaje = new Viaje();
                        viaje.IIDLUGARORIGEN = viajeCLS.iidlugarorigen;
                        viaje.IIDLUGARDESTINO = viajeCLS.iidlugardestino;
                        viaje.PRECIO = viajeCLS.precio;
                        viaje.FECHAVIAJE = viajeCLS.fechaViaje;
                        viaje.IIDBUS = viajeCLS.iidbus;
                        viaje.NUMEROASIENTOSDISPONIBLES = viajeCLS.numeroAsientosDisponibles;
                        viaje.BHABILITADO = 1;
                        viaje.FOTO = fotobd;
                        viaje.nombrefoto = viajeCLS.nombreFoto;
                        bd.Viaje.Add(viaje);
                        mensaje = bd.SaveChanges().ToString();
                    }
                    else
                    {
                        Viaje viaje = bd.Viaje.Where(p => p.IIDVIAJE == titulo).First();
                        viaje.IIDLUGARORIGEN = viajeCLS.iidlugarorigen;
                        viaje.IIDLUGARDESTINO = viajeCLS.iidlugardestino;
                        viaje.PRECIO = viajeCLS.precio;
                        viaje.FECHAVIAJE = viajeCLS.fechaViaje;
                        viaje.IIDBUS = viajeCLS.iidbus;
                        viaje.NUMEROASIENTOSDISPONIBLES = viajeCLS.numeroAsientosDisponibles;
                        viaje.FOTO = fotobd;
                        viaje.nombrefoto = viajeCLS.nombreFoto;
                        mensaje = bd.SaveChanges().ToString();
                    }
                    
                }
            }


            return mensaje;
        }

        public ActionResult Filtrar(int? lugardestinoFiltro)
        {
            llenarComboBus();
            llenarComboViaje();

            using (var bd = new BDPasajeEntities())
            {
                if (lugardestinoFiltro==null)
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarOrigen in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                                  join lugarDestino in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  select new ViajeCLS
                                  {
                                      iidviaje = viaje.IIDVIAJE,
                                      NombrelugarOrigen = lugarOrigen.NOMBRE,
                                      NombrelugarDestino = lugarDestino.NOMBRE,
                                      fechaViaje = (DateTime)viaje.FECHAVIAJE,
                                      nombreBus = bus.PLACA
                                  }).ToList();
                }
                else
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarOrigen in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                                  join lugarDestino in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  && viaje.IIDLUGARDESTINO == lugardestinoFiltro
                                  select new ViajeCLS
                                  {
                                      iidviaje = viaje.IIDVIAJE,
                                      NombrelugarOrigen = lugarOrigen.NOMBRE,
                                      NombrelugarDestino = lugarDestino.NOMBRE,
                                      fechaViaje = (DateTime)viaje.FECHAVIAJE,
                                      nombreBus = bus.PLACA
                                  }).ToList();
                }
            }
                return PartialView("_tablaViaje",listaViaje);
        }

        public JsonResult RecuperarInformacion(int idrecuperar)
        {
            ViajeCLS viajeCLS = new ViajeCLS();
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    Viaje viaje = bd.Viaje.Where(p => p.IIDVIAJE.Equals(idrecuperar)).First();
                    viajeCLS.iidviaje = (int)viaje.IIDVIAJE;
                    viajeCLS.iidlugarorigen = (int)viaje.IIDLUGARORIGEN;
                    viajeCLS.iidlugardestino = (int)viaje.IIDLUGARDESTINO;
                    viajeCLS.precio = (int)viaje.PRECIO;
                    //pide (anio-mes-dia)
                    //la bd manda (dia-mes-anio)
                    viajeCLS.recuperarFechaCadena = ((DateTime)viaje.FECHAVIAJE).ToString("yyyy-MM-dd");
                    viajeCLS.iidbus = (int)viaje.IIDBUS;
                    viajeCLS.numeroAsientosDisponibles = (int)viaje.NUMEROASIENTOSDISPONIBLES;
                    viajeCLS.nombreFoto = viaje.nombrefoto;

                    //recuperando foto
                    //para mostrar la foto primero necesitamos la extencion de la foto, osea el nombre  Path.GetExtension(viaje.nombrefoto)
                    //lo segundo es recuperar la foto que esta en byte[], debemos convertirla a base64 para recuperarla y mostrarla,
                    //para recuperarla debemos pasarla como base64String, estamos pasando la foto de byte convertida a ;base64,
                    viajeCLS.extencion = Path.GetExtension(viaje.nombrefoto);
                    viajeCLS.recuperarFotoCadena = Convert.ToBase64String(viaje.FOTO);
                }

                return Json(viajeCLS, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public int Eliminar(int idElimina)
        {
            int respuesta = 0;
            using (var bd = new BDPasajeEntities())
            {
                Viaje viaje = bd.Viaje.Where(p => p.IIDVIAJE == idElimina).First();
                viaje.BHABILITADO = 0;
                 respuesta= bd.SaveChanges();
            }

                return respuesta;
        }
    }
}