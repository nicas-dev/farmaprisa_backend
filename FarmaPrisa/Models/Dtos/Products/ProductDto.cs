namespace FarmaPrisa.Models.Dtos.Products
{
    public class ProductDto
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string BarCode { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public int BrandId { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }

        public List<ProductDetailDto> Details { get; set; } = new List<ProductDetailDto>();

        public ProductImageDto MainImage { get; set; }     // Imagen principal
        public List<ProductImageDto> Images { get; set; }  // Todas las imágenes
    }
}
