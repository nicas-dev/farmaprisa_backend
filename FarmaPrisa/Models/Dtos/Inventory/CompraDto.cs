using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Inventory
{
    public class CompraDto
    {
        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int SucursalId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        public string Lote { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [Required]
        public decimal Costo { get; set; }
    }
}
