using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Usuario;
using FarmaPrisa.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Services
{
    public interface IUsuarioService
    {
        // Este método es exclusivo para el administrador
        Task<IEnumerable<UsuarioAdminDto>> GetTodosLosUsuariosAsync();
        Task<IEnumerable<RolDto>> GetRolesAsync();
        Task<bool> UpdateUserRolesAsync(int userId, List<int> rolIds);
        Task<int> CreateUsuarioAdminAsync(UsuarioAdminCreateDto dto);
        Task<bool> ToggleUserStatusAsync(int id);
        Task<UsuarioAdminDto?> GetUsuarioDetalleAsync(int id);
    }
    public class UsuarioService : IUsuarioService
    {
        private readonly FarmaPrisaContext _context;

        public UsuarioService(FarmaPrisaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioAdminDto>> GetTodosLosUsuariosAsync()
        {
            // 1. Cargar datos, IGNORANDO el filtro global 'EstaActivo = true'
            var usuarios = await _context.Usuarios
                .IgnoreQueryFilters()
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol) // Incluir la tabla pivote y el objeto Rol para obtener el nombre
                .ToListAsync();

            // 2. Mapeo a DTO y extracción de Roles
            return usuarios.Select(u => new UsuarioAdminDto
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto,
                Email = u.Email,
                Telefono = u.Telefono,
                EstaActivo = u.EstaActivo ?? true,
                PuntosFidelidad = u.PuntosFidelidad,
                FechaCreacion = u.FechaCreacion ?? DateTime.MinValue,
                Roles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList() // Mapeamos los nombres de los roles
            }).ToList();
        }

        public async Task<bool> ToggleUserStatusAsync(int id)
        {
            // 1. Buscamos el usuario por su ID
            var usuario = await _context.Usuarios
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return false; // Usuario no encontrado
            }

            // 2. Lógica de Alternancia (Toggle): Invertir el valor actual de EstaActivo
            usuario.EstaActivo = !usuario.EstaActivo; // ¡Alternamos el estado!

            // 3. Guardamos los cambios
            await _context.SaveChangesAsync();

            // Devolvemos true si el guardado fue exitoso
            return true;
        }

        public async Task<IEnumerable<RolDto>> GetRolesAsync()
        {
            // Carga todos los roles de la tabla maestra 'roles'
            var roles = await _context.Roles
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            // Mapeo a DTO
            return roles.Select(r => new RolDto
            {
                Id = r.Id,
                Nombre = r.Nombre
            }).ToList();
        }

        public async Task<bool> UpdateUserRolesAsync(int userId, List<int> rolIds)
        {
            // 1. Cargar el usuario y sus roles actuales para sincronización
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuario == null)
            {
                return false; // Usuario no encontrado
            }

            // 2. Lógica de Sincronización: Determinar qué añadir y qué eliminar

            // Convertimos los roles actuales del usuario a una lista simple de IDs
            var rolesActualesIds = usuario.UsuarioRoles.Select(ur => ur.RolId).ToList();

            // Roles a añadir: Los IDs que están en la lista nueva (rolIds) pero no en la lista actual
            var rolesAAgregarIds = rolIds.Except(rolesActualesIds).ToList();

            // Roles a eliminar: Las entidades (UsuarioRole) que no están en la lista nueva (rolIds)
            var rolesAEliminar = usuario.UsuarioRoles
                .Where(ur => !rolIds.Contains(ur.RolId))
                .ToList();

            // 3. Ejecutar Cambios

            // Eliminar: Le decimos al contexto que remueva las entidades pivote obsoletas
            _context.UsuarioRoles.RemoveRange(rolesAEliminar);

            // Añadir: Creamos nuevas entidades pivote (UsuarioRole) por cada nuevo ID
            foreach (var rolId in rolesAAgregarIds)
            {
                usuario.UsuarioRoles.Add(new UsuarioRole
                {
                    UsuarioId = userId,
                    RolId = rolId
                });
            }

            // 4. Persistir los Cambios en la Base de Datos
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateUsuarioAdminAsync(UsuarioAdminCreateDto dto)
        {
            // 1. Verificar si el correo ya existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                throw new InvalidOperationException("El correo electrónico ya está registrado.");
            }

            // 2. Obtener los IDs de los roles solicitados
            var rolesDb = await _context.Roles
                .Where(r => dto.Roles.Contains(r.Nombre))
                .ToDictionaryAsync(r => r.Nombre, r => r.Id);

            if (rolesDb.Count != dto.Roles.Count)
            {
                // Esto significa que se solicitó un rol que no existe en la BD (ej: "invalido")
                throw new KeyNotFoundException("Uno o más roles especificados no existen.");
            }

            // 3. Crear el usuario dentro de una transacción
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Hashear la contraseña usando BCrypt
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                // Crear la nueva entidad de Usuario
                var newUser = new Usuario
                {
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    PasswordHash = passwordHash,
                    EstaActivo = true, // Creado como activo por defecto
                    PuntosFidelidad = 0,
                    FechaCreacion = DateTime.UtcNow,
                    PaisId = dto.PaisId
                };

                _context.Usuarios.Add(newUser);
                await _context.SaveChangesAsync(); // Guarda para obtener newUser.Id

                // 4. Asignar los roles
                var nuevosRoles = rolesDb.Values.Select(rolId => new UsuarioRole
                {
                    UsuarioId = newUser.Id,
                    RolId = rolId
                }).ToList();

                _context.UsuarioRoles.AddRange(nuevosRoles);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return newUser.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                // Relanzar una excepción genérica o específica para manejo de errores en el controlador
                throw;
            }
        }

        public async Task<UsuarioAdminDto?> GetUsuarioDetalleAsync(int id)
        {
            // 1. Cargar el usuario, ignorando el filtro global para asegurar que se encuentre
            var usuario = await _context.Usuarios
                .IgnoreQueryFilters()
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return null; // Usuario no encontrado
            }

            // 2. Mapeo a DTO (UsuarioAdminDto)
            return new UsuarioAdminDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                EstaActivo = usuario.EstaActivo,
                PuntosFidelidad = usuario.PuntosFidelidad,
                // Las fechas deben manejar la nulabilidad con '?? DateTime.MinValue' o con un cast seguro
                FechaCreacion = usuario.FechaCreacion,
                FechaActualizacion = usuario.FechaActualizacion,
                Roles = usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList()
            };
        }
    }
}
