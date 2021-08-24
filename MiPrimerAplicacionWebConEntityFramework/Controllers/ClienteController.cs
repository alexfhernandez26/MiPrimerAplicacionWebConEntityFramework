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
        public ActionResult Index(ClienteCLS clienteCLS)
        {
            llenarComboSexo();
            int iidsexo = clienteCLS.IIDSEXO;
            using (var bd = new BDPasajeEntities())
            {
                if (iidsexo ==0)
                {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    select new ClienteCLS
                                    {
                                        iidCliente = cliente.IIDCLIENTE,
                                        NOMBRE = cliente.NOMBRE,
                                        APPATERNO = cliente.APPATERNO,
                                        APMATERNO = cliente.APMATERNO,
                                        TELEFONOCELULAR = cliente.TELEFONOCELULAR
                                    }).ToList();
                }
                else
                {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    && cliente.IIDSEXO == iidsexo
                                    select new ClienteCLS
                                    {
                                        iidCliente = cliente.IIDCLIENTE,
                                        NOMBRE = cliente.NOMBRE,
                                        APPATERNO = cliente.APPATERNO,
                                        APMATERNO = cliente.APMATERNO,
                                        TELEFONOCELULAR = cliente.TELEFONOCELULAR
                                    }).ToList();
                }
                
            }

            return View(listaCliente);
        }

        //Llenado comboBox,para llenar un comboBox c# pide que le des una lista de SelectListItem
       
        public void llenarComboSexo()
        {
            List<SelectListItem> listaSexo = null;
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
                ViewBag.lista = listaSexo;
            }
        }

        public ActionResult Agregar()
        {
            //viewBag me permite pasar informacion del controller a la vista
            
            llenarComboSexo();
            
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(ClienteCLS clienteCLS)
        {

            if (!ModelState.IsValid)
            {
                List<SelectListItem> listaSexo = null;
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
            llenarComboSexo();
            Cliente cliente = new Cliente();
            int id = clienteCLS.iidCliente;
            string nombre = clienteCLS.NOMBRE;
            string apellidoP = clienteCLS.APPATERNO;
            string apellidoM = clienteCLS.APMATERNO;
            int existe = 0;

            using (var bd = new BDPasajeEntities())
            {
                existe = bd.Cliente.Where(p => p.NOMBRE.Equals(nombre) && p.APPATERNO.Equals(apellidoP) && p.APMATERNO.Equals(apellidoM) && !p.IIDCLIENTE.Equals(id)).Count();
            }

                
            if (!ModelState.IsValid || existe >=1)
            {
                llenarComboSexo();

                if (existe >= 1) clienteCLS.mensajeError = "El cliente ya existe";
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
        
        public ActionResult Eliminar(int iidcliente)
        {
            using (var bd = new BDPasajeEntities())
            {
                Cliente cliente = new Cliente();
                cliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(iidcliente)).First();
                cliente.BHABILITADO = 0;
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }

    }
}