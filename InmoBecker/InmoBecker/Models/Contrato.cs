using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class Contrato
    {
        [Display(Name ="Codigo")]
        public int IdContrato { get; set; }
        public int Monto { get; set; }
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Fecha de Cierre")]
        public DateTime FechaCierre { get; set; }
        [Display(Name ="Inquilino")]
        public int InquilinoId { get; set; }
        [ForeignKey(nameof(InquilinoId))]
        [Display(Name = "Direccion")]
        public int InmuebleId { get; set; }
        [ForeignKey(nameof(InmuebleId))]
        public Inquilino Inquilino { get; set; }
        public Inmueble Inmueble { get; set; }




    }
}
