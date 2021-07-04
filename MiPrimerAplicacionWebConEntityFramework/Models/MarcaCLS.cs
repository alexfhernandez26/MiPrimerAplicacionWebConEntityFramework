using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class MarcaCLS
    {
        [Display(Name ="ID Marca")]
        [Key]
        public int iidmarca { get; set; }
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }
        public int bhabilitado { get; set; }

    }
}