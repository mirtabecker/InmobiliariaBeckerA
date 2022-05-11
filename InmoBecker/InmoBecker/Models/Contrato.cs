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
        [Key]
        public int IdContrato { get; set; }
        public int Monto { get; set; }
        [Display(Name = "Inicia")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Finaliza")]
        public DateTime FechaCierre { get; set; }
        [Display(Name ="Inquilino")]
        public int InquilinoId { get; set; }
        [ForeignKey("InquilinoId")]
        [Display(Name = "Direccion")]
        public int InmuebleId { get; set; }
        [ForeignKey("InmuebleId")]
        public Inquilino Inquilino { get; set; }
        public Inmueble Inmueble { get; set; }




    }
}
