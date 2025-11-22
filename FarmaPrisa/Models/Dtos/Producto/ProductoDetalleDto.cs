using FarmaPrisa.Models.Dtos.OpinionProducto;

namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoDetalleDto
    {
        // === Datos Básicos del Producto ===
        public int Id { get; set; }
        public string Nombre { get; set; } = null!; // Se asume que no debe ser null
        public string Descripcion { get; set; } = null!; // Descripción principal (traducida de producto_detalle)
        public decimal Precio { get; set; }
        public decimal? PrecioAnterior { get; set; }
        public string Sku { get; set; } = null!;
        public bool RequiereReceta { get; set; }
        public bool EstaActivo { get; set; } // Estado actual del producto
        public string ProveedorNombre { get; set; } = null!;

        // === Galería de Imágenes (Colección para la Galería) ===
        // Devuelve todas las imágenes del producto con su orden de visualización.
        public List<ProductoImagenDto> ImagenesGaleria { get; set; } = new List<ProductoImagenDto>();

        // === Detalles Multilingües (Ingredientes, Advertencias, etc.) ===
        // Colección de detalles técnicos/legales (filtrada por idioma en el servicio)
        public List<ProductoDetalleItemDto> Especificaciones { get; set; } = new List<ProductoDetalleItemDto>();

        // === Opiniones y Valoración ===
        public List<OpinionDto> Opiniones { get; set; } = new List<OpinionDto>();
        public double CalificacionPromedio { get; set; }

        // === Datos de Pedido/Logística (Opcionales) ===
        public string? SucursalRecogidaNombre { get; set; } // Nombre de sucursal para "Recoger en tienda"
        public string? DireccionCompletaEnvio { get; set; } // Dirección del cliente
        public string? EstadoTransaccion { get; set; } // Estado de pago (para administración o historial)
    }
}
