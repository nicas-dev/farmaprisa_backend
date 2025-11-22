using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Promocion;
using FarmaPrisa.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FarmaPrisa.Services
{
    public interface IPromocionService
    {
        Task<int> CreatePromocionAsync(PromocionCreateDto dto);
        Task<IEnumerable<PromocionAdminDto>> GetPromocionesAdminAsync();
        Task<PromocionDetalleDto?> GetPromocionDetalleAsync(int id);
        Task<bool> UpdatePromocionAsync(int id, PromocionCreateDto dto);
        Task<bool> TogglePromocionStatusAsync(int id);
    }
    public class PromocionService : IPromocionService
    {
        private readonly FarmaPrisaContext _context;

        public PromocionService(FarmaPrisaContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePromocionAsync(PromocionCreateDto dto)
        {
            // 1. Verificación de Unicidad (Código de Cupón)
            if (await _context.Promociones.AnyAsync(p => p.CodigoCupon == dto.CodigoCupon))
            {
                throw new InvalidOperationException($"El código de cupón '{dto.CodigoCupon}' ya existe y debe ser único.");
            }

            // La creación debe ser transaccional
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 2. Mapeo a la Entidad Maestra (Promocione)
                var nuevaPromocion = new Promocione
                {
                    Nombre = dto.Nombre,
                    CodigoCupon = dto.CodigoCupon,
                    TipoDescuento = dto.TipoDescuento,
                    ValorDescuento = dto.ValorDescuento,
                    FechaInicio = dto.FechaInicio,
                    FechaFin = dto.FechaFin,
                    EstaActiva = true, // Se asume activa al momento de la creación
                    Descripcion = dto.Descripcion ?? ""
                };

                _context.Promociones.Add(nuevaPromocion);
                await _context.SaveChangesAsync(); // Guarda para obtener el ID de la promoción

                // 3. Asignación de Productos (Tabla PromocionProducto)
                if (dto.ProductoIds.Any())
                {
                    var asignacionesProducto = dto.ProductoIds.Select(pId => new PromocionProducto
                    {
                        PromocionId = nuevaPromocion.Id,
                        ProductoId = pId
                    }).ToList();
                    _context.PromocionProductos.AddRange(asignacionesProducto);
                }

                // 4. Asignación de Categorías (Tabla PromocionCategoria)
                if (dto.CategoriaIds.Any())
                {
                    var asignacionesCategoria = dto.CategoriaIds.Select(cId => new PromocionCategoria
                    {
                        PromocionId = nuevaPromocion.Id,
                        CategoriaId = cId
                    }).ToList();
                    _context.PromocionCategorias.AddRange(asignacionesCategoria);
                }

                // 5. Guardar asignaciones y confirmar transacción
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return nuevaPromocion.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Relanzar para que el controlador lo maneje
            }
        }

        public async Task<IEnumerable<PromocionAdminDto>> GetPromocionesAdminAsync()
        {
            // Carga la tabla maestra 'Promociones' e incluye las tablas pivote para el conteo
            var promociones = await _context.Promociones
                .Include(p => p.PromocionProductos)
                .Include(p => p.PromocionCategoria)
                .OrderByDescending(p => p.FechaFin) // Ordenar por fecha de fin (las más nuevas o próximas a vencer primero)
                .ToListAsync();

            // Mapeo a DTO y cálculo de conteos
            return promociones.Select(p => new PromocionAdminDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoCupon = p.CodigoCupon,
                TipoDescuento = p.TipoDescuento,
                ValorDescuento = p.ValorDescuento,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                EstaActiva = p.EstaActiva,

                // Conteo de las relaciones (cargadas por el .Include)
                TotalProductosAsociados = p.PromocionProductos.Count,
                TotalCategoriasAsociadas = p.PromocionCategoria.Count
            }).ToList();
        }

        public async Task<PromocionDetalleDto?> GetPromocionDetalleAsync(int id)
        {
            // 1. Cargar la promoción e incluir las tablas pivote
            var promocion = await _context.Promociones
                .Include(p => p.PromocionProductos)   // Tabla pivote de productos
                .Include(p => p.PromocionCategoria)  // Tabla pivote de categorías
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promocion == null)
            {
                return null; // Promoción no encontrada
            }

            // 2. Mapeo a DTO de Detalle
            return new PromocionDetalleDto
            {
                Id = promocion.Id,
                Nombre = promocion.Nombre,
                CodigoCupon = promocion.CodigoCupon,
                TipoDescuento = promocion.TipoDescuento,
                ValorDescuento = promocion.ValorDescuento,
                FechaInicio = promocion.FechaInicio,
                FechaFin = promocion.FechaFin,
                EstaActiva = promocion.EstaActiva,

                // Mapeo de Colecciones: Extraer solo los IDs de las tablas pivote
                ProductosAsociadosIds = (promocion.PromocionProductos
                                ?? new List<PromocionProducto>())
                                .Select(pp => pp.ProductoId)
                                .ToList(),

                CategoriasAsociadasIds = (promocion.PromocionCategoria
                                ?? new List<PromocionCategoria>())
                                .Select(pc => pc.CategoriaId)
                                .ToList()
            };
        }

        public async Task<bool> UpdatePromocionAsync(int id, PromocionCreateDto dto)
        {
            // 1. Cargar la promoción existente con todas sus colecciones
            var promocionAActualizar = await _context.Promociones
                .Include(p => p.PromocionProductos)
                .Include(p => p.PromocionCategoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promocionAActualizar == null)
            {
                return false; // Promoción no encontrada
            }

            // 2. Verificación de Unicidad (Código de Cupón)
            // Se verifica si el código cambió Y si el nuevo código ya existe en otro registro
            if (dto.CodigoCupon != null &&
                dto.CodigoCupon != promocionAActualizar.CodigoCupon &&
                await _context.Promociones.AnyAsync(p => p.CodigoCupon == dto.CodigoCupon))
            {
                throw new InvalidOperationException($"El código de cupón '{dto.CodigoCupon}' ya está registrado.");
            }

            // Iniciar Transacción
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 3. ACTUALIZACIÓN DE PROPIEDADES BÁSICAS (Lógica Delta)
                promocionAActualizar.Nombre = dto.Nombre ?? promocionAActualizar.Nombre;
                promocionAActualizar.Descripcion = dto.Descripcion ?? promocionAActualizar.Descripcion;
                promocionAActualizar.CodigoCupon = dto.CodigoCupon ?? promocionAActualizar.CodigoCupon;
                promocionAActualizar.TipoDescuento = dto.TipoDescuento ?? promocionAActualizar.TipoDescuento;

                if (dto.ValorDescuento > 0) promocionAActualizar.ValorDescuento = dto.ValorDescuento;
                if (dto.FechaInicio != DateTime.MinValue) promocionAActualizar.FechaInicio = dto.FechaInicio;
                if (dto.FechaFin != DateTime.MinValue) promocionAActualizar.FechaFin = dto.FechaFin;

                // 4. SINCRONIZACIÓN DE LAS TABLAS PIVOTE

                // 4a. Sincronizar Productos
                SincronizarProductos(promocionAActualizar, dto.ProductoIds);

                // 4b. Sincronizar Categorías
                SincronizarCategorias(promocionAActualizar, dto.CategoriaIds);

                // 5. Guardar Cambios y Confirmar
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // MÉTODO PRIVADO AUXILIAR PARA SINCRONIZACIÓN DE COLECCIONES
        // Este método es reutilizable y reemplaza la lógica de eliminación/adición en bucle.
        private void SincronizarProductos(Promocione promocion, List<int> nuevosIds)
        {
            var coleccionActual = promocion.PromocionProductos;

            // Obtenemos un HashSet de los IDs actuales (asegurando que solo sean valores no nulos)
            var idsActuales = coleccionActual
                .Select(pp => pp.ProductoId)
                .Where(id => id.HasValue) // Filtramos cualquier valor NULL que pueda haber
                .Select(id => id.Value)   // Extraemos el valor int no nulo
                .ToHashSet(); // Usamos ToHashSet para eficiencia

            // Eliminar: Elementos actuales que NO están en la lista nueva
            var elementosAEliminar = coleccionActual
                .Where(pp => pp.ProductoId.HasValue && !nuevosIds.Contains(pp.ProductoId.Value)) // <--- ¡CORRECCIÓN CRÍTICA AQUÍ!
                .ToList();

            _context.PromocionProductos.RemoveRange(elementosAEliminar);

            // Añadir: IDs nuevos que NO están en la lista actual
            var idsAAgregar = nuevosIds.Except(idsActuales);
            foreach (var itemId in idsAAgregar)
            {
                coleccionActual.Add(new PromocionProducto { PromocionId = promocion.Id, ProductoId = itemId });
            }
        }

        private void SincronizarCategorias(Promocione promocion, List<int> nuevosIds)
        {
            var coleccionActual = promocion.PromocionCategoria;
            var idsActuales = coleccionActual.Select(pc => pc.CategoriaId).ToHashSet();

            // Eliminar: Elementos actuales que NO están en la lista nueva
            var elementosAEliminar = coleccionActual
                .Where(pc => !nuevosIds.Contains(pc.CategoriaId))
                .ToList();
            _context.PromocionCategorias.RemoveRange(elementosAEliminar);

            // Añadir: IDs nuevos que NO están en la lista actual
            var idsAAgregar = nuevosIds.Except(idsActuales);
            foreach (var itemId in idsAAgregar)
            {
                coleccionActual.Add(new PromocionCategoria { PromocionId = promocion.Id, CategoriaId = itemId });
            }
        }

        public async Task<bool> TogglePromocionStatusAsync(int id)
        {
            var promocion = await _context.Promociones
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promocion == null)
            {
                return false; // Promoción no encontrada
            }

            // Lógica de Alternancia: Invertir el valor actual
            promocion.EstaActiva = !promocion.EstaActiva;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
