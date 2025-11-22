namespace FarmaPrisa.Models.Dtos
{
    // Este DTO define la estructura de datos que enviaremos al cliente.
    public class UserDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; } // Agregamos el teléfono
        public bool EstaActivo { get; set; }
        public int? ProveedorId { get; set; }
        public int? PaisId { get; set; }
        public int PuntosFidelidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}