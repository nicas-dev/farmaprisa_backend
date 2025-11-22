namespace FarmaPrisa.Models.Dtos.Usuario
{
    using System.ComponentModel.DataAnnotations;

    public class UsuarioAdminCreateDto
    {
        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public int PaisId { get; set; }

        // Lista de nombres de roles a asignar (ej: "administrador", "entregador")
        public List<string> Roles { get; set; } = new List<string>();
    }
}
