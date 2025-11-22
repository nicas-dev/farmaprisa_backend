using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Proveedor
{
    public class ProveedorCreateUpdateDto
    {
        [Required]
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? LogoUrl { get; set; }
        public string? Direccion { get; set; }
        public string? NombreContacto { get; set; }
        [EmailAddress]
        public string? EmailContacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? IdentificacionFiscal { get; set; }
    }
}
