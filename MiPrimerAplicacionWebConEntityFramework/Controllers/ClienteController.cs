using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        List<ClienteCLS> listaCliente = null;
        public ActionResult Index()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaCliente = (from cliente in bd.Cliente
                                where cliente.BHABILITADO ==1
                              select new ClienteCLS                             
                              {
                                  iidCliente = cliente.IIDCLIENTE,
                                  NOMBRE = cliente.NOMBRE,
                                  APPATERNO = cliente.APPATERNO,
                                  APMATERNO = cliente.APMATERNO,
                                  TELEFONOCELULAR = cliente.TELEFONOCELULAR
                              }).ToList();
            }

            return View(listaCliente);
        }

        //Llenado comboBox,para llenar un comboBox c# pide que le des una lista de SelectListItem
        List<SelectListItem> listaSexo = null;
        public void llenarComboSexo()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaSexo = (from sexo in bd.Sexo
                             where sexo.BHABILITADO == 1
                             select new SelectListItem
                             {
                                 Text = sexo.NOMBRE,
                                 Value = sexo.IIDSEXO.ToString()
                             }).ToList();

                listaSexo.Insert(0,new SelectListItem {Text="--Seleccione--",Value="" });
            }
        }

        public ActionResult Agregar()
        {
            //viewBag me permite pasar informacion del controller a la vista

            llenarComboSexo();
            ViewBag.lista = listaSexo;
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(ClienteCLS clienteCLS)
        {

            if (!ModelState.IsValid)
            {
                llenarComboSexo();
                ViewBag.lista = listaSexo;
                return View(clienteCLS);
            }
            else
            {
                using (var bd = new BDPasajeEntities())
                {
                    Cliente cliente = new Cliente();
                    cliente.NOMBRE = clienteCLS.NOMBRE;
                    cliente.APPATERNO = clienteCLS.APPATERNO;
                    cliente.APMATERNO = clienteCLS.APMATERNO;
                    cliente.EMAIL = clienteCLS.EMAIL;
                    cliente.DIRECCION = clienteCLS.DIRECCION;
                    cliente.IIDSEXO = clienteCLS.IIDSEXO;
                    cliente.TELEFONOFIJO = clienteCLS.TELEFONOFIJO;
                    cliente.TELEFONOCELULAR = clienteCLS.TELEFONOCELULAR;
                    cliente.BHABILITADO = 1;
                    bd.Cliente.Add(cliente);
                    bd.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            llenarComboSexo();
            ViewBag.lista = listaSexo;
            ClienteCLS clienteCLS = new ClienteCLS();

            using (var bd = new BDPasajeEntities())
            {
                Cliente cliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(id)).First();
                clienteCLS.iidCliente = cliente.IIDCLIENTE;
                clienteCLS.NOMBRE = cliente.NOMBRE;
                clienteCLS.APPATERNO = cliente.APPATERNO;
                clienteCLS.APMATERNO = cliente.APMATERNO;
                clienteCLS.EMAIL = cliente.EMAIL;
                clienteCLS.DIRECCION= cliente.DIRECCION;
                clienteCLS.IIDSEXO = (int)cliente.IIDSEXO;
                clienteCLS.TELEFONOFIJO= cliente.TELEFONOFIJO;
                clienteCLS.TELEFONOCELULAR = cliente.TELEFONOCELULAR;
            }

            return View(clienteCLS);
        }
        [HttpPost]
        public ActionResult Editar(ClienteCLS clienteCLS)
        {
            Cliente cliente = new Cliente();
            llenarComboSexo();
            if (!ModelState.IsValid)
            {
                return View(clienteCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                cliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(clienteCLS.iidCliente)).First();
                cliente.NOMBRE = clienteCLS.NOMBRE;
                cliente.APPATERNO = clienteCLS.APPATERNO;
                cliente.APMATERNO = clienteCLS.APMATERNO;
                cliente.EMAIL = clienteCLS.EMAIL;
                cliente.DIRECCION = clienteCLS.DIRECCION;
                cliente.IIDSEXO = (int)clienteCLS.IIDSEXO;
                cliente.TELEFONOFIJO = clienteCLS.TELEFONOFIJO;
                cliente.TELEFONOCELULAR = clienteCLS.TELEFONOCELULAR;
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}