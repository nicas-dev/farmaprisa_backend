using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Producto
{
    public class ProductoGaleriaUploadDto
    {
        // El ID del producto al que se añadirán las imágenes
        [Required]
        public int ProductoId { get; set; }

        // La colección de archivos subidos
        [Required]
        public ICollection<IFormFile> ArchivosGaleria { get; set; } = new List<IFormFile>();
    }
}
