using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Se debe de ingresar un correo electrónico válido")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "La contraseña debe de contener como mínimo 6 caracteres")]
        public string Password { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del país debe ser un número positivo.")]
        public int PaisId { get; set; }
    }
}
