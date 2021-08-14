using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class ViajeCLS
    {
        [Key]
        [Display(Name ="ID Viaje")]
        public int iidviaje { get; set; }
        [Required]
        [Display(Name ="Lugar Origen")]
        public int iidlugarorigen { get; set; }
        [Required]
        [Display(Name = "Lugar Destino")]
        public int iidlugardestino { get; set; }
        [Required]
        [Display(Name = "Precio")]
        public int precio { get; set; }
        [Required]
        [Display(Name = "Fecha Viaje")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaViaje { get; set; }
        [Required]
        [Display(Name = "Nombre bus")]
        public int iidbus { get; set; }
        [Required]
        [Display(Name = "Asientos Disponibles")]
        public int numeroAsientosDisponibles { get; set; }
        public int bhabilitado { get; set; }

        //Propiedades adicionales
        [Display(Name ="Lugar de Origen")]
        public string NombrelugarOrigen { get; set; }
        [Display(Name = "Lugar de destino   ")]
        public string NombrelugarDestino { get; set; }
        [Display(Name = "Nombre de Bus")]
        public string nombreBus { get; set; }


    }
}