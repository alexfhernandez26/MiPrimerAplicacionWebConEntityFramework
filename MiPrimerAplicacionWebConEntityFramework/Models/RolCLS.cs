using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class RolCLS
    {
        public int iidrol { get; set; }
        [Required]
        [Display(Name ="Nombre")]
        public string nombre { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }
        public int bhabilitado { get; set; }

    }
}