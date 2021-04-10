using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class Inmueble
    {  
        public int IdInmueble { get; set; }
        public string Direccion { get; set; }
        public int Ambientes { get; set; }
        public int Superficie { get; set; }
        public int PropietarioId { get; set; }
        [ForeignKey(nameof(PropietarioId))]
        public Propietario Duenio { get; set; }
    }
}
