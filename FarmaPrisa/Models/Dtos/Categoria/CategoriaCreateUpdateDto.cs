using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Categoria
{
    public class CategoriaCreateUpdateDto
    {
        [Required]
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

        // Campo opcional para crear subcategorías (FK a la misma tabla)
        public int? CategoriaPadreId { get; set; }
    }
}
