using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class Inmueble
    {  
        [Display (Name = "Codigo")]
        public int IdInmueble { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        [Display (Name = "Cantidad Ambientes")]
        public int Ambientes { get; set; }
        [Required]
        public int Superficie { get; set; }
        [Display (Name = "Propietario")]
        public int PropietarioId { get; set; }
        [ForeignKey(nameof(PropietarioId))]
        [Display(Name = "Propietario")]
        public Propietario Duenio { get; set; }
    }
}
