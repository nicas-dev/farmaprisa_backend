namespace FarmaPrisa.Models.Dtos
{
    /// <summary>
    /// DTO con los campos que se pueden actualizar de un usuario.
    /// </summary>
    public class UserUpdateDto
    {
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public bool EstaActivo { get; set; }
        public int PuntosFidelidad { get; set; }
        public int? ProveedorId { get; set; }
        public int? PaisId { get; set; }
    }
}
