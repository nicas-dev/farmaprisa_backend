using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Usuario
{
    public class UserRolesUpdateDto
    {
        /// <summary>
        /// Lista de IDs de los roles que el usuario debe tener. 
        /// Esta lista reemplazará por completo los roles actuales del usuario.
        /// </summary>
        [Required(ErrorMessage = "La lista de roles es obligatoria.")]
        public List<int> RolIds { get; set; } = new List<int>();
    }
}
