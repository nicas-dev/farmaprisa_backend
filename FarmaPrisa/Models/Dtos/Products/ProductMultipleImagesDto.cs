namespace FarmaPrisa.Models.Dtos.Products
{
    public class ProductMultipleImagesDto
    {
        public int ProductoId { get; set; }

        // Índice de la imagen principal (opcional)
        public int ImagenPrincipalIndex { get; set; }

        // Lista de órdenes, uno por cada imagen
        public List<int> Ordenes { get; set; }

        public List<IFormFile> Imagenes { get; set; }
    }
}
