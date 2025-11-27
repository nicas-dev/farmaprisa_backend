namespace FarmaPrisa.Models.Dtos.Products
{
    public class ProductDtoInvSucursal
    {

        public int IdProduct { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string BarCode { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }

        public string Marca { get; set; }

        //nuevos campos para poder listar los productos por sucursal por stock
        public int Stock { get; set; }

        public List<ProductDetailDto> Details { get; set; } = new List<ProductDetailDto>();

        public ProductImageDto MainImage { get; set; }     // Imagen principal
        public List<ProductImageDto> Images { get; set; }  // Todas las imágenes
    }
}
