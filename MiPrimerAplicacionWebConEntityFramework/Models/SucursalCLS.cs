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
        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }
        [Display(Name = "Direccion")]
        [Required]
        public string direccion { get; set; }
        [Display(Name = "Telefono")]
        [Required]
        public string telefono { get; set; }
        [Display(Name = "Email")]
        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        public string email { get; set; }
        //Esto es para darle forma de fecha al editfor osea para que salga el control fecha en la caja de texto
        //Le estamos especificando que va a ser un campo de tipo fecha
        //[DataType(DataType.Date)]
        [Display(Name = "Fecha Apertura")]
        [Required]
        [DataType(DataType.Date)]

        //Este es otro tag para la insersion de fecha en la BD, osea estamos indicando el formato de la fecha para
        //que no tengamos problemas al insertar, sin este [DisplayFormat] no va a insertar y dara problemas
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime fechaApertura { get; set; }
        public int bhabilitado { get; set; }

        //Propiedad adicional para mandar mensaje de error en caso de que se repita el nombreMarca
        public string mensajeError  { get; set; }

    }
}