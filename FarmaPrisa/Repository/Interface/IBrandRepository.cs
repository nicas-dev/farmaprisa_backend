using FarmaPrisa.Models.Dtos.Brands;
using FarmaPrisa.Models.Entities;

namespace FarmaPrisa.Services.Interface
{
    public interface IBrandRepository
    {
        Task<IEnumerable<GenericBrandDto>> GetAllBrandsAsync();
        Task<GenericBrandDto?> GetByIdAsync(int id);
        Task AddAsync(GenericBrandDto brandDto);
        Task UpdateAsync(GenericBrandDto brandDto);
        Task DeleteAsync(int id);

    }
}
