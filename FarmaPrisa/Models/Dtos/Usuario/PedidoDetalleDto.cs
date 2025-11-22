namespace FarmaPrisa.Models.Dtos.Usuario
{
    public class PedidoDetalleDto
    {
        // Datos del Pedido (pedidos)
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; }
        public string TipoEntrega { get; set; }
        public decimal Subtotal { get; set; }
        public decimal CostoEnvio { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal MontoImpuesto { get; set; }
        public decimal Total { get; set; }

        // Información del Cliente y Ubicación
        public int UsuarioId { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteEmail { get; set; }
        public string? SucursalRecogidaNombre { get; set; }
        public string? DireccionCompletaEnvio { get; set; }

        // Artículos del Pedido (detalles_pedido)
        public List<DetallePedidoItemDto> Articulos { get; set; } = new List<DetallePedidoItemDto>();

        // Información de Pago (transacciones)
        public string? EstadoTransaccion { get; set; }
        public string? PasarelaPago { get; set; }
    }
}
