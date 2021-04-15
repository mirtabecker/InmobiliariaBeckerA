﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class Propietario
    {
        [Display (Name = "Codigo")]
        public int IdPropietario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [Display(Name = "Documento")]
        public string Dni { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Clave { get; set; }
    }
}
