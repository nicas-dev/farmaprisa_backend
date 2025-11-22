namespace FarmaPrisa.Models.Dtos.Products
{
    public class ProductImageDto
    {
        public int IdImage { get; set; }
        public string ImageUrl { get; set; } 
        public int Order { get; set; }
        public bool IsMain { get; set; } // Imagen principal
    }
}
