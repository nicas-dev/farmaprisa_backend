namespace FarmaPrisa.Controllers
{
    using FarmaPrisa.Models.Dtos;
    using FarmaPrisa.Models.Dtos.OpinionProducto;
    using FarmaPrisa.Models.Dtos.Producto;
    using FarmaPrisa.Models.Dtos.Usuario;
    using FarmaPrisa.Services;
    using Microsoft.AspNetCore.Authorization; // Necesario para [Authorize]
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("admin")] // Ruta base para todos los endpoints administrativos
    [Authorize(Roles = "administrador")] // Protege TODO el controlador para administradores
    public class AdminController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPedidoService _pedidoService; // Servicio para gestionar pedidos
        //private readonly IProductosService _productosService; // Servicio para gestionar productos
        // Puedes inyectar otros servicios de administración aquí

        public AdminController(IUsuarioService usuarioService, IPedidoService pedidoService) /*IProductosService productosService*/
        {
            _usuarioService = usuarioService;
            _pedidoService = pedidoService;
            //_productosService = productosService;
        }

        /// <summary>
        /// Devuelve el listado completo de todos los usuarios registrados (activos e inactivos). Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Este endpoint ignora cualquier filtro global de seguridad para permitir la gestión completa.
        /// Devuelve los roles de cada usuario.
        /// </remarks>
        /// <response code="200">Listado de todos los usuarios (incluye estado e información de roles).</response>
        /// <response code="401">No autorizado (falta token).</response>
        /// <response code="403">Acceso denegado (el usuario no tiene el rol 'administrador').</response>
        [HttpGet("usuarios")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioAdminDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetTodosLosUsuariosAsync();
            return Ok(usuarios);
        }

        /// <summary>
        /// Alterna (Bloquea/Desbloquea) el acceso de un usuario al sistema. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este endpoint invierte el estado actual de 'EstaActivo'.
        /// </remarks>
        /// <param name="id">ID del usuario a modificar.</param>
        /// <response code="200">Alternancia de estado exitosa. Devuelve un ApiResultDto.</response>
        /// <response code="404">Usuario no encontrado.</response>
        [HttpPut("usuarios/{id}/toggle-status")]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status200OK)] // Asegurar el tipo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            try
            {
                // 1. Llamar al nuevo método de alternancia
                bool exito = await _usuarioService.ToggleUserStatusAsync(id);

                if (!exito)
                {
                    return NotFound($"Usuario con ID {id} no encontrado.");
                }

                // 2. Obtener el nuevo estado para el mensaje
                var usuarioActualizado = await _usuarioService.GetUsuarioDetalleAsync(id);
                string estado = usuarioActualizado?.EstaActivo == true ? "desbloqueada" : "bloqueada";

                // 3. Respuesta de Éxito usando ApiResultDto (CRÍTICO)
                var resultDto = new ApiResultDto
                {
                    Success = true,
                    Message = $"Cuenta de usuario con ID {id} {estado} exitosamente."
                };

                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                // 4. Respuesta de Error (500 Internal Server Error) usando ApiResultDto
                var errorDto = new ApiResultDto
                {
                    Success = false,
                    Message = "Error al alternar el estado del usuario: " + ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorDto);
            }
        }

        /// <summary>
        /// Muestra la lista de todos los pedidos registrados en el sistema con un resumen de su estado. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Este endpoint es utilizado por el dashboard para la gestión de órdenes.
        /// Incluye el nombre del cliente, el estado, el tipo de entrega y el total.
        /// </remarks>
        /// <response code="200">Listado de Pedidos con información resumida.</response>
        /// <response code="403">Acceso denegado (el usuario no tiene el rol 'administrador').</response>
        [HttpGet("pedidos")]
        [ProducesResponseType(typeof(IEnumerable<PedidoAdminDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPedidosAdmin()
        {
            var pedidos = await _pedidoService.GetPedidosAdminAsync();
            return Ok(pedidos);
        }

        /// <summary>
        /// Muestra el detalle completo de un pedido específico, incluyendo artículos, costos, cliente y estado de pago. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este es el endpoint utilizado por la vista de detalle de la orden.
        /// Carga datos complejos como detalles_pedido, cliente, dirección y transacciones.
        /// </remarks>
        /// <param name="id">ID del pedido a consultar.</param>
        /// <response code="200">Devuelve el objeto PedidoDetalleDto completo.</response>
        /// <response code="404">Pedido no encontrado.</response>
        /// <response code="403">Acceso denegado.</response>
        [HttpGet("pedidos/{id}")]
        [ProducesResponseType(typeof(PedidoDetalleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPedidoDetalle(int id)
        {
            var detalle = await _pedidoService.GetPedidoDetalleAdminAsync(id);

            if (detalle == null)
            {
                return NotFound($"Pedido con ID {id} no encontrado.");
            }

            return Ok(detalle);
        }

        /// <summary>
        /// Carga masiva de productos desde un archivo CSV o Excel. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Este endpoint lee el archivo, valida los datos, resuelve los identificadores de Categoría/Proveedor y realiza la inserción transaccional.
        /// Se utiliza un archivo de plantilla estricta.
        /// </remarks>
        /// <param name="file">El archivo CSV o Excel a importar.</param>
        /// <response code="200">Operación completada. Devuelve un resumen de filas exitosas y fallidas.</response>
        /// <response code="400">Archivos no válidos o errores de formato.</response>
        //[HttpPost("productos/importar")]
        //[ProducesResponseType(typeof(ImportResultDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> ImportarProductos(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("Se requiere un archivo para la importación.");
        //    }

        //    // Solo permitir archivos de texto/CSV
        //    if (!file.ContentType.Contains("xlsx") && !file.FileName.EndsWith(".xlsx"))
        //    {
        //        return BadRequest("Formato de archivo inválido. Por favor, suba un archivo en formato CSV (delimitado por punto y coma).");
        //    }

        //    try
        //    {
        //        using (var stream = file.OpenReadStream())
        //        {
        //            // Asumimos que el tipo de archivo es CSV o Excel para la lógica de servicio
        //            //var resultado = await _productosService.ImportarProductosAsync(stream, file.ContentType);
        //            return Ok(resultado);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Error durante la importación: " + ex.Message);
        //    }
        //}

        /// <summary>
        /// Sube múltiples imágenes para la galería de un producto existente. Solo Administradores.
        /// </summary>
        /// <param name="id">ID del producto al que se añadirán las imágenes.</param>
        /// <param name="archivos">Colección de archivos (IFormFile) a subir.</param>
        [HttpPost("productos/{id}/galeria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> AddImagenesGaleria(int id, [FromForm] ICollection<IFormFile> archivos)
        //{
        //    if (archivos == null || !archivos.Any())
        //    {
        //        return BadRequest("Debe seleccionar al menos un archivo para la galería.");
        //    }

        //    // Llamar al servicio
        //    bool exito = await _productosService.AddImagenesGaleriaAsync(id, archivos);

        //    if (!exito)
        //    {
        //        return NotFound($"Producto con ID {id} no encontrado.");
        //    }

        //    return Ok(new { message = $"Se subieron {archivos.Count} imágenes al producto {id}." });
        //}

        /// <summary>
        /// Devuelve el catálogo completo de roles disponibles en el sistema (ej: administrador, entregador, cliente). Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Este endpoint se usa para poblar listas de selección en el panel de administración al asignar roles a un usuario.
        /// Requiere el rol 'administrador'.
        /// </remarks>
        /// <response code="200">Listado de roles (Id y Nombre).</response>
        /// <response code="403">Acceso denegado.</response>
        [HttpGet("roles")]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _usuarioService.GetRolesAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Sincroniza la lista completa de roles asignados a un usuario. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Este endpoint requiere la lista *completa* de IDs de roles que el usuario debe tener. 
        /// Los roles existentes que no estén en la lista enviada serán eliminados.
        /// Requiere el rol 'administrador'.
        /// </remarks>
        /// <param name="id">ID del usuario cuyos roles serán actualizados.</param>
        /// <param name="dto">Objeto con la lista de IDs de roles que el usuario debe tener.</param>
        /// <response code="200">Sincronización de roles exitosa.</response>
        /// <response code="400">Datos de entrada inválidos (ej: RolId inexistente).</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="404">Usuario no encontrado.</response>
        [HttpPut("usuarios/{id}/roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUserRoles(int id, [FromBody] UserRolesUpdateDto dto)
        {
            try
            {
                // Llamar al servicio que implementa la sincronización
                bool exito = await _usuarioService.UpdateUserRolesAsync(id, dto.RolIds);

                if (!exito)
                {
                    return NotFound($"Usuario con ID {id} no encontrado.");
                }

                return Ok(new { message = $"Roles del usuario {id} sincronizados exitosamente." });
            }
            catch (DbUpdateException dbEx)
            {
                // Captura errores de FK (si un RolId enviado no existe en la tabla Roles)
                return StatusCode(500, "Error de Base de Datos: Verifique que todos los IDs de roles sean válidos.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inesperado al sincronizar los roles: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo usuario y le asigna roles específicos (ej: Entregador o Cajero). Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este endpoint permite registrar empleados o administradores directamente.
        /// </remarks>
        /// <param name="dto">Datos del nuevo usuario y lista de Roles a asignar (por nombre).</param>
        /// <response code="201">Usuario creado y roles asignados exitosamente. Devuelve el ID del nuevo usuario.</response>
        /// <response code="400">Datos de entrada inválidos (ej. Correo ya existe).</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="500">Error interno (ej. Rol no encontrado o fallo en la BD).</response>
        [HttpPost("usuarios")]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAdminUser([FromBody] UsuarioAdminCreateDto dto)
        {
            try
            {
                int nuevoId = await _usuarioService.CreateUsuarioAdminAsync(dto);

                // Respuesta de Éxito (201 Created)
                var result = new ApiResultDto
                {
                    Success = true,
                    Message = "Usuario creado y roles asignados exitosamente.",
                    Data = new { id = nuevoId }
                };
                return CreatedAtAction(nameof(GetUsuarios), new { id = nuevoId }, result);
            }
            catch (InvalidOperationException ex)
            {
                // Correo ya existe (400 Bad Request)
                var result = new ApiResultDto { Success = false, Message = ex.Message };
                return BadRequest(result);
            }
            catch (KeyNotFoundException ex)
            {
                // Rol no existe (400 Bad Request)
                var result = new ApiResultDto { Success = false, Message = ex.Message };
                return BadRequest(result);
            }
            catch (Exception)
            {
                // Error genérico o fallo de la BD (500 Internal Server Error)
                var result = new ApiResultDto { Success = false, Message = "Ocurrió un error inesperado durante la creación del usuario." };
                return StatusCode(500, result);
            }
        }

        /// <summary>
        /// Sube un archivo de imagen al servidor (VPS) y devuelve su URL pública. Uso general del Panel Admin.
        /// </summary>
        /// <remarks>
        /// Este endpoint se usa para subir logos, banners de proveedores, imágenes de perfil, etc., antes de crear o actualizar entidades.
        /// Requiere el rol 'administrador'.
        /// </remarks>
        /// <param name="file">El archivo de imagen a subir (IFormFile).</param>
        /// <response code="200">Archivo subido con éxito. Devuelve la URL pública.</response>
        /// <response code="400">Archivo no válido o faltante.</response>
        //[HttpPost("archivos/upload")]
        //[ProducesResponseType(typeof(FileUploadResponseDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> UploadFile(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("Se requiere un archivo.");
        //    }

        //    // Aquí podrías añadir validación de tipo de archivo (solo jpg, png, etc.)
        //    string url = await _productosService.UploadFileAsync(file);

        //    return Ok(new FileUploadResponseDto { UrlPublica = url });
        //}

        /// <summary>
        /// Devuelve todos los datos estadísticos y de gestión necesarios para el Dashboard del Administrador.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Consolida KPIs, gráficos de ventas y alertas de inventario.
        /// </remarks>
        /// <response code="200">Objeto DashboardDataDto con todos los indicadores.</response>
        /// <response code="403">Acceso denegado.</response>
        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(DashboardDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDashboardData()
        {
            var data = await _pedidoService.GetDashboardDataAsync();
            return Ok(data);
        }

        /// <summary>
        /// Devuelve el listado completo del inventario con paginación para la gestión de stock. Solo Administradores.
        /// </summary>
        /// <param name="page">Número de página a recuperar (por defecto 1).</param>
        /// <param name="pageSize">Número de elementos por página (por defecto 25).</param>
        /// <param name="searchTerm"></param>
        /// <response code="200">Datos paginados del inventario.</response>
        //[HttpGet("inventario")]
        //[ProducesResponseType(typeof(PagedResultDto<ProductoVencimientoDto>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetInventarioPaginado([FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string? searchTerm = null)
        //{
        //    if (page < 1 || pageSize < 1)
        //    {
        //        return BadRequest("La página y el tamaño de la página deben ser mayores que cero.");
        //    }

        //    // Pasar el nuevo parámetro al servicio
        //    var resultado = await _productosService.GetInventarioPaginadoAsync(page, pageSize, searchTerm);
        //    return Ok(resultado);
        //}

        /// <summary>
        /// Devuelve el listado completo de todas las opiniones de productos para moderación y gestión. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este endpoint se usa para el panel de moderación.
        /// </remarks>
        /// <response code="200">Listado de todas las opiniones.</response>
        /// <response code="403">Acceso denegado.</response>
        //[HttpGet("opiniones")]
        //[ProducesResponseType(typeof(IEnumerable<OpinionDto>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //public async Task<IActionResult> GetOpiniones()
        //{
        //    var opiniones = await _productosService.GetOpinionesAdminAsync();
        //    return Ok(opiniones);
        //}
    }
}
