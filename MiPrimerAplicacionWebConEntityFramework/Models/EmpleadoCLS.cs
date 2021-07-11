using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class EmpleadoCLS
    {
        [Key]
        public int iidempleado { get; set; }
        [Required]
        [Display(Name ="Nombre")]
        public string nombre { get; set; }
        [Required]
        [Display(Name = "Apellido Paterno")]
        public string apPaterno { get; set; }
        [Required]
        [Display(Name = "Apellido Materno")]
        public string apMaterno { get; set; }
        [Required]
        [Display(Name = "Fecha Contrato")]
        public DateTime fechaContrato { get; set; }
        [Required]
        [Display(Name = "Tipo Usuario")]
        public int iidtipousuario { get; set; }
        [Required]
        [Display(Name = "Tipo Contrato")]
        public int iidtipocontrato { get; set; }
        [Required]
        [Display(Name = "Sexo")]
        public int iidsexo { get; set; }

        public int bhabilitado { get; set; }

        //Propiedades adicionales
        [Display(Name = "TipoUsuario")]
        public string NombreTipoUsuario { get; set; }

        [Display(Name = "TipoContrato")]
        public string NombreTipoContrato { get; set; }


    }
}