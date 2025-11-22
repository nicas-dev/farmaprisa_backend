using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoCreateDto
    {
        // Propiedades de la tabla Productos
        [Required]
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public decimal Precio { get; set; }
        public decimal? PrecioAnterior { get; set; }
        //public string? ImagenUrl { get; set; }
        [Required]
        public string Sku { get; set; }
        public bool RequiereReceta { get; set; } = false;
        public int? CategoriaId { get; set; }
        public int? ProveedorId { get; set; }

        // Asumimos que los productos nuevos siempre están activos por defecto
        public bool EstaActivo { get; set; } = true;

        // Colección de detalles para la tabla producto_detalle
        public List<ProductoDetalleAdminDto>? Detalles { get; set; }
    }
}
