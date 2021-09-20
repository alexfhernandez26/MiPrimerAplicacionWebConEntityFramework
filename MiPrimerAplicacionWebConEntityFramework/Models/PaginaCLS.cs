using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class PaginaCLS
    {
        [Key]
        public int iidpagina { get; set; }
        [Required]
        [Display(Name = "Mensaje")]
        public string mensaje { get; set; }
        [Required]
        [Display(Name = "Accion")]
        public string accion { get; set; }
        [Required]
        [Display(Name = "Controlador")]
        public string controlador { get; set; }

        public int bhabilitado { get; set; }

        //popiedades adicionales
        public string mensajePagina { get; set; }
    }
}