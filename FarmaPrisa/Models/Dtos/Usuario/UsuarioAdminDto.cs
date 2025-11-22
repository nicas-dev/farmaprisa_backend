namespace FarmaPrisa.Models.Dtos.Usuario
{
    public class UsuarioAdminDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string? Telefono { get; set; }
        public bool? EstaActivo { get; set; }
        public int PuntosFidelidad { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Lista de roles asignados (cliente, administrador, entregador, etc.)
        public List<string> Roles { get; set; }
    }
}
