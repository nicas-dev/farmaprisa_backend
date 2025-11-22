namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoDetalleItemDto
    {
        public string TipoDetalle { get; set; } // e.g., "INGREDIENTS", "WARNINGS"
        public string? ValorDetalle { get; set; } // El texto a mostrar (puede contener HTML)
        public string? MediaUrl { get; set; } // El URL del banner, si aplica
    }
}
