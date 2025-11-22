using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Brands;
using FarmaPrisa.Models.Dtos.Currency;
using FarmaPrisa.Models.Entities;
using FarmaPrisa.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Repository.Service
{
    public class CurrencyService: ICurrencyRepository
    {

        private readonly FarmaPrisaContext _context;

        public CurrencyService(FarmaPrisaContext context)
        {
            _context = context;
        }

        //metodo para listar
        public async Task<IEnumerable<CurrencyDto>> GetAllAsync()
        {
            return await _context.Currencys
                .Select(c => new CurrencyDto
                {
                    IdCurrency = c.IdCurrency,
                    Name = c.CurrencyName,
                    Symbol = c.Currencysymbol,
                    IsActive = c.IsActive
                }).ToListAsync();
        }

        //Metodo para listar por id
        public async Task<CurrencyDto?> GetByIdAsync(int id)
        {
            var currency = await _context.Currencys
                .FirstOrDefaultAsync(c => c.IdCurrency == id);

            if (currency == null) return null;

            return new CurrencyDto
            {
                IdCurrency = currency.IdCurrency,
                Name = currency.CurrencyName,
                Symbol = currency.Currencysymbol,
                IsActive = currency.IsActive
            };
        }

        //Metodo para crear monedas
        public async Task<CurrencyDto> CreateAsync(CreateCurrencyDto dto)
        {
            var currency = new Currency
            {
                CurrencyName = dto.Name,
                Currencysymbol = dto.Symbol,
                IsActive = dto.IsActive
            };

            _context.Currencys.Add(currency);
            await _context.SaveChangesAsync();

            return new CurrencyDto
            {
                IdCurrency = currency.IdCurrency,
                Name = currency.CurrencyName,
                Symbol = currency.Currencysymbol,
                IsActive = currency.IsActive
            };
        }

        //Metodo para actualizar monedas
        public async Task<bool> UpdateAsync(int id, UpdateCurrencyDto dto)
        {
            var currency = await _context.Currencys.FindAsync(id);
            if (currency == null) return false;

            currency.CurrencyName = dto.Name;
            currency.Currencysymbol = dto.Symbol;
            currency.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        //Metodo para desactivar monedas
        public async Task<bool> DeleteAsync(int id)
        {
            var currency = await _context.Currencys.FindAsync(id);
            if (currency == null) return false;

            _context.Currencys.Remove(currency);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
