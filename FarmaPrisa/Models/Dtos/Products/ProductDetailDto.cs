namespace FarmaPrisa.Models.Dtos.Products
{
    public class ProductDetailDto
    {
        public int IdProductDetail { get; set; }
        public int ProductId { get; set; }
        public int DetailTypeId { get; set; }
        public string DetailText { get; set; }
    }
}
