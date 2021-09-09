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

        public int Guardar(UsuarioCLS usuarioCLS, int titulo)
        {
            //Aqui haremos una transaccion, porque vamos a aggregar a la tabla usuario un usuario y a cliente o empleado
            //le vamos a hacer una actualizacion en el campo btieneusuario, haremos mas de una operacion en la bd
            int respuesta = 0;

            try { 
             using (var bd = new BDPasajeEntities())
             {
                    using (var transaccion = new TransactionScope() ) {
                        if (titulo == 1)
                        {
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
                }
            }
            catch (Exception e)
            {
                
                respuesta = 0;
            }

            return respuesta;
        }
    }
}