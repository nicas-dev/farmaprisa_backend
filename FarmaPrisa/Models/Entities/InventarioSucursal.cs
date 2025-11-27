using FarmaPrisa.Models.Dtos.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities
{
    public class InventarioSucursal
    {
        [Key]
        public int Id { get; set; }

        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Product Producto { get; set; }

        public int SucursalId { get; set; }
        [ForeignKey("SucursalId")]
        public Branch Sucursal { get; set; }

        // Suma total de CantidadRestante de todos los ingresos activos
        public int StockTotal { get; set; }

        public DateTime UltimaActualizacion { get; set; }
    }
}
