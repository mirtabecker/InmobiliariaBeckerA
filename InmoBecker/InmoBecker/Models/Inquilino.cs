﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class Inquilino
    {
		public int IdInquilino { get; set; }
	
		public string Nombre { get; set; }
		
		public string Apellido { get; set; }
		
		public string Dni { get; set; }
		public string Telefono { get; set; }
	
		public string Email { get; set; }
		public string Garante { get; set; }
		public string TelGarante { get; set; }
	}
}
