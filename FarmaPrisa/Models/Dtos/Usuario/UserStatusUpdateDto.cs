using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Usuario
{
    public class UserStatusUpdateDto
    {
        // El administrador debe enviar el nuevo estado de la cuenta
        [Required]
        public bool EstaActivo { get; set; }
    }
}
