using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Promocion
{
    public class PromocionCreateDto
    {
        // Propiedades de la tabla Promociones
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string CodigoCupon { get; set; } // CRÍTICO: Debe ser único

        [Required]
        public string TipoDescuento { get; set; } // 'porcentaje' o 'fijo'

        [Required]
        [Range(0.01, 100000.00)]
        public decimal ValorDescuento { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public string? Descripcion { get; set; }

        // Propiedades de las tablas pivote (Asignaciones)
        public List<int> ProductoIds { get; set; } = new List<int>(); // Productos específicos
        public List<int> CategoriaIds { get; set; } = new List<int>(); // Categorías enteras
    }
}
