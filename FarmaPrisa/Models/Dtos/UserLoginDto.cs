using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "El correo no es válido: example@gmail.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Se debe de ingresar la contraseña")]
        public string Password { get; set; }
    }
}
