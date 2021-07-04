using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class ClienteCLS
    {
        [Key]
        [Display(Name ="Id Cliente")]
        public int iidCliente { get; set; }
        [Display(Name = "Nombre")]
        public string NOMBRE { get; set; }
        [Display(Name = "Apellido")]
        public string APPATERNO { get; set; }
        [Display(Name = "Segundo apellido")]
        public string APMATERNO { get; set; }
        [Display(Name = "Correo")]
        public string EMAIL { get; set; }
        [Display(Name = "Direccion")]
        public string DIRECCION { get; set; }
        [Display(Name = "Sexo")]
        public int IIDSEXO { get; set; }
        [Display(Name = "Telefono")]
        public string TELEFONOFIJO { get; set; }
        [Display(Name = "Celular")]
        public string TELEFONOCELULAR { get; set; }
        public int BHABILITADO { get; set; }
      

    }
}