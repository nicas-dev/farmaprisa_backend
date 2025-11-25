using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Products;
using FarmaPrisa.Models.Entities;
using FarmaPrisa.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Repository.Service
{
    public class ProductRepository : IProductRepository
    {
        private readonly FarmaPrisaContext _context;

        public ProductRepository(FarmaPrisaContext context)
        {
            _context = context;
        }

      
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    IdProduct = p.IdProduct,
                    Name = p.Name,
                    Price = p.Price,
                    BarCode = p.BarCode,
                    CategoryId = p.CategoryId,
                    SupplierId = p.SupplierId,
                    BrandId = p.BrandId,
                    IsActive = p.IsActive,
                    Language = p.Language,
                    Details = p.Details.Select(d => new ProductDetailDto
                    {
                        IdProductDetail = d.IdProductDetail,
                        ProductId = d.ProductId,
                        DetailTypeId = d.DetailTypeId,
                        DetailText = d.DetailText
                    }).ToList()
                })
                .ToListAsync();
        }

  
        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Where(p => p.IdProduct == id)
                .Select(p => new ProductDto
                {
                    IdProduct = p.IdProduct,
                    Name = p.Name,
                    Price = p.Price,
                    BarCode = p.BarCode,
                    CategoryId = p.CategoryId,
                    SupplierId = p.SupplierId,
                    BrandId = p.BrandId,
                    IsActive = p.IsActive,
                    Language = p.Language,
                    Details = p.Details.Select(d => new ProductDetailDto
                    {
                        IdProductDetail = d.IdProductDetail,
                        ProductId = d.ProductId,
                        DetailTypeId = d.DetailTypeId,
                        DetailText = d.DetailText
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        //public async Task AddAsync(ProductDto productDto)
        //{
        //    var product = new Product
        //    {
        //        Name = productDto.Name,
        //        Price = productDto.Price,
        //        BarCode = productDto.BarCode,
        //        CategoryId = productDto.CategoryId,
        //        SupplierId = productDto.SupplierId,
        //        BrandId = productDto.BrandId,
        //        IsActive = productDto.IsActive,
        //        Language = productDto.Language
        //    };

        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();
        //}
        public async Task AddAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                BarCode = productDto.BarCode,
                CategoryId = productDto.CategoryId,
                SupplierId = productDto.SupplierId,
                BrandId = productDto.BrandId,
                IsActive = productDto.IsActive,
                Language = productDto.Language,
                Details = productDto.Details?.Select(d => new ProductDetail
                {
                    DetailTypeId = d.DetailTypeId,
                    DetailText = d.DetailText
                }).ToList() ?? new List<ProductDetail>()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Devuelve el ID generado
            productDto.IdProduct = product.IdProduct;
        }


        //public async Task UpdateAsync(ProductDto productDto)
        //{
        //    var product = await _context.Products.FindAsync(productDto.IdProduct);
        //    if (product == null) return;

        //    product.Name = productDto.Name;
        //    product.Price = productDto.Price;
        //    product.BarCode = productDto.BarCode;
        //    product.CategoryId = productDto.CategoryId;
        //    product.SupplierId = productDto.SupplierId;
        //    product.BrandId = productDto.BrandId;
        //    product.IsActive = productDto.IsActive;
        //    product.Language = productDto.Language;

        //    _context.Products.Update(product);
        //    await _context.SaveChangesAsync();
        //}

        public async Task UpdateAsync(ProductDto productDto)
        {
            var product = await _context.Products
                .Include(p => p.Details)
                .FirstOrDefaultAsync(p => p.IdProduct == productDto.IdProduct);

            if (product == null) return;

            // Actualizar campos principales
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.BarCode = productDto.BarCode;
            product.CategoryId = productDto.CategoryId;
            product.SupplierId = productDto.SupplierId;
            product.BrandId = productDto.BrandId;
            product.IsActive = productDto.IsActive;
            product.Language = productDto.Language;

            // Sincronizar detalles
            // Elimina los que ya no existen
            var existingDetailIds = productDto.Details.Select(d => d.IdProductDetail).ToList();
            var detailsToRemove = product.Details.Where(d => !existingDetailIds.Contains(d.IdProductDetail)).ToList();
            _context.ProductDetails.RemoveRange(detailsToRemove);

            // Actualiza o agrega los nuevos
            foreach (var detailDto in productDto.Details)
            {
                var existingDetail = product.Details.FirstOrDefault(d => d.IdProductDetail == detailDto.IdProductDetail);
                if (existingDetail != null)
                {
                    existingDetail.DetailTypeId = detailDto.DetailTypeId;
                    existingDetail.DetailText = detailDto.DetailText;
                }
                else
                {
                    product.Details.Add(new ProductDetail
                    {
                        DetailTypeId = detailDto.DetailTypeId,
                        DetailText = detailDto.DetailText
                    });
                }
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }



        //public async Task DeleteAsync(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null) return;

        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //}


        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Details)
                .FirstOrDefaultAsync(p => p.IdProduct == id);

            if (product == null) return;

            // Primero eliminar detalles asociados
            if (product.Details.Any())
                _context.ProductDetails.RemoveRange(product.Details);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public Task<List<ProductCardDto>> GetProductsByBranchAsync(int branchId, int? categoryId = null, int? brandId = null, string? search = null)
        {
            throw new NotImplementedException();
        }
    }
}
