using FarmaPrisa.Models.Dtos.Products;
using FarmaPrisa.Models.Entities;

namespace FarmaPrisa.Services.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task AddAsync(ProductDto productDto);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(int id);


    }
}
