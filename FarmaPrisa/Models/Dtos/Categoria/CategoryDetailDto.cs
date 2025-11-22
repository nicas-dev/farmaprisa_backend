using FarmaPrisa.Models.Dtos.Brands;
using FarmaPrisa.Models.Dtos.Products;

namespace FarmaPrisa.Models.Dtos.Categoria
{
    public class CategoryDetailDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<GenericBrandDto> Marcas { get; set; } = new();
        public List<ProductDto> Productos { get; set; } = new();
    }
}
