namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        //public string ImagenUrl { get; set; }
        public List<ProductoImagenDto> ImagenesGaleria { get; set; } = new List<ProductoImagenDto>();
        public bool RequiereReceta { get; set; } // Para mostrar icono de requerimiento
        public string ProveedorNombre { get; set; } // Nombre del proveedor/laboratorio
        public bool EstaActivo { get; set; }
    }
}
