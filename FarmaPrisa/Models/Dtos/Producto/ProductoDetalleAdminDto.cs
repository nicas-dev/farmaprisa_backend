namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoDetalleAdminDto
    {
        // ID del registro de producto_detalle. 
        // Es NULL si es un nuevo detalle (POST) o tiene valor si es UPDATE.
        public int? Id { get; set; }
        // Usamos el IdentificadorUnico (ej: "INGREDIENTS") que el Admin seleccionó del catálogo
        public string TipoDetalleIdentificador { get; set; }
        public string? ValorDetalle { get; set; }
        public string? MediaUrl { get; set; }
        public string Idioma { get; set; } = "ES";
    }
}
