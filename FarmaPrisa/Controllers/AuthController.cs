namespace FarmaPrisa.Controllers
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using FarmaPrisa.Data;
    using FarmaPrisa.Models.Dtos;
    using FarmaPrisa.Models.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using System.Text;

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly FarmaPrisaContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(FarmaPrisaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Crea una nueva cuenta de cliente en la plataforma.
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe los datos del nuevo usuario, verifica que el correo no esté en uso,
        /// hashea la contraseña de forma segura y finalmente almacena el registro en la base de datos.
        /// Al registrarse, al usuario se le asigna automáticamente el rol de 'Cliente'.
        /// </remarks>
        /// <param name="request">Objeto que contiene la información del usuario: NombreCompleto, Email, Password y PaisId.</param>
        /// <response code="200">Registro exitoso. Devuelve un mensaje de confirmación.</response>
        /// <response code="400">Error en la solicitud. Ocurre si el correo ya está registrado o los datos son inválidos.</response>
        /// <response code="500">Error interno del servidor. Ocurre si el rol 'Cliente' no se encuentra en la base de datos.</response>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            // 1. Verificar si el correo ya existe en la base de datos
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("El correo electrónico ya está registrado.");
            }

            // Iniciamos una transacción para asegurar que ambas operaciones (crear usuario y asignar rol) sean exitosas.
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 2. Hashear la contraseña usando BCrypt
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // 3. Crear la nueva entidad de Usuario
                var newUser = new Usuario
                {
                    NombreCompleto = request.NombreCompleto,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PaisId = request.PaisId,
                    FechaCreacion = DateTime.UtcNow,
                    EstaActivo = true // Por defecto, el usuario se crea como activo.
                };

                // 4. Añadir el nuevo usuario al contexto
                _context.Usuarios.Add(newUser);

                // 5. Buscar el ID del rol "Cliente"
                var clienteRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Cliente");
                if (clienteRole == null)
                {
                    // Si el rol "Cliente" no existe, es un error de configuración del sistema.
                    // Revertimos la transacción y lanzamos un error.
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Error de configuración: El rol 'Cliente' no fue encontrado.");
                }

                // 6. Asignar el rol de "Cliente" al nuevo usuario
                // Primero guardamos el usuario para que EF Core le asigne un ID.
                await _context.SaveChangesAsync();

                var usuarioRol = new UsuarioRole
                {
                    UsuarioId = newUser.Id, // Usamos el ID del usuario recién creado
                    RolId = clienteRole.Id
                };
                _context.UsuarioRoles.Add(usuarioRol);

                // Guardamos la asignación del rol.
                await _context.SaveChangesAsync();

                // 7. Si todo fue bien, confirmamos la transacción
                await transaction.CommitAsync();

                // 8. Devolver la respuesta de sesión completa llamando al método de ayuda
                return await GenerateLoginResponse(newUser);

                // 8. Devolver una respuesta exitosa (201 Created es más apropiado para un registro)
                //return StatusCode(201, new { message = "Usuario registrado exitosamente." });
            }
            catch (Exception ex)
            {
                // Si algo falla, revertimos todos los cambios.
                await transaction.RollbackAsync();
                // Opcional: Registrar el error 'ex' para depuración.
                return StatusCode(500, "Ocurrió un error inesperado durante el registro.");
            }
        }

        /// <summary>
        /// Inicia sesión para un usuario existente y genera un token de acceso.
        /// </summary>
        /// <remarks>
        /// Este endpoint valida las credenciales del usuario (email y contraseña).
        /// Utiliza BCrypt para comparar de forma segura la contraseña proporcionada con el hash almacenado.
        /// Si la autenticación es exitosa, devuelve un token JWT que debe ser utilizado en la cabecera 'Authorization'
        /// de las siguientes peticiones a endpoints protegidos (Ej: 'Bearer {token}').
        /// </remarks>
        /// <param name="request">Objeto que contiene el Email y la Contraseña del usuario.</param>
        /// <response code="200">Autenticación exitosa. Devuelve un objeto que contiene el token JWT.</response>
        /// <response code="401">Credenciales incorrectas. Ocurre si el email no existe o la contraseña es errónea.</response>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) || user.EstaActivo == false)
            {
                return Unauthorized(new { success = false, message = "Credenciales inválidas o cuenta inactiva." });
            }

            // Llama al método de ayuda para generar la respuesta
            return await GenerateLoginResponse(user);
        }

        private string CreateToken(Usuario usuario, List<string> roles)
        {
            // Los "claims" son la información que queremos guardar en el token.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto), // Opcional: añadir el nombre
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // Añadimos cada rol como un claim de tipo 'Role'.
            // Esto permite que el atributo [Authorize(Roles = "Administrador")] funcione automáticamente.
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
                Audience = _configuration.GetValue<string>("Jwt:Audience")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<IActionResult> GenerateLoginResponse(Usuario user)
        {
            // 1. Obtener los datos adicionales (país y roles)
            var countryName = await _context.DivisionesGeograficas
                .Where(dg => dg.Id == user.PaisId)
                .Select(dg => dg.Nombre)
                .FirstOrDefaultAsync();

            var userRoles = await _context.UsuarioRoles
                .Where(ur => ur.UsuarioId == user.Id)
                .Join(_context.Roles, ur => ur.RolId, r => r.Id, (ur, r) => r.Nombre)
                .ToListAsync();

            // 2. Crear el token JWT
            string token = CreateToken(user, userRoles);

            // 3. Construir la respuesta final
            var response = new
            {
                success = true,
                token,
                usuario = new
                {
                    id = user.Id,
                    nombreCompleto = user.NombreCompleto,
                    email = user.Email,
                    telefono = user.Telefono,
                    paisId = user.PaisId,
                    paisNombre = countryName,
                    roles = userRoles
                }
            };

            return Ok(response);
        }
    }
}
