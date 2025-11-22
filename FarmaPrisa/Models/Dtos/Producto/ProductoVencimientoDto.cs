namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoVencimientoDto
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; } = null!;
        public int StockActual { get; set; } // Asumimos Stock de InventarioSucursal (o total)
        public string Sku { get; set; } = null!;
        public DateTime? FechaVencimiento { get; set; }
        public int DiasParaExpirar { get; set; } // Calculado en el servicio
        public int StockMinimo { get; set; } // El valor de seguridad
        public bool AlertaStockBajo { get; set; } // Alerta calculada
    }
}
