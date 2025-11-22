using FarmaPrisa.Data;
using Microsoft.AspNetCore.Mvc;

namespace FarmaPrisa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly FarmaPrisaContext _context;

        // El DbContext se inyecta gracias a la configuración en Program.cs
        public HealthCheckController(FarmaPrisaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Realiza una prueba de conexión a la base de datos MariaDB.
        /// </summary>
        /// <remarks>
        /// Endpoint de diagnóstico ("Health Check") para verificar que la API puede establecer
        /// una conexión exitosa con la base de datos. Es útil para monitorear el estado del servicio.
        /// </remarks>
        /// <response code="200">Conexión exitosa. El servicio está operativo.</response>
        /// <response code="400">La conexión se intentó pero falló sin generar una excepción crítica.</response>
        /// <response code="500">Error interno del servidor. La conexión falló debido a una excepción (ej. credenciales incorrectas, firewall).</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpGet("db-connection")]
        public async Task<IActionResult> CheckDbConnection()
        {
            try
            {
                // Este método intenta conectarse a la DB y devuelve true si lo logra.
                if (await _context.Database.CanConnectAsync())
                {
                    return Ok("¡Conexión a la base de datos exitosa! ✅");
                }
                else
                {
                    return BadRequest("No se pudo conectar a la base de datos, pero no hubo excepción.");
                }
            }
            catch (Exception ex)
            {
                // Si hay un error (ej. contraseña incorrecta), caerá aquí.
                return StatusCode(500, $"Error en la conexión: {ex.Message}");
            }
        }
    }
}
