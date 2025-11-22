using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Brands;
using FarmaPrisa.Models.Dtos.Categoria;
using FarmaPrisa.Models.Dtos.Products;
using FarmaPrisa.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Services
{
    public interface ICategoriaService
    {
        Task<CategoriaDto> CreateCategoriaAsync(CategoriaCreateUpdateDto dto);
        Task<IEnumerable<CategoriaDto>> GetCategoriasJerarquiaAsync();
        Task<bool> UpdateCategoriaAsync(int id, CategoriaCreateUpdateDto dto);

        Task<CategoryDetailDto?> GetCategoriaDetallesAsync(int categoriaId);
    }
    public class CategoriaService : ICategoriaService
    {
        private readonly FarmaPrisaContext _context;

        public CategoriaService(FarmaPrisaContext context)
        {
            _context = context;
        }

        public async Task<CategoriaDto> CreateCategoriaAsync(CategoriaCreateUpdateDto dto)
        {
            var nuevaCategoria = new Categoria
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                CategoriaPadreId = dto.CategoriaPadreId
            };

            _context.Categorias.Add(nuevaCategoria);
            await _context.SaveChangesAsync();

            // Mapeamos la entidad guardada al DTO y la devolvemos
            var categoriaDto = new CategoriaDto
            {
                Id = nuevaCategoria.Id,
                Nombre = nuevaCategoria.Nombre,
                Descripcion = nuevaCategoria.Descripcion,
                CategoriaPadreId = nuevaCategoria.CategoriaPadreId,
                // Subcategorias será null, ya que se acaba de crear
                Subcategorias = null
            };

            return categoriaDto;
        }

        ////metodo para filtro de categorias ---METODO ANTERIOR SIN IMAGENES-----
        //public async Task<CategoryDetailDto?> GetCategoriaDetallesAsync(int categoriaId)
        //{
        //    var categoria = await _context.Categorias
        //        .Where(c => c.Id == categoriaId)
        //        .FirstOrDefaultAsync();

        //    if (categoria == null)
        //        return null;

        //    // Productos de la categoría
        //    var productos = await _context.Products
        //        .Where(p => p.CategoryId == categoriaId && p.IsActive)
        //        .Select(p => new ProductDto
        //        {
        //            IdProduct = p.IdProduct,
        //            Name = p.Name,
        //            Price = p.Price,
        //            BarCode = p.BarCode,
        //            CategoryId = p.CategoryId,
        //            SupplierId = p.SupplierId,
        //            BrandId = p.BrandId,
        //            IsActive = p.IsActive,
        //            Language = p.Language
        //        })
        //        .ToListAsync();

        //    // Obtener marcas únicas asociadas a los productos
        //    var marcas = productos
        //        .Select(p => p.BrandId)
        //        .Distinct()
        //        .ToList();

        //    var listaMarcas = await _context.Brands
        //        .Where(b => marcas.Contains(b.IdBrand))
        //        .Select(b => new GenericBrandDto
        //        {
        //            IdBrand = b.IdBrand,
        //            Name = b.Name,
        //            IsActive = true
        //        })
        //        .ToListAsync();

        //    return new CategoryDetailDto
        //    {
        //        Id = categoria.Id,
        //        Nombre = categoria.Nombre,
        //        Marcas = listaMarcas,
        //        Productos = productos
        //    };
        //}

        //metodo para filtro de categorias y trae imagen principal y todas las  imagenes
        public async Task<CategoryDetailDto?> GetCategoriaDetallesAsync(int categoriaId)
        {
            var categoria = await _context.Categorias
                .Where(c => c.Id == categoriaId)
                .FirstOrDefaultAsync();

            if (categoria == null)
                return null;

            // Productos de la categoría
            var productos = await _context.Products
                .Where(p => p.CategoryId == categoriaId && p.IsActive)
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

            // Obtener marcas únicas asociadas a los productos
            var marcas = productos
                .Select(p => p.BrandId)
                .Distinct()
                .ToList();

            var listaMarcas = await _context.Brands
                .Where(b => marcas.Contains(b.IdBrand))
                .Select(b => new GenericBrandDto
                {
                    IdBrand = b.IdBrand,
                    Name = b.Name,
                    IsActive = true
                })
                .ToListAsync();

            return new CategoryDetailDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Marcas = listaMarcas,
                Productos = productos
            };
        }


        public async Task<IEnumerable<CategoriaDto>> GetCategoriasJerarquiaAsync()
        {
            // Carga todas las categorías a la vez para evitar múltiples consultas a la BD
            var todasCategorias = await _context.Categorias.ToListAsync();

            // Mapeamos todas las entidades a DTOs
            var categoriaDtos = todasCategorias.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                CategoriaPadreId = c.CategoriaPadreId,
                Subcategorias = new List<CategoriaDto>() // Inicializamos la lista de hijos
            }).ToList();

            // Estructura para búsqueda rápida
            var diccionario = categoriaDtos.ToDictionary(c => c.Id);
            var categoriasRaiz = new List<CategoriaDto>();

            // Construimos la jerarquía
            foreach (var dto in categoriaDtos)
            {
                if (dto.CategoriaPadreId.HasValue && diccionario.ContainsKey(dto.CategoriaPadreId.Value))
                {
                    // Es una subcategoría: la añadimos a la lista de hijos de su padre
                    diccionario[dto.CategoriaPadreId.Value].Subcategorias.Add(dto);
                }
                else
                {
                    // Es una categoría raíz o su padre no existe: la añadimos a la lista principal
                    categoriasRaiz.Add(dto);
                }
            }

            return categoriasRaiz;
        }

        public async Task<bool> UpdateCategoriaAsync(int id, CategoriaCreateUpdateDto dto)
        {
            var categoriaAActualizar = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoriaAActualizar == null)
            {
                return false; // Categoría no encontrada
            }

            // Aplicar solo los cambios que se envían en el DTO
            if (!string.IsNullOrEmpty(dto.Nombre))
            {
                categoriaAActualizar.Nombre = dto.Nombre;
            }

            // La descripción puede ser vacía o nula, lo validamos si se envió
            if (dto.Descripcion != null)
            {
                categoriaAActualizar.Descripcion = dto.Descripcion;
            }

            // El CategoriaPadreId puede ser null (cambiar a categoría principal) o un ID
            if (dto.CategoriaPadreId.HasValue)
            {
                // Si el ID del padre es 0, lo tratamos como NULL (categoría raíz)
                categoriaAActualizar.CategoriaPadreId = (dto.CategoriaPadreId.Value == 0)
                                                       ? null
                                                       : dto.CategoriaPadreId.Value;
            }
            else if (dto.CategoriaPadreId == null && categoriaAActualizar.CategoriaPadreId.HasValue)
            {
                // Si se envía NULL explícitamente y antes tenía un padre, lo eliminamos
                categoriaAActualizar.CategoriaPadreId = null;
            }

            // Guardar cambios
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
