using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class RolPaginaCLS
    {
        [Display(Name ="ID RolPagina")]
        public int iidrolpagina { get; set; }
        public int iidrol { get; set; }
        public int iidpagina { get; set; }
        public int bhabilitado { get; set; }

        //Propiedades adicionales
        [Display(Name = "Nombre Rol")]
        public string nombrerol{ get; set; }
        [Display(Name = "Nombre Pagina")]
        public string nombrepagina { get; set; }

    }
}