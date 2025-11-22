using FarmaPrisa.Models.Dtos.Categoria;
using FarmaPrisa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmaPrisa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {

        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        /// <summary>
        /// Crea una nueva categoría o subcategoría en el catálogo. Solo accesible para Administradores.
        /// </summary>
        /// <remarks>
        /// Utiliza 'CategoriaPadreId' para crear una subcategoría vinculada a una categoría principal existente.
        /// Requiere el rol 'administrador'.
        /// </remarks>
        /// <param name="dto">Datos de la nueva categoría (Nombre, Descripción, CategoriaPadreId opcional).</param>
        /// <response code="201">Categoría creada exitosamente. Devuelve el ID de la nueva categoría.</response>
        /// <response code="400">Datos de entrada inválidos.</response>
        [HttpPost]
        [Authorize(Roles = "administrador")]
        [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategoria([FromBody] CategoriaCreateUpdateDto dto)
        {
            try
            {
                // DTO
                var categoriaCreadaDto = await _categoriaService.CreateCategoriaAsync(dto);

                // Devolvemos 201 Created y el objeto completo
                return CreatedAtAction(nameof(CreateCategoria), new { id = categoriaCreadaDto.Id }, categoriaCreadaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al crear la categoría: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el catálogo completo de categorías con su estructura jerárquica (padre/hijo).
        /// </summary>
        /// <remarks>
        /// Este endpoint es utilizado por el panel de administración para mostrar el árbol de navegación.
        /// No está protegido, ya que el catálogo de categorías es público.
        /// </remarks>
        /// <response code="200">Devuelve un listado de categorías raíz con sus subcategorías anidadas.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _categoriaService.GetCategoriasJerarquiaAsync();
            return Ok(categorias);
        }

        /// <summary>4
        /// endpoint de filtro de categorias,marcas y  productos
        /// </summary>
        /// <returns></returns>
        [HttpGet("categoria/{id}/detalles")]
        public async Task<IActionResult> GetDetalles(int id)
        {
            try
            {
                var result = await _categoriaService.GetCategoriaDetallesAsync(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Categoría no encontrada"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Datos obtenidos correctamente",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Error al obtener los datos de la categoría",
                    error = ex.Message
                });
            }
        }




        /// <summary>
        /// Actualiza el nombre, descripción y/o jerarquía de una categoría existente. Solo Administradores.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite renombrar o mover una categoría a otra categoría padre.
        /// Requiere el rol 'administrador'.
        /// </remarks>
        /// <param name="id">ID de la categoría a actualizar.</param>
        /// <param name="dto">Datos de la categoría a actualizar (se actualiza solo lo que se envía).</param>
        /// <response code="200">Actualización exitosa.</response>
        /// <response code="400">Datos de entrada inválidos (ej. ID de padre inexistente).</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="404">Categoría no encontrada.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] CategoriaCreateUpdateDto dto)
        {
            try
            {
                bool exito = await _categoriaService.UpdateCategoriaAsync(id, dto);

                if (!exito)
                {
                    return NotFound($"Categoría con ID {id} no encontrada.");
                }

                return Ok(new { message = $"Categoría con ID {id} actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                // Esto captura errores como un FK inválido (ID de CategoriaPadreId inexistente)
                return StatusCode(500, "Error al actualizar la categoría: " + ex.Message);
            }
        }
    }
}
