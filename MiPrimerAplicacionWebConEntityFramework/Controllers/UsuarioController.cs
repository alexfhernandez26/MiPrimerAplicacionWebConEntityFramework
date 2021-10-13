using MiPrimerAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using System.Security.Cryptography;
using System.Text;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario


        public ActionResult Index()
        {
            ListaPersonas();
            listaRol();

            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using (var bd = new BDPasajeEntities())
            {
                
                List<UsuarioCLS> usuarioCliente = (from usuario in bd.Usuario
                                                   join cliente in bd.Cliente
                                                   on usuario.IID equals cliente.IIDCLIENTE
                                                   join rol in bd.Rol
                                                   on usuario.IIDROL equals rol.IIDROL
                                                   where usuario.bhabilitado == 1
                                                   && usuario.TIPOUSUARIO == "C"
                                                   select new UsuarioCLS
                                                   {
                                                       iidusuario = usuario.IIDUSUARIO,
                                                       nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO,
                                                       nombreusuario = usuario.NOMBREUSUARIO,
                                                       nombreRol = rol.NOMBRE,
                                                       TipodeUsuario = "Cliente"
                                                   }).ToList();

                List<UsuarioCLS> usuarioEmpleado = (from usuario in bd.Usuario
                                                   join empleado in bd.Empleado
                                                   on usuario.IID equals empleado.IIDEMPLEADO
                                                   join rol in bd.Rol
                                                   on usuario.IIDROL equals rol.IIDROL
                                                   where usuario.bhabilitado == 1
                                                   && usuario.TIPOUSUARIO == "E"
                                                   select new UsuarioCLS
                                                   {
                                                       iidusuario = usuario.IIDUSUARIO,
                                                       nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO,
                                                       nombreusuario = usuario.NOMBREUSUARIO,
                                                       nombreRol = rol.NOMBRE,
                                                       TipodeUsuario = "Empleado"
                                                   }).ToList();
                listaUsuario.AddRange(usuarioCliente);
                listaUsuario.AddRange(usuarioEmpleado);
                listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
            }
                return View(listaUsuario);
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

        public int Guardar(UsuarioCLS usuarioCLS, int titulo)
        {
            //Aqui haremos una transaccion, porque vamos a aggregar a la tabla usuario un usuario y a cliente o empleado
            //le vamos a hacer una actualizacion en el campo btieneusuario, haremos mas de una operacion en la bd
            int respuesta = 1;
            int existe = 0;
            try { 
             using (var bd = new BDPasajeEntities())
             {
                    using (var transaccion = new TransactionScope() ) {
                        if (titulo == -1)
                        {
                            existe = bd.Usuario.Where(p => p.NOMBREUSUARIO == usuarioCLS.nombreusuario).Count();
                            if (existe >=1)
                            {
                                respuesta = -100;
                            }
                            else { 
                            Usuario usuario = new Usuario();
                            usuario.NOMBREUSUARIO = usuarioCLS.nombreusuario;
          //Esta clase me va a permitir cifrar cualquier valor para cifrar el password debemos convertirlo a byte[]
          //luego de cifrada la convertimos a string con BitConverter.ToString para insertar en la bd
                            SHA256Managed sha = new SHA256Managed();
                            byte[] contrabyte = Encoding.Default.GetBytes(usuarioCLS.contra);
                            byte[] contrabyteHasheada =  sha.ComputeHash(contrabyte);
                          string contraCadena =  BitConverter.ToString(contrabyteHasheada).Replace("-", "");

                            usuario.CONTRA = contraCadena;
                            usuario.TIPOUSUARIO = usuarioCLS.nombrePersona.Substring(usuarioCLS.nombrePersona.Length-2,1);
                            usuario.IID = usuarioCLS.iid;
                            usuario.IIDROL = usuarioCLS.iidrol;
                            usuario.bhabilitado = 1;
                            bd.Usuario.Add(usuario);

                            if (usuario.TIPOUSUARIO.Equals("C"))
                            {
                                Cliente cliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(usuarioCLS.iid)).First();
                                cliente.bTieneUsuario = 1;

                            }
                            else
                            {
                                Empleado empleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(usuarioCLS.iid)).First();
                                empleado.bTieneUsuario = 1;
                            }
                            respuesta = bd.SaveChanges();
                            transaccion.Complete();
                            }
                        }
                        else
                        {
                            existe = bd.Usuario.Where(p => p.NOMBREUSUARIO == usuarioCLS.nombreusuario && 
                            p.IIDUSUARIO != usuarioCLS.iidusuario).Count();
                            if (existe >= 1)
                            {
                                respuesta = -100;
                            }
                            else
                            {
                                Usuario usuario = bd.Usuario.Where(p => p.IIDUSUARIO == titulo).First();
                                usuario.NOMBREUSUARIO = usuarioCLS.nombreusuario;
                                usuario.IIDROL = usuarioCLS.iidrol;
                                respuesta = bd.SaveChanges();
                                transaccion.Complete();
                            }
                           
                        }

                    }
                }
            }
            catch (Exception e)
            {
                
                respuesta = 0;
            }

            return respuesta;
        }

     public ActionResult Filtro(UsuarioCLS usuarioCLS)
        {
            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            if (usuarioCLS.nombrePersonaBuqueda ==null)
            {
                ListaPersonas();
                listaRol();

                
                using (var bd = new BDPasajeEntities())
                {

                    List<UsuarioCLS> usuarioCliente = (from usuario in bd.Usuario
                                                       join cliente in bd.Cliente
                                                       on usuario.IID equals cliente.IIDCLIENTE
                                                       join rol in bd.Rol
                                                       on usuario.IIDROL equals rol.IIDROL
                                                       where usuario.bhabilitado == 1
                                                       && usuario.TIPOUSUARIO == "C"
                                                       select new UsuarioCLS
                                                       {
                                                           iidusuario = usuario.IIDUSUARIO,
                                                           nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO,
                                                           nombreusuario = usuario.NOMBREUSUARIO,
                                                           nombreRol = rol.NOMBRE,
                                                           TipodeUsuario = "Cliente"
                                                       }).ToList();

                    List<UsuarioCLS> usuarioEmpleado = (from usuario in bd.Usuario
                                                        join empleado in bd.Empleado
                                                        on usuario.IID equals empleado.IIDEMPLEADO
                                                        join rol in bd.Rol
                                                        on usuario.IIDROL equals rol.IIDROL
                                                        where usuario.bhabilitado == 1
                                                        && usuario.TIPOUSUARIO == "E"
                                                        select new UsuarioCLS
                                                        {
                                                            iidusuario = usuario.IIDUSUARIO,
                                                            nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO,
                                                            nombreusuario = usuario.NOMBREUSUARIO,
                                                            nombreRol = rol.NOMBRE,
                                                            TipodeUsuario = "Empleado"
                                                        }).ToList();
                    listaUsuario.AddRange(usuarioCliente);
                    listaUsuario.AddRange(usuarioEmpleado);
                    listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
                }
            }
            else
            {
                ListaPersonas();
                listaRol();

               
                using (var bd = new BDPasajeEntities())
                {

                    List<UsuarioCLS> usuarioCliente = (from usuario in bd.Usuario
                                                       join cliente in bd.Cliente
                                                       on usuario.IID equals cliente.IIDCLIENTE
                                                       join rol in bd.Rol
                                                       on usuario.IIDROL equals rol.IIDROL
                                                       where usuario.bhabilitado == 1
                                                       && usuario.TIPOUSUARIO == "C"
                                                       &&(
                                                       cliente.NOMBRE.Contains(usuarioCLS.nombrePersonaBuqueda)
                                                       || cliente.APPATERNO.Contains(usuarioCLS.nombrePersonaBuqueda)
                                                       || cliente.APMATERNO.Contains(usuarioCLS.nombrePersonaBuqueda)
                                                       )
                                                       select new UsuarioCLS
                                                       {
                                                           iidusuario = usuario.IIDUSUARIO,
                                                           nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO,
                                                           nombreusuario = usuario.NOMBREUSUARIO,
                                                           nombreRol = rol.NOMBRE,
                                                           TipodeUsuario = "Cliente"
                                                       }).ToList();

                    List<UsuarioCLS> usuarioEmpleado = (from usuario in bd.Usuario
                                                        join empleado in bd.Empleado
                                                        on usuario.IID equals empleado.IIDEMPLEADO
                                                        join rol in bd.Rol
                                                        on usuario.IIDROL equals rol.IIDROL
                                                        where usuario.bhabilitado == 1
                                                        && usuario.TIPOUSUARIO == "E"
                                                        && (
                                                       empleado.NOMBRE.Contains(usuarioCLS.nombrePersonaBuqueda)
                                                       || empleado.APPATERNO.Contains(usuarioCLS.nombrePersonaBuqueda)
                                                       || empleado.APMATERNO.Contains(usuarioCLS.nombrePersonaBuqueda)
                                                       )
                                                        select new UsuarioCLS
                                                        {
                                                            iidusuario = usuario.IIDUSUARIO,
                                                            nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO,
                                                            nombreusuario = usuario.NOMBREUSUARIO,
                                                            nombreRol = rol.NOMBRE,
                                                            TipodeUsuario = "Empleado"
                                                        }).ToList();
                    listaUsuario.AddRange(usuarioCliente);
                    listaUsuario.AddRange(usuarioEmpleado);
                    listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
                }
            }
                return PartialView("_tablaUsuario",listaUsuario);
        }

        public JsonResult RecuperarInformacion(int iidusuario)
        {
            UsuarioCLS usuarioCLS = new UsuarioCLS();
            using (var bd = new BDPasajeEntities())
            {
                Usuario usuario = bd.Usuario.Where(p => p.IIDUSUARIO == iidusuario).First();
                usuarioCLS.nombreusuario = usuario.NOMBREUSUARIO;
                usuarioCLS.iidrol = (int)usuario.IIDROL;
            }
                return Json(usuarioCLS,JsonRequestBehavior.AllowGet);
        }

        public int EliminarRegistro(int iidusuario)
        {
            int respuesta = 0;
            using (var bd = new BDPasajeEntities())
            {
                Usuario usuario = bd.Usuario.Where(p => p.IIDUSUARIO == iidusuario).First();
               usuario.bhabilitado = 0;
                respuesta = bd.SaveChanges();
            }
            return respuesta;
        }
    }
}