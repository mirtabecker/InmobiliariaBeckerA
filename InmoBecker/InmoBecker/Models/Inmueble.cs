using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public enum enEstados
    {
        Disponible = 1,
        Ocupado = 2,
    }
    public class Inmueble
    {
        [Display(Name = "Codigo")]
        [Key]
        public int IdInmueble { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        [Display(Name = "Cantidad Ambientes")]
        public int Ambientes { get; set; }
        [Required]
        public int Superficie { get; set; }
        public string Tipo { get; set; }
        public int Precio { get; set; }
        [Display(Name = "Estado")]
        public int Estado { get; set; }
        public string Imagen { get; set; }
        [NotMapped]
        public IFormFile ImagenFile { get; set; }

        [ForeignKey(nameof(PropietarioId))]
        [Display(Name = "Propietario")]
        public int PropietarioId { get; set; }
        public Propietario Duenio { get; set; }
        

        public string EstadoNombre => Estado > 0 ? ((enEstados)Estado).ToString().Replace('_', ' ') : "";
        public static IDictionary<int, string> ObtenerEstados()
        {
        SortedDictionary<int, string> estados = new SortedDictionary<int, string>();
        Type tipoEnumEstados = typeof(enEstados);
           
        foreach (var valor in Enum.GetValues(tipoEnumEstados))
            {
                estados.Add((int) valor, Enum.GetName(tipoEnumEstados, valor).Replace('_', ' '));
            }
            return estados;
        }
    }
}
