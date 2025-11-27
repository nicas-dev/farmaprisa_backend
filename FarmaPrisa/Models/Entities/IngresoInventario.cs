using FarmaPrisa.Models.Dtos.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities
{
    public class IngresoInventario
    {
        [Key]
        public int Id { get; set; }

        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Product Producto { get; set; }

        public int SucursalId { get; set; }
        [ForeignKey("SucursalId")]
        public Branch Sucursal { get; set; }

        public string Lote { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public int CantidadInicial { get; set; }

        // Esta es la cantidad disponible actual de ESTE lote específico
        public int CantidadRestante { get; set; }

        public DateTime FechaIngreso { get; set; }

        public decimal CostoUnitario { get; set; }
    }
}
