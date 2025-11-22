using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Proveedor;
using FarmaPrisa.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Services
{
    public interface IProveedorService
    {
        Task<int> CreateProveedorAsync(ProveedorCreateUpdateDto dto);
        Task<IEnumerable<ProveedorAdminDto>> GetProveedoresAdminAsync();
        Task<bool> UpdateProveedorAsync(int id, ProveedorCreateUpdateDto dto);
        //Task<bool> DeleteProveedorAsync(int id);
        Task<bool> ToggleProveedorStatusAsync(int id);
        Task<ProveedorAdminDto?> GetProveedorDetalleAsync(int id);
    }

    public class ProveedorService : IProveedorService
    {
        private readonly FarmaPrisaContext _context;

        public ProveedorService(FarmaPrisaContext context)
        {
            _context = context;
        }

        public async Task<int> CreateProveedorAsync(ProveedorCreateUpdateDto dto)
        {
            // 1. Verificar Unicidad por Nombre, Email, Teléfono e Identificación Fiscal

            // Verificación de unicidad por Nombre (Existente)
            if (await _context.Proveedores.AnyAsync(p => p.Nombre == dto.Nombre))
            {
                throw new InvalidOperationException($"Ya existe un proveedor registrado con el nombre: {dto.Nombre}.");
            }

            // Verificación de unicidad por Identificación Fiscal (Solo si se proporciona)
            if (!string.IsNullOrEmpty(dto.IdentificacionFiscal) &&
                await _context.Proveedores.AnyAsync(p => p.IdentificacionFiscal == dto.IdentificacionFiscal))
            {
                throw new InvalidOperationException($"Ya existe un proveedor con la Identificación Fiscal: {dto.IdentificacionFiscal}.");
            }

            // Verificación de unicidad por Email de Contacto (Solo si se proporciona)
            if (!string.IsNullOrEmpty(dto.EmailContacto) &&
                await _context.Proveedores.AnyAsync(p => p.EmailContacto == dto.EmailContacto))
            {
                throw new InvalidOperationException($"Ya existe un proveedor registrado con el Email de Contacto: {dto.EmailContacto}.");
            }

            // Verificación de unicidad por Teléfono de Contacto (Solo si se proporciona)
            if (!string.IsNullOrEmpty(dto.TelefonoContacto) &&
                await _context.Proveedores.AnyAsync(p => p.TelefonoContacto == dto.TelefonoContacto))
            {
                throw new InvalidOperationException($"Ya existe un proveedor registrado con el Teléfono de Contacto: {dto.TelefonoContacto}.");
            }

            // 2. Mapeo a la Entidad Proveedore (Si todas las verificaciones pasan)
            var nuevoProveedor = new Proveedore
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                LogoUrl = dto.LogoUrl,
                Direccion = dto.Direccion,
                NombreContacto = dto.NombreContacto,
                EmailContacto = dto.EmailContacto,
                TelefonoContacto = dto.TelefonoContacto,
                IdentificacionFiscal = dto.IdentificacionFiscal
            };

            // 3. Guardar en la Base de Datos
            _context.Proveedores.Add(nuevoProveedor);
            await _context.SaveChangesAsync();

            return nuevoProveedor.Id;
        }

        public async Task<IEnumerable<ProveedorAdminDto>> GetProveedoresAdminAsync()
        {
            // Carga todos los proveedores, ya que es la vista de administración
            var proveedores = await _context.Proveedores
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            // Mapeo a DTO (reutilizando el DTO público)
            return proveedores.Select(p => new ProveedorAdminDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                LogoUrl = p.LogoUrl,

                // Mapeo de la información sensible/administrativa
                Direccion = p.Direccion,
                NombreContacto = p.NombreContacto,
                EmailContacto = p.EmailContacto,
                TelefonoContacto = p.TelefonoContacto,
                IdentificacionFiscal = p.IdentificacionFiscal,
                EstaActivo = p.EstaActivo
            }).ToList();
        }

        public async Task<bool> UpdateProveedorAsync(int id, ProveedorCreateUpdateDto dto)
        {
            var proveedorAActualizar = await _context.Proveedores
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedorAActualizar == null)
            {
                return false; // Proveedor no encontrado
            }

            // 1. VERIFICACIÓN DE UNICIDAD (Excluyendo el registro actual que se está modificando)

            // Validar Nombre (Esta ya está bien, verifica si el nombre cambió Y si el nuevo nombre existe)
            if (!string.IsNullOrEmpty(dto.Nombre) &&
                dto.Nombre != proveedorAActualizar.Nombre &&
                await _context.Proveedores.AnyAsync(p => p.Nombre == dto.Nombre))
            {
                throw new InvalidOperationException($"Ya existe un proveedor registrado con el nombre: {dto.Nombre}.");
            }

            // Validar Identificación Fiscal
            if (!string.IsNullOrEmpty(dto.IdentificacionFiscal) &&
                dto.IdentificacionFiscal != proveedorAActualizar.IdentificacionFiscal &&
                // CRÍTICO: Excluir el registro actual (p.Id != id)
                await _context.Proveedores.AnyAsync(p => p.IdentificacionFiscal == dto.IdentificacionFiscal && p.Id != id))
            {
                throw new InvalidOperationException($"Ya existe un proveedor con la Identificación Fiscal: {dto.IdentificacionFiscal}.");
            }

            // Validar Email
            if (!string.IsNullOrEmpty(dto.EmailContacto) &&
                dto.EmailContacto != proveedorAActualizar.EmailContacto &&
                // CRÍTICO: Excluir el registro actual (p.Id != id)
                await _context.Proveedores.AnyAsync(p => p.EmailContacto == dto.EmailContacto && p.Id != id))
            {
                throw new InvalidOperationException($"Ya existe un proveedor registrado con el Email de Contacto: {dto.EmailContacto}.");
            }

            // Validar Teléfono
            if (!string.IsNullOrEmpty(dto.TelefonoContacto) &&
                dto.TelefonoContacto != proveedorAActualizar.TelefonoContacto &&
                // CRÍTICO: Excluir el registro actual (p.Id != id)
                await _context.Proveedores.AnyAsync(p => p.TelefonoContacto == dto.TelefonoContacto && p.Id != id))
            {
                throw new InvalidOperationException($"Ya existe un proveedor registrado con el Teléfono de Contacto: {dto.TelefonoContacto}.");
            }

            // 2. APLICAR CAMBIOS (Actualizar solo lo que no es null)
            proveedorAActualizar.Nombre = dto.Nombre ?? proveedorAActualizar.Nombre;
            proveedorAActualizar.Descripcion = dto.Descripcion ?? proveedorAActualizar.Descripcion;
            proveedorAActualizar.LogoUrl = dto.LogoUrl ?? proveedorAActualizar.LogoUrl;
            proveedorAActualizar.Direccion = dto.Direccion ?? proveedorAActualizar.Direccion;
            proveedorAActualizar.NombreContacto = dto.NombreContacto ?? proveedorAActualizar.NombreContacto;
            proveedorAActualizar.EmailContacto = dto.EmailContacto ?? proveedorAActualizar.EmailContacto;
            proveedorAActualizar.TelefonoContacto = dto.TelefonoContacto ?? proveedorAActualizar.TelefonoContacto;
            proveedorAActualizar.IdentificacionFiscal = dto.IdentificacionFiscal ?? proveedorAActualizar.IdentificacionFiscal;

            // 3. GUARDAR CAMBIOS
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> DeleteProveedorAsync(int id)
        //{
        //    // Buscamos el proveedor por su ID
        //    var proveedorAInactivar = await _context.Proveedores
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (proveedorAInactivar == null)
        //    {
        //        return false; // Proveedor no encontrado
        //    }

        //    // Soft Delete: Inactiva el proveedor
        //    proveedorAInactivar.EstaActivo = false;

        //    await _context.SaveChangesAsync();

        //    return true; // Éxito al inactivar
        //}
        public async Task<bool> ToggleProveedorStatusAsync(int id)
        {
            // 1. Buscamos el proveedor por su ID
            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null)
            {
                return false; // Proveedor no encontrado
            }

            // 2. Lógica de Alternancia (Toggle): Invertir el valor actual de EstaActivo
            proveedor.EstaActivo = !proveedor.EstaActivo; // <-- ¡Invertimos el estado!

            // 3. Guardar los cambios
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ProveedorAdminDto?> GetProveedorDetalleAsync(int id)
        {
            // Buscamos el proveedor por su ID
            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null)
            {
                return null; // Proveedor no encontrado
            }

            // Mapeo al DTO de Administración (ProveedorAdminDto)
            return new ProveedorAdminDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Descripcion = proveedor.Descripcion,
                LogoUrl = proveedor.LogoUrl,
                Direccion = proveedor.Direccion,
                NombreContacto = proveedor.NombreContacto,
                EmailContacto = proveedor.EmailContacto,
                TelefonoContacto = proveedor.TelefonoContacto,
                IdentificacionFiscal = proveedor.IdentificacionFiscal,
                EstaActivo = proveedor.EstaActivo
            };
        }
    }
}
