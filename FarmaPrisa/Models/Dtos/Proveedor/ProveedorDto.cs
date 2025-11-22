namespace FarmaPrisa.Models.Dtos.Proveedor
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? LogoUrl { get; set; } // Opcional, si el menú muestra logos
        public bool? EstaActivo { get; set; }
    }
}
