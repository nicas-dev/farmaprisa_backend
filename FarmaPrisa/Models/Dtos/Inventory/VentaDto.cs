using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Inventory
{
    public class VentaDto
    {
        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int SucursalId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
    }
}
