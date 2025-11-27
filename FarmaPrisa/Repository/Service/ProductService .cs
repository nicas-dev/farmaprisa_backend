using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Products;
using FarmaPrisa.Models.Entities;
using FarmaPrisa.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Repository.Service
{
    public class ProductService : IProductRepository
    {
        private readonly FarmaPrisaContext _context;

        public ProductService(FarmaPrisaContext context)
        {
            _context = context;
        }

        //Metodo para devolver todos los productos.
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

            //DESCRIPCIONES
             CategoryName= p.Category != null ? p.Category.Nombre : null,
             SupplierName = p.Supplier != null ? p.Supplier.Nombre : null,
             BrandName = p.Brand != null ? p.Brand.Name : null,

            Details = p.Details.Select(d => new ProductDetailDto
            {
                IdProductDetail = d.IdProductDetail,
                ProductId = d.ProductId,
                DetailTypeId = d.DetailTypeId,
                DetailText = d.DetailText
            }).ToList(),

            //IMAGEN PRINCIPAL
            MainImage = p.Imagenes
                .Where(img => img.EsPrincipal)
                .Select(img => new ProductImageDto
                {
                    IdImage = img.Id,
                    ImageUrl = img.UrlImagen,
                    Order = img.Orden,
                    IsMain = img.EsPrincipal
                }).FirstOrDefault(),

            //TODAS LAS IMÁGENES
            Images = p.Imagenes.Select(img => new ProductImageDto
            {
                IdImage = img.Id,
                ImageUrl = img.UrlImagen,
                Order = img.Orden,
                IsMain = img.EsPrincipal
            }).ToList()
        })
        .ToListAsync();
        }

        //Metodo para devolver productos por ID
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

           //DESCRIPCIONES
           CategoryName = p.Category != null ? p.Category.Nombre : null,
           SupplierName = p.Supplier != null ? p.Supplier.Nombre : null,
           BrandName = p.Brand != null ? p.Brand.Name : null,

           Details = p.Details.Select(d => new ProductDetailDto
           {
               IdProductDetail = d.IdProductDetail,
               ProductId = d.ProductId,
               DetailTypeId = d.DetailTypeId,
               DetailText = d.DetailText
           }).ToList(),

           //IMAGEN PRINCIPAL
           MainImage = p.Imagenes
               .Where(img => img.EsPrincipal)
               .Select(img => new ProductImageDto
               {
                   IdImage = img.Id,
                   ImageUrl = img.UrlImagen,
                   Order = img.Orden,
                   IsMain = img.EsPrincipal
               }).FirstOrDefault(),

           //TODAS LAS IMÁGENES
           Images = p.Imagenes.Select(img => new ProductImageDto
           {
               IdImage = img.Id,
               ImageUrl = img.UrlImagen,
               Order = img.Orden,
               IsMain = img.EsPrincipal
           }).ToList()
       })
       .FirstOrDefaultAsync();
        }

        //Medodo para guardar un producto.
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

        //Metodo para actualizar un producto.
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

        //Metodo para desactivar un producto.
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

    


        //// metodo para listar productos tomando en cuenta de sucursal
        // public async Task<List<ProductCardDto>> GetProductsByBranchAsync(int branchId, int? categoryId = null, int? brandId = null, string? search = null)
        // {
        //     // 1. Iniciamos la consulta
        //     var query = _context.Inventories
        //         .Include(inv => inv.Product)
        //             .ThenInclude(p => p.Brand)
        //         .Include(inv => inv.Product)
        //             .ThenInclude(p => p.Category)
        //         .Include(inv => inv.InventoryDetails) // <--- IMPORTANTE: Incluir detalles para poder sumar stock/precio
        //         .Where(inv => inv.IdBranch == branchId
        //                    && inv.IsActive
        //                    && inv.Product.IsActive == true);

        //     // 2. Filtros
        //     if (categoryId.HasValue)
        //     {
        //         query = query.Where(inv => inv.Product.CategoryId == categoryId.Value);
        //     }

        //     if (brandId.HasValue)
        //     {
        //         query = query.Where(inv => inv.Product.BrandId == brandId.Value);
        //     }

        //     if (!string.IsNullOrEmpty(search))
        //     {
        //         query = query.Where(inv => inv.Product.Name.Contains(search)
        //                                 || inv.Product.BarCode.Contains(search));
        //     }

        //     // 3. Proyección
        //     var resultados = await query
        //         .Select(inv => new ProductCardDto
        //         {
        //             ProductId = inv.Product.IdProduct,
        //             Name = inv.Product.Name,
        //             // Manejo seguro de imagen (si la lista es null o vacía)
        //             ImageUrl = inv.Product.Imagenes != null && inv.Product.Imagenes.Any()
        //                        ? inv.Product.Imagenes.FirstOrDefault().UrlImagen
        //                        : "",
        //             BrandName = inv.Product.Brand.Name,
        //             CategoryName = inv.Product.Category.Nombre,

        //             // --- LÓGICA DE INVENTARIO DETALLADO ---

        //             // Sumar stock de todos los detalles (lotes)
        //             AvailableStock = inv.InventoryDetails.Sum(d => d.stock),

        //             // Tomar el precio del primer detalle disponible (o 0 si no hay)
        //             // Asumiendo que la propiedad en InventoryDetails se llama 'Price' o 'SalePrice'
        //             CurrentPrice = inv.InventoryDetails
        //                               .OrderByDescending(d => d.stock) // Priorizamos el precio del lote con más stock
        //                               .Select(d => d.Price) // <--- Verifica si se llama 'Price' o 'Precio'
        //                               .FirstOrDefault(),

        //             // --- EXTRAS (Comentados hasta que tengas las propiedades) ---
        //             // IsNew = inv.Product.CreatedAt > DateTime.Now.AddDays(-30),
        //             // RatingPromedio = ...
        //         })
        //         .OrderByDescending(p => p.AvailableStock > 0) // Mostrar disponibles primero
        //         .ThenBy(p => p.Name)
        //         .ToListAsync();

        //     return resultados;
        // }





    }
}
