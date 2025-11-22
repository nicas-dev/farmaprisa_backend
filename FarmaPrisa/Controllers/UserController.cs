using FarmaPrisa.Data; // Necesitas este using para el DbContext
using FarmaPrisa.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace FarmaPrisa.Controllers
{
    // 2. Atributos para API y ruta
    [Route("[controller]")]
    [ApiController]
    // 1. Hereda de ControllerBase
    public class UserController : ControllerBase
    {
        // 3. Campo privado para guardar la instancia del DbContext
        private readonly FarmaPrisaContext _context;

        // 4. Constructor para la Inyección de Dependencias
        // El framework de .NET se encargará de "inyectar" aquí el DbContext
        public UserController(FarmaPrisaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene los datos de un usuario específico por su ID. (Uso administrativo).
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, destinado al panel de administración para ver el detalle de una cuenta de usuario.
        /// La respuesta utiliza un DTO (UserDto) para no exponer datos sensibles como el hash de la contraseña.
        /// </remarks>
        /// <param name="id">El ID numérico del usuario que se desea consultar.</param>
        /// <response code="200">Operación exitosa. Devuelve los datos del usuario solicitado.</response>
        /// <response code="403">Acceso denegado. Si el usuario no tiene permisos de administrador.</response>
        /// <response code="404">No encontrado. Si no existe un usuario con el ID proporcionado.</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id")]
        public async Task<ActionResult<UserDto>> GetUsuario(int id)
        {
            // Buscamos el usuario en la base de datos por su Primary Key
            var usuario = await _context.Usuarios.FindAsync(id);

            // Si no se encuentra ningún usuario con ese ID, devolvemos un error 404 Not Found
            if (usuario == null)
            {
                return NotFound();
            }

            // Si lo encontramos, lo mapeamos al DTO para no exponer datos sensibles
            var usuarioDto = new UserDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Telefono = usuario.Telefono ?? string.Empty,
                EstaActivo = usuario.EstaActivo ?? false,
                ProveedorId = usuario.ProveedorId,
                PaisId = usuario.PaisId,
                PuntosFidelidad = usuario.PuntosFidelidad,
                FechaCreacion = usuario.FechaCreacion ?? DateTime.MinValue,
                FechaActualizacion = usuario.FechaActualizacion ?? DateTime.MinValue
            };

            // Devolvemos el DTO con un código de estado 200 OK
            return Ok(usuarioDto);
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente por su ID. (Uso administrativo).
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, destinado al panel de administración para modificar la información de un usuario.
        /// Campos que se pueden actualizar: NombreCompleto, Telefono, EstaActivo, PuntosFidelidad, ProveedorId y PaisId.
        /// El email y la contraseña no se modifican desde este endpoint.
        /// </remarks>
        /// <param name="id">El ID numérico del usuario que se va a actualizar.</param>
        /// <param name="userUpdateDto">Objeto con los datos actualizados del usuario.</param>
        /// <response code="204">Actualización exitosa. No devuelve contenido.</response>
        /// <response code="400">Solicitud incorrecta. Si los datos enviados en el cuerpo no son válidos.</response>
        /// <response code="403">Acceso denegado. Si el usuario no tiene permisos de administrador.</response>
        /// <response code="404">No encontrado. Si no existe un usuario con el ID proporcionado.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("id")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            // 1. Buscar el usuario existente en la base de datos
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(); // Si no existe, devolvemos 404
            }

            // 2. Actualizar los campos del usuario con los datos del DTO
            usuario.NombreCompleto = userUpdateDto.NombreCompleto;
            usuario.Telefono = userUpdateDto.Telefono;
            usuario.EstaActivo = userUpdateDto.EstaActivo;
            usuario.PuntosFidelidad = userUpdateDto.PuntosFidelidad;
            usuario.ProveedorId = userUpdateDto.ProveedorId;
            // Reemplaza la línea problemática en el método UpdateUser:
            usuario.PaisId = userUpdateDto.PaisId ?? usuario.PaisId;
            usuario.FechaActualizacion = DateTime.UtcNow; // Actualizamos la fecha de modificación

            // 3. Guardar los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Esto es para manejar casos de concurrencia, por si otro proceso intenta
                // modificar el mismo registro al mismo tiempo.
                if (!_context.Usuarios.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Devolvemos 204 No Content, que es la respuesta estándar para una actualización exitosa.
            return NoContent();
        }

        ///// <summary>
        ///// Desactiva un usuario en el sistema (borrado lógico).
        ///// </summary>
        ///// <remarks>
        ///// Esta operación no elimina el registro de la base de datos, 
        ///// simplemente cambia el campo 'esta_activo' a falso.
        ///// </remarks>
        ///// <param name="id">El ID del usuario que se va a desactivar.</param>
        ///// <response code="204">Usuario desactivado con éxito.</response>
        ///// <response code="404">No se encontró un usuario con el ID proporcionado.</response>
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    // Lógica para buscar el usuario en la base de datos
        //    var usuario = await _context.Usuarios.FindAsync(id);

        //    if (usuario == null)
        //    {
        //        return NotFound(); // Devuelve 404 si el usuario no existe
        //    }

        //    // Se cambia el estado a inactivo
        //    usuario.EstaActivo = false;

        //    // 3. Guardar los cambios en la base de datos
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        // Esto es para manejar casos de concurrencia, por si otro proceso intenta
        //        // modificar el mismo registro al mismo tiempo.
        //        if (!_context.Usuarios.Any(e => e.Id == id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent(); // Devuelve 204 indicando éxito sin contenido
        //}
    }
}