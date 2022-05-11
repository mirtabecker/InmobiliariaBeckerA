using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class Pago
    {
		[Display(Name = "Código")]
		[Key]
		public int IdPago { get; set; }
		[Required]
		[Display(Name = "Nro de Pago")]
		public int NroPago { get; set; }
		[Required]
		[DataType(DataType.Date)]
		public DateTime Fecha { get; set; }
		[Required]
		public decimal Importe { get; set; }
		[Required]
		[Display(Name = "Nro Contrato")]
		public int ContratoId { get; set; }
		[ForeignKey("ContratoId")]
		public Contrato Alquiler { get; set; }
	}
}
