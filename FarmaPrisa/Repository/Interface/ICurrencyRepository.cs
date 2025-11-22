using FarmaPrisa.Models.Dtos.Currency;
using FarmaPrisa.Models.Dtos.Products;
using FarmaPrisa.Models.Entities;

namespace FarmaPrisa.Repository.Interface
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<CurrencyDto>> GetAllAsync();
        Task<CurrencyDto?> GetByIdAsync(int id);
        Task<CurrencyDto> CreateAsync(CreateCurrencyDto dto);
        Task<bool> UpdateAsync(int id, UpdateCurrencyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
