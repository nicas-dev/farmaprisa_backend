using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos
{
    public class StatusUpdateDto
    {
        [Required]
        public bool EstaActiva { get; set; } // El nuevo estado (true o false)
    }
}
