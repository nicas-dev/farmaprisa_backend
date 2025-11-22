//using FarmaPrisa.Models.Dtos;
//using FarmaPrisa.Models.Dtos.OpinionProducto;
//using FarmaPrisa.Models.Dtos.Producto;
//using FarmaPrisa.Models.Dtos.Proveedor;
//using FarmaPrisa.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace FarmaPrisa.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class ProductosController : ControllerBase
//    {
//        private readonly IProductosService _productosService;

//        // Inyectamos el servicio
//        public ProductosController(IProductosService productosService)
//        {
//            _productosService = productosService;
//        }

//        /// <summary>
//        /// Obtiene el catálogo de productos de FarmaPrisa con soporte para filtros opcionales.
//        /// </summary>
//        /// <remarks>
//        /// Este endpoint accede al catálogo completo de la tabla 'productos' en MariaDB.
//        /// Permite a los clientes (web y móvil) explorar artículos y aplicar filtros.
//        /// 
//        /// **Filtros disponibles (Opcionales):**
//        /// * **categoriaId:** Filtra productos por una categoría o subcategoría específica.
//        /// * **proveedorId:** Filtra productos por la marca o laboratorio.
//        /// * **sintomaId:** Filtra productos que tratan un síntoma específico (usa la tabla pivote producto_sintomas).
//        /// </remarks>
//        /// <param name="categoriaId">ID de la categoría para filtrar el listado.</param>
//        /// <param name="proveedorId">ID del proveedor o marca para filtrar el listado.</param>
//        /// <param name="sintomaId">ID del síntoma para filtrar los productos relacionados.</param>
//        /// <response code="200">Devuelve un listado de productos con la estructura de ProductoDto.</response>
//        /// <response code="500">Error interno del servidor al procesar la solicitud.</response>
//        [HttpGet]
//        [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<IActionResult> GetProductos(
//            [FromQuery] int? categoriaId,
//            [FromQuery] int? proveedorId,
//            [FromQuery] int? sintomaId)
//        {
//            try
//            {
//                var productos = await _productosService.GetProductosAsync(categoriaId, proveedorId, sintomaId);
//                return Ok(productos);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Error al obtener la lista de productos: " + ex.Message);
//            }
//        }

//        /// <summary>
//        /// Obtiene todos los detalles de un producto específico (ficha completa).
//        /// </summary>
//        /// <remarks>
//        /// Este endpoint carga datos pesados: detalles personalizados, especificaciones, y la lista de opiniones.
//        /// Es utilizado por la vista de detalle de la aplicación web y móvil.
//        /// </remarks>
//        /// <param name="id">El ID del producto a buscar.</param>
//        /// <param name="idioma">Idioma a obtener los detalles</param>
//        /// <response code="200">Devuelve un objeto ProductoDetalleDto con todos los datos.</response>
//        /// <response code="404">Si el producto con el ID especificado no se encuentra.</response>
//        /// <response code="500">Error interno del servidor.</response>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(ProductoDetalleDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<IActionResult> GetProductoDetalle(int id, [FromQuery] string idioma = "ES")
//        {
//            try
//            {
//                // 1. Estandarizar el valor del idioma a mayúsculas
//                string idiomaCode = idioma.ToUpper();

//                // 2. Llamar al servicio con el parámetro limpio
//                var producto = await _productosService.GetProductoDetalleAsync(id, idiomaCode);

//                if (producto == null)
//                {
//                    return NotFound($"Producto con ID {id} no encontrado.");
//                }

//                return Ok(producto);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Error al obtener los detalles del producto: " + ex.Message);
//            }
//        }

//        /// <summary>
//        /// Crea un nuevo producto en el catálogo. Solo accesible para Administradores.
//        /// </summary>
//        /// <remarks>
//        /// Este endpoint requiere el rol 'administrador' para su ejecución.
//        /// Permite ingresar el producto principal y su colección de detalles (especificaciones, ingredientes).
//        /// Los valores de tipoDetalleIdentificador son los identificadores para los detalles (DESCRIPTION, PRODUCT_SPECIFICATIONS, 
//        /// INGREDIENTS, NUTRITION_FACTS, WARNINGS, SHIPPING_SPECIFICATIONS, FROM_THE_BRAND, PRODUCT_BANNER).
//        /// </remarks>
//        /// <param name="dto">Datos del nuevo producto.</param>
//        /// <response code="201">Producto creado exitosamente. Devuelve el ID del nuevo producto.</response>
//        /// <response code="400">Datos de entrada inválidos (ej. SKu duplicado, falta el Nombre).</response>
//        /// <response code="401">No autorizado.</response>
//        [HttpPost]
//        [Authorize(Roles = "administrador")]
//        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        public async Task<IActionResult> CreateProducto([FromBody] ProductoCreateDto dto)
//        {
//            try
//            {
//                int nuevoId = await _productosService.CreateProductoAsync(dto);
//                return CreatedAtAction(nameof(GetProductoDetalle), new { id = nuevoId }, nuevoId);
//            }
//            catch (KeyNotFoundException)
//            {
//                // Se lanza si el TipoDetalleIdentificador no existe en la BD
//                return BadRequest("Uno o más identificadores de detalle no son válidos.");
//            }
//            catch (Exception ex)
//            {
//                // Manejo de otros errores, como violación de unicidad de SKU
//                return StatusCode(500, "Error al crear el producto: " + ex.Message);
//            }
//        }

//        // Controllers/ProductosController.cs (Añadir este método)
//        /// <summary>
//        /// Actualiza la información básica y sincroniza la colección de detalles de un producto existente. Solo Administradores.
//        /// </summary>
//        /// <remarks>
//        /// Este endpoint realiza una actualización "delta" para las propiedades básicas y una sincronización completa para la lista de detalles.
//        /// * **Detalles con ID:** Se actualizan.
//        /// * **Detalles sin ID:** Se añaden.
//        /// * **Detalles existentes no enviados:** Se eliminan.
//        /// </remarks>
//        /// <param name="id">ID del producto a actualizar.</param>
//        /// <param name="dto">Datos del producto a actualizar.</param>
//        /// <response code="200">Actualización exitosa.</response>
//        /// <response code="400">Datos de entrada inválidos (ej. SKU duplicado).</response>
//        /// <response code="401">No autorizado.</response>
//        /// <response code="404">Producto no encontrado.</response>
//        [HttpPut("{id}")]
//        [Authorize(Roles = "administrador")] // ¡PROTECCIÓN CRÍTICA!
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        public async Task<IActionResult> UpdateProducto(int id, [FromBody] ProductoUpdateDto dto)
//        {
//            try
//            {
//                bool exito = await _productosService.UpdateProductoAsync(id, dto);

//                if (!exito)
//                {
//                    return NotFound($"Producto con ID {id} no encontrado.");
//                }

//                return Ok(new { message = $"Producto con ID {id} actualizado exitosamente." });
//            }
//            catch (Exception ex)
//            {
//                // Esto captura errores como violación de unicidad (SKU) o FKs inválidas.
//                return StatusCode(500, "Error al actualizar el producto: " + ex.Message);
//            }
//        }

//        /// <summary>
//        /// Alterna (Activa/Desactiva) el estado de un producto para mostrarlo u ocultarlo del catálogo. Solo Administradores.
//        /// </summary>
//        /// <remarks>
//        /// Este endpoint invierte el valor actual de 'EstaActivo' (Soft Delete), manteniendo la integridad de la BD.
//        /// </remarks>
//        /// <param name="id">ID del producto a modificar.</param>
//        /// <response code="200">Alternancia de estado exitosa. Devuelve un mensaje de éxito y el nuevo estado.</response>
//        /// <response code="404">Producto no encontrado.</response>
//        [HttpPut("{id}/toggle-status")]
//        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [Authorize(Roles = "administrador")]
//        public async Task<IActionResult> ToggleProductoStatus(int id)
//        {
//            try
//            {
//                // 1. Ejecutar la alternancia de estado
//                bool exito = await _productosService.ToggleProductoStatusAsync(id);

//                if (!exito)
//                {
//                    return NotFound($"Producto con ID {id} no encontrado.");
//                }

//                // 2. Obtener el producto para el mensaje (se podría optimizar el servicio para devolver el estado)
//                var productoActualizado = await _productosService.GetProductoDetalleAsync(id, "ES"); // Asumimos 'ES' para obtener el detalle

//                string estado = productoActualizado?.EstaActivo == true ? "activado" : "desactivado";

//                // 3. Respuesta Consistente (200 OK)
//                var resultDto = new ApiResultDto
//                {
//                    Success = true,
//                    Message = $"Producto con ID {id} {estado} exitosamente."
//                };

//                return Ok(resultDto);
//            }
//            catch (Exception ex)
//            {
//                // Capturamos el error
//                return StatusCode(500, new ApiResultDto { Success = false, Message = $"Error inesperado al alternar el estado: {ex.Message}" });
//            }
//        }

//        /// <summary>
//        /// Recupera la lista completa de proveedores y laboratorios para usarlos en filtros.
//        /// </summary>
//        /// <remarks>
//        /// Este endpoint es de acceso público y es usado por el frontend para poblar el menú de filtros de marca.
//        /// </remarks>
//        /// <response code="200">Devuelve un listado de ProveedorDto (Id y Nombre).</response>
//        [HttpGet("proveedores")]
//        [ProducesResponseType(typeof(IEnumerable<ProveedorDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetProveedores()
//        {
//            var proveedores = await _productosService.GetProveedoresAsync();
//            return Ok(proveedores);
//        }

//        /// <summary>
//        /// Elimina permanentemente (físicamente) una imagen de la galería de un producto. Solo Administradores.
//        /// </summary>
//        /// <remarks>
//        /// Elimina la imagen del disco del servidor (VPS) y su registro de la base de datos para liberar espacio.
//        /// Requiere el rol 'administrador'.
//        /// </remarks>
//        /// <param name="imagenId">ID del registro de la imagen en la tabla producto_imagenes.</param>
//        /// <response code="204">Imagen eliminada exitosamente (No Content).</response>
//        /// <response code="404">Registro de imagen no encontrado.</response>
//        [HttpDelete("galeria/{imagenId}")]
//        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<IActionResult> DeleteImagenGaleria(int imagenId)
//        {
//            try
//            {
//                bool exito = await _productosService.DeleteImagenGaleriaAsync(imagenId);

//                if (!exito)
//                {
//                    return NotFound($"Registro de imagen con ID {imagenId} no encontrado.");
//                }

//                // Respuesta de Éxito (200 OK)
//                var resultDto = new ApiResultDto
//                {
//                    Success = true,
//                    Message = $"Imagen con ID {imagenId} eliminada permanentemente."
//                };

//                // Devolvemos 200 OK con el objeto de éxito
//                return Ok(resultDto);
//            }
//            catch (Exception)
//            {
//                // Captura errores genéricos o fallos de I/O al eliminar del disco
//                var resultDto = new ApiResultDto
//                {
//                    Success = false,
//                    Message = "Error al eliminar la imagen del servidor. Verifique logs."
//                };
//                return StatusCode(StatusCodes.Status500InternalServerError, resultDto);
//            }
//        }

//        /// <summary>
//        /// Establece una imagen específica de la galería como la foto principal del producto, y desactiva las demás. Solo Administradores.
//        /// </summary>
//        /// <remarks>
//        /// Requiere el rol 'administrador'. Este endpoint asegura que solo una imagen tenga 'EsPrincipal = true' por producto mediante una transacción.
//        /// </remarks>
//        /// <param name="id">ID del producto padre.</param>
//        /// <param name="imagenId">ID del registro de la imagen en la tabla producto_imagenes que será marcada como principal.</param>
//        /// <response code="200">Imagen principal asignada exitosamente.</response>
//        /// <response code="404">Producto o Imagen no encontrada.</response>
//        [HttpPut("{id}/galeria/{imagenId}/principal")]
//        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [Authorize(Roles = "administrador")]
//        public async Task<IActionResult> SetImagenPrincipal(int id, int imagenId)
//        {
//            try
//            {
//                // 1. Llamar al servicio que implementa la lógica transaccional
//                bool exito = await _productosService.SetImagenPrincipalAsync(id, imagenId);

//                if (!exito)
//                {
//                    // Podría ser que la imagenId no exista o no pertenezca al producto ID.
//                    return NotFound($"Imagen o Producto con ID {id} no encontrado.");
//                }

//                // 2. Respuesta Consistente (200 OK)
//                var resultDto = new ApiResultDto
//                {
//                    Success = true,
//                    Message = $"Imagen {imagenId} asignada como principal del producto {id}."
//                };

//                return Ok(resultDto);
//            }
//            catch (Exception)
//            {
//                // Esto captura errores si la transacción falla
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    new ApiResultDto { Success = false, Message = "Error inesperado al establecer la imagen principal. La transacción fue revertida." });
//            }
//        }

//        /// <summary>
//        /// Permite a un cliente autenticado agregar una calificación y comentario (reseña) a un producto.
//        /// </summary>
//        /// <remarks>
//        /// Requiere autenticación. La opinión se vincula al ProductoId de la URL y al UsuarioId del token JWT.
//        /// </remarks>
//        /// <param name="id">ID del producto al cual se añade la opinión.</param>
//        /// <param name="dto">Objeto con la calificación (1-5) y el comentario.</param>
//        /// <response code="201">Opinión creada exitosamente.</response>
//        /// <response code="401">No autorizado (falta token).</response>
//        /// <response code="404">Producto no encontrado.</response>
//        [HttpPost("{id}/opiniones")]
//        [Authorize] // Solo usuarios autenticados
//        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> CreateOpinion(int id, [FromBody] OpinionCreateDto dto)
//        {
//            try
//            {
//                // 1. Obtener el ID del usuario autenticado del token JWT (ClaimTypes.NameIdentifier)
//                // CRÍTICO: Debemos asegurar que el claim exista y se pueda convertir a entero.
//                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
//                if (!int.TryParse(userIdString, out int userId))
//                {
//                    return Unauthorized("Token inválido o ID de usuario faltante.");
//                }

//                // 2. Llamar al servicio
//                bool exito = await _productosService.CreateOpinionAsync(id, userId, dto);

//                if (exito)
//                {
//                    var resultDto = new ApiResultDto { Success = true, Message = "Opinión registrada exitosamente." };
//                    // Devolvemos 201 Created ya que estamos creando un nuevo recurso
//                    return StatusCode(StatusCodes.Status201Created, resultDto);
//                }

//                // Si el servicio devuelve false (aunque no debería con la lógica actual)
//                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResultDto { Success = false, Message = "Error al registrar la opinión." });
//            }
//            catch (KeyNotFoundException ex)
//            {
//                // Producto no encontrado
//                return NotFound(new ApiResultDto { Success = false, Message = ex.Message });
//            }
//            catch (InvalidOperationException ex)
//            {
//                // El usuario ya opinó, o regla de negocio violada
//                return BadRequest(new ApiResultDto { Success = false, Message = ex.Message });
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResultDto { Success = false, Message = "Ocurrió un error inesperado al procesar la opinión." });
//            }
//        }

//        /// <summary>
//        /// Devuelve el listado de opiniones (reseñas) para un producto específico. Acceso Público.
//        /// </summary>
//        /// <remarks>
//        /// Este endpoint es usado por la vista de detalle para cargar las reseñas de forma separada.
//        /// </remarks>
//        /// <param name="id">ID del producto.</param>
//        /// <response code="200">Listado de opiniones ordenadas por fecha reciente.</response>
//        /// <response code="404">Producto no encontrado (si el listado está vacío).</response>
//        [HttpGet("{id}/opiniones")]
//        [ProducesResponseType(typeof(IEnumerable<OpinionDto>), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetOpinionesByProductoId(int id)
//        {
//            var opiniones = await _productosService.GetOpinionesByProductoIdAsync(id);

//            if (opiniones == null || !opiniones.Any())
//            {
//                // Se devuelve 200 OK con lista vacía, o 404 si prefieres ser estricto.
//                // Optamos por 200 OK y lista vacía, ya que el producto podría existir sin reseñas.
//                return Ok(opiniones);
//            }

//            return Ok(opiniones);
//        }
//    }
//}
