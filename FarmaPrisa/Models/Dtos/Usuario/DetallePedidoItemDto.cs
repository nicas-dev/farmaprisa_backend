namespace FarmaPrisa.Models.Dtos.Usuario
{
    public class DetallePedidoItemDto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string ImagenUrl { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; } // Cantidad * PrecioUnitario
    }
}
