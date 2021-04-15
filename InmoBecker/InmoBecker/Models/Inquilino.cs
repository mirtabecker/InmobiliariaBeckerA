using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class Inquilino
    {
		[Display (Name = "Codigo")]
		public int IdInquilino { get; set; }
	
		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
		[Required]
		[Display (Name = "Documento")]
		public string Dni { get; set; }
		[Required]
		public string Telefono { get; set; }
        [Required, EmailAddress]	
		public string Email { get; set; }
		[Required]
		public string Garante { get; set; }
		[Required]
		[Display (Name = "Telefono del Garante")]
		public string TelGarante { get; set; }
	}
}
