using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Brands;
using FarmaPrisa.Models.Entities;
using FarmaPrisa.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Repository.Service
{
    public class BrandService: IBrandRepository
    {

        private readonly FarmaPrisaContext _context;

        public BrandService(FarmaPrisaContext context)
        {
            _context = context;
        }

        //netodo para listar todas las marcas
        public async Task<IEnumerable<GenericBrandDto>> GetAllBrandsAsync()
        {
            return await _context.Brands
                     .Where(b => b.IsActive)
                    .Select(b => new GenericBrandDto
                    {
                        IdBrand = b.IdBrand,
                        Name = b.Name,
                        IsActive= b.IsActive,
                        
                      
                    })
                    .ToListAsync();
        }

        //metodo para buscar listar marcas por id
        public async Task<GenericBrandDto?> GetByIdAsync(int id)
        {
            var brand = await _context.Brands
            .Where(b => b.IdBrand == id && b.IsActive)
            .FirstOrDefaultAsync();

            if (brand == null) return null;

            return new GenericBrandDto
            {
                IdBrand = brand.IdBrand,
                Name = brand.Name,
                IsActive = brand.IsActive,

            };
        }
        //metodo para guardar marcas
        public async Task AddAsync(GenericBrandDto brandDto)
        {
            var brand = new Brand
            {
                Name = brandDto.Name,
               
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        //metodo para actualizar marcas
        public async Task UpdateAsync(GenericBrandDto brandDto)
        {
            var brand = await _context.Brands.FindAsync(brandDto.IdBrand);

            if (brand == null || !brand.IsActive)
                throw new Exception("Marca no encontrada");

            brand.Name = brandDto.Name;
           

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        //metodo para desactivar marcas
        public async Task DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
                throw new Exception("Marca no encontrada");

            // Soft delete
            brand.IsActive = false;

            await _context.SaveChangesAsync();
        }
    }
}
