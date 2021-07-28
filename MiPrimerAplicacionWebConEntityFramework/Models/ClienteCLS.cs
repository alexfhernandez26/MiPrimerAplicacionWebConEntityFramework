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
        [Required]
        public string NOMBRE { get; set; }
        [Display(Name = "Apellido")]
        [Required]
        public string APPATERNO { get; set; }
        [Display(Name = "Segundo apellido")]
        [Required]
        public string APMATERNO { get; set; }
        [Display(Name = "Correo")]
        [Required]
        public string EMAIL { get; set; }
        [Display(Name = "Direccion")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string DIRECCION { get; set; }
        [Display(Name = "Sexo")]
        [Required]
        public int IIDSEXO { get; set; }
        [Display(Name = "Telefono")]
        [Required]
        public string TELEFONOFIJO { get; set; }
        [Display(Name = "Celular")]
        [Required]
        public string TELEFONOCELULAR { get; set; }
        public int BHABILITADO { get; set; }
      

    }
}