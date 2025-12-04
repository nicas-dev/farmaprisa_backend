using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Products;
using FarmaPrisa.Models.Entities;
using FarmaPrisa.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly FarmaPrisaContext _context;

        public ProductsController(IProductRepository productRepository, FarmaPrisaContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        /// <summary>
        /// Devuelve Listado De Todos Los Productos.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Devuelve Listado De Productos Por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound($"No se encontró el producto con ID {id}");

            return Ok(product);
        }


        /// <summary>
        /// Crear Nuevo Producto. Solo Administradores.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = "administrador")]
        public async Task<ActionResult> Create(ProductDto productDto)
        {
            if (productDto == null)
                return BadRequest("Los datos del producto no pueden ser nulos.");

            await _productRepository.AddAsync(productDto);
            return CreatedAtAction(nameof(GetById), new { id = productDto.IdProduct }, productDto);
        }

        /// <summary>
        /// Actualizar Productos. Solo Administradores.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<ActionResult> Update(int id, ProductDto productDto)
        {
            if (id != productDto.IdProduct)
                return BadRequest("El ID del producto no coincide con la solicitud.");

            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el producto con ID {id}");

            await _productRepository.UpdateAsync(productDto);
            return NoContent();
        }


        /// <summary>
        /// Desactivar Productos. Solo Administradores.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el producto con ID {id}");

            await _productRepository.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Listar todos los productos con sus imagenes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-images")]
        public async Task<IActionResult> GetAllImages()
        {
            var imagenes = await _context.ProductoImagenes
                .OrderBy(img => img.ProductoId)
                .ThenBy(img => img.Orden)
                .Select(img => new
                {
                    img.Id,
                    img.ProductoId,
                    img.UrlImagen,
                    img.EsPrincipal,
                    img.Orden
                })
                .ToListAsync();

            if (!imagenes.Any())
                return NotFound(new { message = "No hay imágenes registradas." });

            return Ok(imagenes);
        }

        /// <summary>
        /// Listar Imagenes de productos, orden, esprincipal, url
        /// se debe de especificar el id del producto.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{productId}/images")]
        public async Task<IActionResult> GetProductImages(int productId)
        {
            var imagenes = await _context.ProductoImagenes
                .Where(img => img.ProductoId == productId)
                .OrderBy(img => img.Orden)
                .Select(img => new
                {
                    img.Id,
                    img.UrlImagen,
                    img.EsPrincipal,
                    img.Orden
                })
                .ToListAsync();

            if (!imagenes.Any())
                return NotFound($"No hay imágenes para el producto con ID {productId}");

            return Ok(new
            {
                success = true,
                total = imagenes.Count,
                data = imagenes
            });
        }

        /// <summary>
        /// Subir múltiples imágenes para un producto. Solo Administradores.
        /// Se guardara en wwwroot/images del proyecto de manera publica, asignar si es principal e igual opcional establecer si es principal.
        /// </summary>
        /// 
        [HttpPost("upload-images/{productId}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> UploadImages(int productId,
                                             List<IFormFile> files,
                                             int? principalIndex = 0)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No se enviaron imágenes.");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var imagenesGuardadas = new List<ProductoImagen>();
            int index = 0;

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var publicUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";

                var imagen = new ProductoImagen
                {
                    ProductoId = productId,
                    UrlImagen = publicUrl,
                    Orden = index + 1,
                    EsPrincipal = index == principalIndex //definimos la imagen principal
                };

                _context.ProductoImagenes.Add(imagen);
                imagenesGuardadas.Add(imagen);
                index++;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = $"{imagenesGuardadas.Count} imágenes guardadas con éxito",
                data = imagenesGuardadas
            });
        }


        //Eliminar imagenes
        [HttpDelete("delete-image/{productId}/{imageId}")]
        //[Authorize(Roles = "administrador")]
        public async Task<IActionResult> DeleteImage(int productId, int imageId)
        {
            // Buscar la imagen
            var imagen = await _context.ProductoImagenes
                                       .FirstOrDefaultAsync(x => x.ProductoId == productId && x.Id == imageId);

            if (imagen == null)
                return NotFound("La imagen no existe o no pertenece a este producto.");

            // Obtener ruta física del archivo
            var imageName = Path.GetFileName(imagen.UrlImagen);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageName);

            // Eliminar archivo físico si existe
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Eliminar registro de BD
            _context.ProductoImagenes.Remove(imagen);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Imagen eliminada.",
                deletedImage = imageId
            });
        }

        //Cambiar imagen principal
        [HttpPut("set-principal/{productId}/{imageId}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> SetPrincipalImage(int productId, int imageId)
        {
            // Obtener todas las imágenes del producto
            var imagenes = await _context.ProductoImagenes
                                        .Where(x => x.ProductoId == productId)
                                        .ToListAsync();

            if (!imagenes.Any())
                return NotFound("El producto no tiene imágenes registradas.");

            // Buscar la imagen que queremos marcar como principal
            var imagenPrincipal = imagenes.FirstOrDefault(x => x.Id == imageId);

            if (imagenPrincipal == null)
                return NotFound("La imagen seleccionada no existe para este producto.");

            // Marcar todas como no principales
            foreach (var img in imagenes)
                img.EsPrincipal = false;

            // Marcar la seleccionada como principal
            imagenPrincipal.EsPrincipal = true;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Imagen principal actualizada.",
                newPrincipal = imageId
            });
        }



    }

}

