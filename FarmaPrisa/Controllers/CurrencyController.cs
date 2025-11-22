using FarmaPrisa.Models.Dtos.Currency;
using FarmaPrisa.Models.Entities;
using FarmaPrisa.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyRepository _currencyService;

    public CurrencyController(ICurrencyRepository currencyService)
    {
        _currencyService = currencyService;
    }


    /// <summary>
    /// Devuelve listado de monedas.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _currencyService.GetAllAsync();

        return Ok(new ApiResponse<IEnumerable<CurrencyDto>>
        {
            Success = true,
            Message = "Listado de monedas obtenido correctamente.",
            Data = result
        });
    }

    /// <summary>
    /// Devuelve Listado De Monedas Por Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _currencyService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = $"No se encontró la moneda con ID {id}."
            });
        }

        return Ok(new ApiResponse<CurrencyDto>
        {
            Success = true,
            Message = "Moneda encontrada.",
            Data = result
        });
    }

    /// <summary>
    /// Crea Monedas. Solo Administradores.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "administrador")]
    public async Task<IActionResult> Create([FromBody] CreateCurrencyDto dto)
    {
        var result = await _currencyService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = result.IdCurrency }, new ApiResponse<CurrencyDto>
        {
            Success = true,
            Message = "Moneda creada exitosamente.",
            Data = result
        });
    }

    /// <summary>
    /// Actualiza Monedas.Solo Administradores.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCurrencyDto dto)
    {
        var updated = await _currencyService.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = $"No se pudo actualizar la moneda con ID {id}."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Moneda actualizada correctamente."
        });
    }

    /// <summary>
    /// Desactiva Monedas. Solo Administradores.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [HttpDelete("{id}")]
    [Authorize(Roles = "administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _currencyService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = $"No se pudo eliminar la moneda con ID {id}."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Moneda eliminada correctamente."
        });
    }
}
