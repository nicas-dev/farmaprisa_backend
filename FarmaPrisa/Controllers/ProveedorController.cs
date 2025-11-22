namespace FarmaPrisa.Controllers
{
    using FarmaPrisa.Models.Dtos;
    using FarmaPrisa.Models.Dtos.Proveedor;
    using FarmaPrisa.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("admin/[controller]")] // Ruta administrativa: /admin/proveedor
    [Authorize(Roles = "administrador")]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        /// <summary>
        /// Registra un nuevo proveedor/laboratorio en el sistema. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este endpoint es necesario para crear nuevos productos.
        /// </remarks>
        /// <param name="dto">Datos del nuevo proveedor.</param>
        /// <response code="201">Proveedor creado exitosamente. Devuelve el ID generado.</response>
        /// <response code="400">Error en la solicitud (ej. Nombre ya existe).</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProveedor([FromBody] ProveedorCreateUpdateDto dto)
        {
            try
            {
                int nuevoId = await _proveedorService.CreateProveedorAsync(dto);

                // Respuesta de Éxito (201 Created)
                var resultDto = new ApiResultDto
                {
                    Success = true,
                    Message = $"Proveedor {dto.Nombre} creado exitosamente.",
                    Data = new { id = nuevoId } // Devolvemos el ID generado
                };

                // Utilizamos CreatedAtAction para devolver el código 201 y el objeto ApiResultDto
                return CreatedAtAction(nameof(CreateProveedor), new { id = nuevoId }, resultDto);
            }
            catch (InvalidOperationException ex)
            {
                // Nombre ya existe (400 Bad Request)
                var resultDto = new ApiResultDto { Success = false, Message = ex.Message };
                return BadRequest(resultDto);
            }
            catch (Exception)
            {
                // Error interno del servidor (500 Internal Server Error)
                var resultDto = new ApiResultDto { Success = false, Message = "Ocurrió un error inesperado al crear el proveedor." };
                return StatusCode(StatusCodes.Status500InternalServerError, resultDto);
            }
        }

        /// <summary>
        /// Devuelve el listado completo de proveedores (marcas/laboratorios) para el Panel de Administración.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este endpoint se usa para poblar la tabla de gestión de proveedores.
        /// </remarks>
        /// <response code="200">Listado de proveedores (Id, Nombre, LogoUrl).</response>
        /// <response code="403">Acceso denegado.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProveedorDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProveedores()
        {
            var proveedores = await _proveedorService.GetProveedoresAdminAsync();

            // Devolvemos 200 OK con el listado completo
            return Ok(proveedores);
        }

        /// <summary>
        /// Actualiza la información de un proveedor/laboratorio existente. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Se aplica validación de unicidad en Nombre, Email y Identificación Fiscal.
        /// </remarks>
        /// <param name="id">ID del proveedor a actualizar.</param>
        /// <param name="dto">Datos del proveedor a actualizar.</param>
        /// <response code="200">Actualización exitosa. Devuelve un mensaje de éxito.</response>
        /// <response code="400">Datos de entrada inválidos (ej. Nombre, Email o RUC ya existe en otro registro).</response>
        /// <response code="404">Proveedor no encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProveedor(int id, [FromBody] ProveedorCreateUpdateDto dto)
        {
            try
            {
                bool exito = await _proveedorService.UpdateProveedorAsync(id, dto);

                if (!exito)
                {
                    return NotFound($"Proveedor con ID {id} no encontrado.");
                }

                // Respuesta de Éxito (200 OK)
                var resultDto = new ApiResultDto { Success = true, Message = $"Proveedor con ID {id} actualizado exitosamente." };
                return Ok(resultDto);
            }
            catch (InvalidOperationException ex)
            {
                // Violación de unicidad
                var resultDto = new ApiResultDto { Success = false, Message = ex.Message };
                return BadRequest(resultDto);
            }
            catch (Exception)
            {
                // Error genérico o de BD
                var resultDto = new ApiResultDto { Success = false, Message = "Ocurrió un error inesperado al actualizar el proveedor." };
                return StatusCode(StatusCodes.Status500InternalServerError, resultDto);
            }
        }

        /// <summary>
        /// Alterna (Activa/Desactiva) el estado de un proveedor para mostrarlo u ocultarlo del catálogo. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este endpoint invierte el valor actual de 'EstaActivo' (Soft Delete).
        /// </remarks>
        /// <param name="id">ID del proveedor a modificar.</param>
        /// <response code="200">Alternancia de estado exitosa. Devuelve un ApiResultDto con el nuevo estado.</response>
        /// <response code="404">Proveedor no encontrado.</response>
        [HttpPut("{id}/toggle-status")] // <--- ¡NUEVO ENDPOINT Y MÉTODO!
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleProveedorStatus(int id)
        {
            try
            {
                // 1. Ejecutar la alternancia de estado
                bool exito = await _proveedorService.ToggleProveedorStatusAsync(id);

                if (!exito)
                {
                    return NotFound($"Proveedor con ID {id} no encontrado.");
                }

                // 2. Obtener el proveedor actualizado para el mensaje
                var proveedorActualizado = await _proveedorService.GetProveedorDetalleAsync(id);

                string estado = proveedorActualizado?.EstaActivo == true ? "activado" : "inactivado";

                // 3. Respuesta Consistente (200 OK)
                var resultDto = new ApiResultDto
                {
                    Success = true,
                    Message = $"Proveedor con ID {id} {estado} exitosamente."
                };

                return Ok(resultDto);
            }
            catch (Exception)
            {
                // Capturamos el error genérico
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResultDto { Success = false, Message = "Error inesperado al alternar el estado del proveedor." });
            }
        }

        /// <summary>
        /// Obtiene los detalles completos de un proveedor específico por su ID. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este endpoint se usa para cargar los datos en el formulario de edición (PUT).
        /// </remarks>
        /// <param name="id">ID del proveedor a consultar.</param>
        /// <response code="200">Devuelve el objeto ProveedorAdminDto completo.</response>
        /// <response code="404">Proveedor no encontrado.</response>
        /// <response code="403">Acceso denegado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProveedorAdminDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetProveedorDetalle(int id)
        {
            var proveedorDetalle = await _proveedorService.GetProveedorDetalleAsync(id);

            if (proveedorDetalle == null)
            {
                return NotFound($"Proveedor con ID {id} no encontrado.");
            }

            return Ok(proveedorDetalle);
        }
    }
}
