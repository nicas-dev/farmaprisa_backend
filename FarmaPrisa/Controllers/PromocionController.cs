using FarmaPrisa.Models.Dtos.Promocion;
using FarmaPrisa.Models.Dtos;
using FarmaPrisa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmaPrisa.Controllers
{
    [ApiController]
    [Route("admin/[controller]")] // Ruta administrativa: /admin/promociones
    [Authorize(Roles = "administrador")]
    public class PromocionController : ControllerBase
    {
        private readonly IPromocionService _promocionService;

        public PromocionController(IPromocionService promocionService)
        {
            _promocionService = promocionService;
        }

        /// <summary>
        /// Crea una nueva promoción y asigna productos o categorías asociadas. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. La operación es transaccional.
        /// El 'CodigoCupon' debe ser único.
        /// </remarks>
        /// <param name="dto">Datos de la promoción (descuento, fechas) y colecciones de IDs (ProductoIds, CategoriaIds).</param>
        /// <response code="201">Promoción creada exitosamente.</response>
        /// <response code="400">Datos de entrada inválidos (ej. Código de Cupón duplicado o falta de campos obligatorios).</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePromocion([FromBody] PromocionCreateDto dto)
        {
            try
            {
                int nuevoId = await _promocionService.CreatePromocionAsync(dto);

                var resultDto = new ApiResultDto
                {
                    Success = true,
                    Message = $"Promoción '{dto.Nombre}' creada con éxito.",
                    Data = new { id = nuevoId }
                };

                return CreatedAtAction(nameof(CreatePromocion), new { id = nuevoId }, resultDto);
            }
            catch (InvalidOperationException ex)
            {
                // Código de cupón duplicado
                return BadRequest(new ApiResultDto { Success = false, Message = ex.Message });
            }
            catch (Exception)
            {
                // Errores de BD, FK, o transacción fallida
                return StatusCode(500, new ApiResultDto { Success = false, Message = "Ocurrió un error inesperado al crear la promoción. Verifique los IDs de Producto/Categoría." });
            }
        }

        /// <summary>
        /// Devuelve el listado completo de todas las promociones activas, inactivas y futuras. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. El listado incluye el conteo de productos y categorías asociadas.
        /// </remarks>
        /// <response code="200">Listado de PromocionesAdminDto ordenadas por FechaFin.</response>
        /// <response code="403">Acceso denegado.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PromocionAdminDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPromociones()
        {
            var promociones = await _promocionService.GetPromocionesAdminAsync();
            return Ok(promociones);
        }

        /// <summary>
        /// Devuelve el detalle completo de una promoción, incluyendo los IDs de todos los productos y categorías asociados. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Este endpoint se usa para precargar el formulario de edición de la promoción.
        /// </remarks>
        /// <param name="id">ID de la promoción a consultar.</param>
        /// <response code="200">Devuelve el objeto PromocionDetalleDto completo.</response>
        /// <response code="404">Promoción no encontrada.</response>
        /// <response code="403">Acceso denegado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PromocionDetalleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPromocionDetalle(int id)
        {
            var detalle = await _promocionService.GetPromocionDetalleAsync(id);

            if (detalle == null)
            {
                return NotFound($"Promoción con ID {id} no encontrada.");
            }

            return Ok(detalle);
        }

        /// <summary>
        /// Actualiza los datos principales de una promoción y sincroniza las listas de productos/categorías asociados. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Requiere el rol 'administrador'. Se utiliza una transacción para asegurar que la actualización de la promoción y las tablas pivote sean atómicas.
        /// </remarks>
        /// <param name="id">ID de la promoción a actualizar.</param>
        /// <param name="dto">Datos actualizados de la promoción (sincroniza ProductoIds y CategoriaIds).</param>
        /// <response code="200">Actualización exitosa.</response>
        /// <response code="400">Error en la solicitud (ej. Código de Cupón duplicado).</response>
        /// <response code="404">Promoción no encontrada.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePromocion(int id, [FromBody] PromocionCreateDto dto)
        {
            try
            {
                bool exito = await _promocionService.UpdatePromocionAsync(id, dto);

                if (!exito)
                {
                    return NotFound($"Promoción con ID {id} no encontrada.");
                }

                // Respuesta de Éxito (200 OK)
                var resultDto = new ApiResultDto { Success = true, Message = $"Promoción con ID {id} actualizada exitosamente." };
                return Ok(resultDto);
            }
            catch (InvalidOperationException ex)
            {
                // Violación de unicidad de Código de Cupón
                return BadRequest(new ApiResultDto { Success = false, Message = ex.Message });
            }
            catch (Exception)
            {
                // Error genérico o de BD (la transacción se revirtió)
                return StatusCode(500, new ApiResultDto { Success = false, Message = "Ocurrió un error inesperado al actualizar la promoción." });
            }
        }

        /// <summary>
        /// Alterna el estado de activación de una promoción (de activo a inactivo o viceversa). Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Este endpoint no requiere cuerpo de solicitud. Simplemente invierte el valor actual de 'EstaActiva'.
        /// </remarks>
        /// <param name="id">ID de la promoción a modificar.</param>
        /// <response code="200">Cambio de estado exitoso.</response>
        /// <response code="404">Promoción no encontrada.</response>
        [HttpPut("{id}/toggle-status")] // <--- ¡NUEVA RUTA!
        [ProducesResponseType(typeof(ApiResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TogglePromocionStatus(int id)
        {
            try
            {
                // 1. Buscamos el estado actual y lo invertimos en el servicio
                bool exito = await _promocionService.TogglePromocionStatusAsync(id);

                if (!exito)
                {
                    return NotFound($"Promoción con ID {id} no encontrada.");
                }

                // Asumimos el estado del servicio para el mensaje de respuesta
                var promocion = await _promocionService.GetPromocionDetalleAsync(id);
                string estado = promocion?.EstaActiva == true ? "activada" : "desactivada";

                var resultDto = new ApiResultDto
                {
                    Success = true,
                    Message = $"Promoción con ID {id} {estado} exitosamente."
                };

                return Ok(resultDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResultDto { Success = false, Message = "Error inesperado al alternar el estado de la promoción." });
            }
        }
    }
}
