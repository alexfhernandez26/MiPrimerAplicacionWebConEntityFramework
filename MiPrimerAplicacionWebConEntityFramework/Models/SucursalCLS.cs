using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class SucursalCLS
    {
        [Key]
        public int iidsucursal { get; set; }
        [Display(Name ="Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Direccion")]
        public string direccion { get; set; }
        [Display(Name = "Telefono")]
        public string telefono { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Fecha Apertura")]
        public DateTime fechaApertura { get; set; }
        public int bhabilitado { get; set; }

    }
}