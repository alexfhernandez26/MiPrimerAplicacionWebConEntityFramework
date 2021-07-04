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
        [Required]
        [StringLength(100,ErrorMessage ="no debe pasar de 100 caracteres")]
        public string nombre { get; set; }
        [Display(Name = "Descripcion")]
        [Required]
        [StringLength(200, ErrorMessage = "no debe pasar de 200 caracteres")]
        public string descripcion { get; set; }
        public int bhabilitado { get; set; }

    }
}