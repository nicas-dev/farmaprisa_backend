using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class GeografiaController : ControllerBase
{
    private readonly FarmaPrisaContext _context;

    public GeografiaController(FarmaPrisaContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Recupera la lista de países disponibles en la plataforma.
    /// </summary>
    /// <remarks>
    /// Este endpoint consulta la tabla 'divisiones_geograficas' para obtener los registros de tipo 'País'.
    /// Se utiliza principalmente para poblar los selectores en los formularios de registro de usuarios y direcciones.
    /// Devuelve una lista simplificada (DTO) con el Id y el Nombre de cada país.
    /// </remarks>
    /// <response code="200">Operación exitosa. Devuelve un array con los países, cada uno con su Id y Nombre.</response>
    [ProducesResponseType(typeof(IEnumerable<PaisDto>), StatusCodes.Status200OK)]
    [HttpGet("paises")]
    public async Task<ActionResult<IEnumerable<PaisDto>>> GetPaises()
    {
        var paises = await _context.DivisionesGeograficas
            .Where(d => d.TipoDivision == "País") // Filtramos solo por 'País'
            .Select(d => new PaisDto // Transformamos al DTO para no exponer toda la data
            {
                Id = d.Id,
                Nombre = d.Nombre
            })
            .ToListAsync();

        return Ok(paises);
    }

    ///// <summary>
    ///// Endpoint de prueba para verificar que la autenticación funciona.
    ///// Solo accesible con un token JWT válido.
    ///// </summary>
    ///// <response code="200">Devuelve el email del usuario autenticado.</response>
    ///// <response code="401">Si no se provee un token válido.</response>
    //[HttpGet("perfil-secreto")]
    //[Authorize] // <-- ¡ESTA ES LA CERRADURA!
    //public IActionResult GetPerfilSecreto()
    //{
    //    // El sistema de autorización nos da acceso a los datos del token
    //    var userEmail = User.FindFirstValue(ClaimTypes.Email);

    //    if (userEmail == null)
    //    {
    //        return NotFound("No se encontró el email en el token.");
    //    }

    //    return Ok(new { message = $"Hola, {userEmail}! Este es un secreto." });
    //}
}