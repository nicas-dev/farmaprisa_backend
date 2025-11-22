namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoUpdateDto
    {
        // Propiedades de Producto que pueden cambiar. Usamos int? y string? para ser opcionales.
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; } // Hacemos precio opcional para que el admin solo envíe lo que cambia.
        public decimal? PrecioAnterior { get; set; }
        //public string? ImagenUrl { get; set; }
        public string? Sku { get; set; }
        public bool? RequiereReceta { get; set; }
        public int? CategoriaId { get; set; }
        public int? ProveedorId { get; set; }
        public bool? EstaActivo { get; set; }

        // La colección completa de detalles que el administrador quiere que tenga el producto
        public List<ProductoDetalleAdminDto>? Detalles { get; set; }
    }
}
