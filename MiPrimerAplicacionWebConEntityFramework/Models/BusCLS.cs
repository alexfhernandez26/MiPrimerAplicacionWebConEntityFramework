using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerAplicacionWebConEntityFramework.Models
{
    public class BusCLS
    {
        [Key]
        [Display(Name = "ID Bus")]
        public int iidbus { get; set; }
        [Required]
        [Display(Name ="ID Sucursal")]
        public int iidsucursal { get; set; }
        [Required]
        [Display(Name = "ID TipoBus")]
        public int iidtipobus { get; set; }
        [Required]
        [Display(Name = "Placa")]
        public string placa { get; set; }
        [Required]
        [Display(Name = "Fecha de Compra")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechacompra { get; set; }
        [Required]
        [Display(Name = "ID Modelo")]
        public int iidmodelo { get; set; }
        [Required]
        [Display(Name = "Numero Filas")]
        public int numeroFilas { get; set; }
        [Required]
        [Display(Name = "Numero Columnas")]
        public int numeroColumnas { get; set; }
        public int bhabilitado { get; set; }
        [Required]
        [Display(Name = "ID Marca")]
        public string iidmarca { get; set; }

        //Nombre a las propiedades adicionales
        [Display(Name = "Nombre Sucursal")]
        public string NombreSucursal { get; set; }
        [Display(Name = "Nombre TipoBus")]
        public string NombreTipoBus { get; set; }
        [Display(Name = "Nombre de Modelo")]
        public string NombreModelo { get; set; }
        [Display(Name = "Nombre de Marca")]
        public string NombreMarca{ get; set; }


    }
}