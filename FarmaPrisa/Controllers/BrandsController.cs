using FarmaPrisa.Models.Dtos.Brands;
using FarmaPrisa.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmaPrisa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandsController : Controller
    {
       
        private readonly IBrandRepository _brandRepository;

        public BrandsController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        /// <summary>
        /// Devuelve Listado De Marcas.
        /// </summary>
        /// <returns></returns>
        //listar brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenericBrandDto>>> GetAll()
        {
            var brands = await _brandRepository.GetAllBrandsAsync();
            return Ok(brands);
        }


        /// <summary>
        /// Devuelve Listado De Marcas Por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //listar por id
        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);

            if (brand == null)
                return NotFound(new { success = false, message = "Marca no encontrada" });

            return Ok(brand);
        }

        /// <summary>
        /// Crear Nueva Marca.
        /// Solo Administradores.
        /// </summary>
        /// <param name="brandDto"></param>
        /// <returns></returns>
        //guardar brands
        [HttpPost]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> AddBrand([FromBody] GenericBrandDto brandDto)
        {
            try
            {
                await _brandRepository.AddAsync(brandDto);

                return Ok(new
                {
                    success = true,
                    message = "Marca guardada correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Error al guardar la marca",
                    error = ex.Message
                });
            }
        }


        /// <summary>
        /// Actualizar Marcas.
        /// Solo Administradores.
        /// </summary>
        /// <param name="brandDto"></param>
        /// <returns></returns>
        //update brands
        [HttpPut]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> UpdateBrand([FromBody] GenericBrandDto brandDto)
        {
            await _brandRepository.UpdateAsync(brandDto);
            return Ok(new { success = true, message = "Marca actualizada correctamente" });
        }

        /// <summary>
        /// Desactivar Marcas.
        /// Solo Administradores.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //desactivar brands
        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            await _brandRepository.DeleteAsync(id);
            return Ok(new { success = true, message = "Marca desactivada correctamente" });
        }

    }

}

