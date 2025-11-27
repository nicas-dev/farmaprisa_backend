using FarmaPrisa.Models.Dtos.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities
{
    public class Kardex
    {
        [Key]
        public int Id { get; set; }
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Product Producto { get; set; }
        public int SucursalId { get; set; }
        [ForeignKey("SucursalId")]
        public Branch Sucursal { get; set; }
        public int TipoMovimientoId { get; set; }
        [ForeignKey("TipoMovimientoId")]
        public TipoMovimiento TipoMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; } // Positivo o negativo según la lógica, pero el Tipo define si es entrada/salida
        public int SaldoAnterior { get; set; }
        public int SaldoNuevo { get; set; }
        public string Referencia { get; set; } // ID de Venta, Compra, etc.
        public decimal CostoUnitario { get; set; }
        public string Notas { get; set; }
    }
}
