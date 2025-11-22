using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.OpinionProducto
{
    public class OpinionCreateDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5 estrellas.")]
        public int Calificacion { get; set; }

        [MaxLength(500)]
        public string? Comentario { get; set; }
    }
}
