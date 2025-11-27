using FarmaPrisa.Models.Dtos.Brands;
using FarmaPrisa.Models.Dtos.Producto;
using FarmaPrisa.Models.Dtos.Products;

namespace FarmaPrisa.Models.Dtos.Categoria
{
    public class CategoriaDataDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<GenericBrandDto> Marcas { get; set; }
        public List<ProductDtoInvSucursal> Productos { get; set; }
    }
}
