namespace FarmaPrisa.Models.Dtos.Products
{
    public class ProductCardDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        // Branch-specific data
        public decimal CurrentPrice { get; set; }
        public decimal AvailableStock { get; set; }
        public bool IsOutOfStock => AvailableStock <= 0;

        // Tags and Extras
        public bool IsNew { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }

    }
}
