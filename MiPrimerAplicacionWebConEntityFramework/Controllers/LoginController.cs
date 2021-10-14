using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MiPrimerAplicacionWebConEntityFramework.Models;
namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {

            return View();
        }

        public string Login(UsuarioCLS usuarioCLS)
        {
            string respuesta = "";
            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                respuesta += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    respuesta += "<li class='list-group-item'>" + item + "</li>";
                }

                return respuesta += "</ul>";

            }
            else
            {
                string usuario = usuarioCLS.nombreusuario;
                string contra = usuarioCLS.contra;

                //Esta clase me va a permitir cifrar cualquier valor para cifrar el password debemos convertirlo a byte[]
                //luego de cifrada la convertimos a string con BitConverter.ToString para insertar en la bd
                SHA256Managed sha = new SHA256Managed();
                byte[] contrabyte = Encoding.Default.GetBytes(contra);
                byte[] contrabyteHasheada = sha.ComputeHash(contrabyte);
                string contraCadena = BitConverter.ToString(contrabyteHasheada).Replace("-", "");


                using (var bd = new BDPasajeEntities())
                {
                    respuesta = bd.Usuario.Where(p => p.NOMBREUSUARIO == usuario && p.CONTRA == contraCadena).Count().ToString();
                    if (respuesta == "0") respuesta = "Usuario o contrasena incorrecta";
                    else
                    {
                        Session["Usuario"] = bd.Usuario.Where(p => p.NOMBREUSUARIO == usuario && p.CONTRA == contraCadena).Count().ToString();
                    }
                }
            }
            return respuesta;
        }
    }
}