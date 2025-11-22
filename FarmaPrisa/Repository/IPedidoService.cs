using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos;
using FarmaPrisa.Models.Dtos.Producto;
using FarmaPrisa.Models.Dtos.Usuario;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Services
{
    public interface IPedidoService
    {
        // Obtener todos los pedidos para el administrador
        Task<IEnumerable<PedidoAdminDto>> GetPedidosAdminAsync();
        Task<PedidoDetalleDto?> GetPedidoDetalleAdminAsync(int id);
        Task<DashboardDataDto> GetDashboardDataAsync();
    }
    public class PedidoService : IPedidoService
    {
        private readonly FarmaPrisaContext _context;

        public PedidoService(FarmaPrisaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoAdminDto>> GetPedidosAdminAsync()
        {
            // 1. Cargar Pedidos e incluir el Usuario para obtener el nombre del cliente
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario) // Incluir al usuario relacionado
                .ToListAsync();

            // 2. Mapeo a DTO
            return pedidos.Select(p => new PedidoAdminDto
            {
                Id = p.Id,
                FechaPedido = p.FechaPedido ?? DateTime.MinValue, // Manejo de nullable
                Estado = p.Estado,
                TipoEntrega = p.TipoEntrega,
                Total = p.Total,
                UsuarioId = p.UsuarioId ?? 0,
                ClienteNombre = p.Usuario?.NombreCompleto ?? "Cliente Eliminado" // Manejo de nulidad
            }).ToList();
        }

        public async Task<PedidoDetalleDto?> GetPedidoDetalleAdminAsync(int id)
        {
            // Carga del pedido principal y todas las dependencias necesarias:
            // Usuario, DetallesPedido, Productos, Direccion, Transacciones, Sucursal
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Direccion) // Para pedidos a domicilio
                .Include(p => p.SucursalRecogida) // Para pedidos de recoger en tienda
                .Include(p => p.Transacciones) // Para el estado del pago
                .Include(p => p.DetallesPedidos) // Colección de artículos
                    .ThenInclude(dp => dp.Producto) // Incluir el nombre e imagen del producto
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return null;
            }

            // Mapeo a DTO (La lógica central)
            var transaccion = pedido.Transacciones.FirstOrDefault();

            return new PedidoDetalleDto
            {
                Id = pedido.Id,
                FechaPedido = pedido.FechaPedido ?? DateTime.MinValue,
                Estado = pedido.Estado,
                TipoEntrega = pedido.TipoEntrega,
                Subtotal = pedido.Subtotal,
                CostoEnvio = pedido.CostoEnvio,
                MontoDescuento = pedido.MontoDescuento,
                MontoImpuesto = pedido.MontoImpuesto,
                Total = pedido.Total,

                // Mapeo de Usuario
                UsuarioId = pedido.UsuarioId ?? 0,
                ClienteNombre = pedido.Usuario?.NombreCompleto ?? "N/A",
                ClienteEmail = pedido.Usuario?.Email ?? "N/A",

                // Mapeo de Ubicación y Recogida
                SucursalRecogidaNombre = pedido.SucursalRecogida?.Nombre,
                DireccionCompletaEnvio = pedido.Direccion?.DireccionCompleta,

                // Mapeo de Artículos del Pedido (detalles_pedido)
                Articulos = pedido.DetallesPedidos.Select(dp => new DetallePedidoItemDto
                {
                    ProductoId = dp.ProductoId ?? 0,
                    NombreProducto = dp.Producto.Name,
                    //ImagenUrl = dp.Producto.ImagenUrl ?? "",
                    Cantidad = dp.Cantidad,
                    PrecioUnitario = dp.PrecioUnitario,
                    Subtotal = dp.PrecioUnitario * dp.Cantidad
                }).ToList(),

                // Mapeo de Pago
                EstadoTransaccion = transaccion?.Estado,
                PasarelaPago = transaccion?.PasarelaPago
            };
        }

        public async Task<DashboardDataDto> GetDashboardDataAsync()
        {
            var hoy = DateTime.UtcNow.Date;
            var hace6Meses = hoy.AddMonths(-6);
            var hace30Dias = hoy.AddDays(-30);

            // 1. Ingresos y Pedidos Diarios
            var pedidosHoy = await _context.Pedidos
                .Where(p => p.FechaPedido.HasValue && p.FechaPedido.Value.Date == hoy)
                .ToListAsync();

            var pedidosHistorial = await _context.Pedidos
                .Where(p => p.FechaPedido.HasValue && p.FechaPedido.Value.Date >= hace6Meses)
                .ToListAsync();

            // 2. Ventas Mensuales
            var ventasDB = pedidosHistorial
                .Where(p => p.FechaPedido.Value.Date >= hace6Meses)
                .GroupBy(p => new { p.FechaPedido.Value.Year, p.FechaPedido.Value.Month })
                .ToDictionary(
                    g => $"{g.Key.Year}-{g.Key.Month:00}",
                    g => g.Sum(p => p.Total)
                );
            var ventas6Meses = GenerarRangoMensual(hace6Meses, hoy, ventasDB); // <-- Usar la función de relleno

            // 3. Pedidos Diarios (Últimos 30 días)
            var pedidosDB = pedidosHistorial
                .Where(p => p.FechaPedido.Value.Date >= hace30Dias)
                .GroupBy(p => p.FechaPedido.Value.Date)
                .ToDictionary(
                    g => g.Key.ToString("yyyy-MM-dd"),
                    g => g.Count()
                );
            var pedidos30Dias = GenerarRangoDiario(hace30Dias, hoy, pedidosDB);

            // 4. Usuarios Registrados (Tabla Usuarios)
            var totalUsuarios = await _context.Usuarios.CountAsync();

            // 5. Ventas y Pedidos Históricos (para gráficos)
            var pedidos6Meses = await _context.Pedidos
                .Where(p => p.FechaPedido.HasValue && p.FechaPedido.Value.Date >= hace6Meses)
                .ToListAsync();

            // 6. Lógica de Mapeo y Agregación
            var resultado = new DashboardDataDto
            {
                IngresosDiarios = pedidosHoy.Sum(p => p.Total),
                NuevasOrdenesDiarias = pedidosHoy.Count,
                TotalUsuariosRegistrados = totalUsuarios,

                // Agregación para Gráficos (Ventas y Pedidos por día/mes)
                VentasUltimos6Meses = ventas6Meses,
                PedidosDiarios = pedidos30Dias,
            };

            return resultado;
        }

        private Dictionary<string, decimal> GenerarRangoMensual(DateTime start, DateTime end, Dictionary<string, decimal> data)
        {
            var resultado = new Dictionary<string, decimal>();
            var fechaActual = new DateTime(start.Year, start.Month, 1);

            while (fechaActual <= end)
            {
                var clave = fechaActual.ToString("yyyy-MM");
                // Añade el valor de la BD si existe; si no, añade 0
                resultado[clave] = data.GetValueOrDefault(clave, 0);
                fechaActual = fechaActual.AddMonths(1);
            }
            return resultado;
        }

        private Dictionary<string, int> GenerarRangoDiario(DateTime start, DateTime end, Dictionary<string, int> data)
        {
            var resultado = new Dictionary<string, int>();
            var fechaActual = start.Date;

            while (fechaActual <= end.Date)
            {
                var clave = fechaActual.ToString("yyyy-MM-dd");
                // Añade el valor de la BD si existe; si no, añade 0
                resultado[clave] = data.GetValueOrDefault(clave, 0);
                fechaActual = fechaActual.AddDays(1);
            }
            return resultado;
        }
    }
}
