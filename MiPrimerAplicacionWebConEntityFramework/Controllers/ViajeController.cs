using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
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
            using (var bd = new BDPasajeEntities())
            {
                listaViaje = (from viaje in bd.Viaje
                              join lugarOrigen in bd.Lugar
                              on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                              join lugarDestino in bd.Lugar
                              on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                              join bus in bd.Bus
                              on viaje.IIDBUS equals bus.IIDBUS
                              select new ViajeCLS
                              {
                                  iidviaje = viaje.IIDVIAJE,
                                  NombrelugarOrigen =  lugarOrigen.NOMBRE,
                                  NombrelugarDestino = lugarDestino.NOMBRE,
                                  nombreBus = bus.PLACA
                              }).ToList();
            }
                return View(listaViaje);
        }
    }
}