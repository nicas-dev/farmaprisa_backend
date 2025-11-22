namespace FarmaPrisa.Models.Dtos.Proveedor
{
    public class ProveedorAdminDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? LogoUrl { get; set; }

        // Información de Contacto y Fiscal
        public string? Direccion { get; set; }
        public string? NombreContacto { get; set; }
        public string? EmailContacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? IdentificacionFiscal { get; set; }
        public bool EstaActivo { get; set; }
    }
}
