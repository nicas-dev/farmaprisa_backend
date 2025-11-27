using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Brands;
using FarmaPrisa.Models.Dtos.Categoria;
using FarmaPrisa.Models.Dtos.Inventory;
using FarmaPrisa.Models.Dtos.Producto;
using FarmaPrisa.Models.Dtos.Products;
using FarmaPrisa.Models.Entities;
using FarmaPrisa.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace FarmaPrisa.Repository.Service
{
    public class InventoryService : IInventoryRepository
    {
        private readonly FarmaPrisaContext _context;

        public InventoryService(FarmaPrisaContext context)
        {
            _context = context;
        }

        #region Registrar Compra
        public async Task RegistrarCompraAsync(int productoId, int sucursalId, int cantidad, string lote, DateTime? fechaVencimiento, decimal costoUnitario, string referenciaCompra)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tipoCompra = await _context.TiposMovimiento.FirstOrDefaultAsync(t => t.Nombre == "Compra");
                if (tipoCompra == null)
                    throw new InvalidOperationException("El tipo de movimiento 'Compra' no existe en la base de datos.");

                var ingreso = new IngresoInventario
                {
                    ProductoId = productoId,
                    SucursalId = sucursalId,
                    Lote = lote,
                    FechaVencimiento = fechaVencimiento,
                    CantidadInicial = cantidad,
                    CantidadRestante = cantidad,
                    FechaIngreso = DateTime.Now,
                    CostoUnitario = costoUnitario
                };
                _context.IngresosInventario.Add(ingreso);

                var inventarioSucursal = await _context.InventariosSucursal
                    .FirstOrDefaultAsync(i => i.ProductoId == productoId && i.SucursalId == sucursalId);

                if (inventarioSucursal == null)
                {
                    inventarioSucursal = new InventarioSucursal
                    {
                        ProductoId = productoId,
                        SucursalId = sucursalId,
                        StockTotal = 0,
                        UltimaActualizacion = DateTime.Now
                    };
                    _context.InventariosSucursal.Add(inventarioSucursal);
                }

                int saldoAnterior = inventarioSucursal.StockTotal;
                inventarioSucursal.StockTotal += cantidad;
                inventarioSucursal.UltimaActualizacion = DateTime.Now;

                var kardex = new Kardex
                {
                    ProductoId = productoId,
                    SucursalId = sucursalId,
                    Fecha = DateTime.Now,
                    TipoMovimientoId = tipoCompra.Id,
                    Cantidad = cantidad,
                    SaldoAnterior = saldoAnterior,
                    SaldoNuevo = inventarioSucursal.StockTotal,
                    Referencia = referenciaCompra,
                    CostoUnitario = costoUnitario,
                    Notas = $"Ingreso Lote: {lote}"
                };
                _context.Kardex.Add(kardex);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        #endregion

        public Task<List<InventoryDto>> ObtenerInventarioGlobalAsync()
        {
            throw new NotImplementedException();
        }

        #region Listar Inventario por Sucursal
        public async Task<List<InventoryDto>> ObtenerInventarioPorSucursalAsync(int sucursalId)
        {
            return await _context.InventariosSucursal
                .Include(i => i.Producto)
                .Include(i => i.Sucursal)
                .Where(i => i.SucursalId == sucursalId)
                .Select(i => new InventoryDto
                {
                    ProductoId = i.ProductoId,
                    Producto = i.Producto.Name,
                    SKU = i.Producto.BarCode,
                    Sucursal = i.Sucursal.BranchName,
                    StockTotal = i.StockTotal
                })
                .ToListAsync();
        }
        #endregion

        #region Registrar Venta
        public async Task RegistrarVentaAsync(int productoId, int sucursalId, int cantidadSolicitada, string referenciaVenta)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tipoVenta = await _context.TiposMovimiento.FirstOrDefaultAsync(t => t.Nombre == "Venta");
                if (tipoVenta == null)
                    throw new InvalidOperationException("El tipo de movimiento 'Venta' no existe en la base de datos.");

                var inventarioSucursal = await _context.InventariosSucursal
                    .FirstOrDefaultAsync(i => i.ProductoId == productoId && i.SucursalId == sucursalId);

                if (inventarioSucursal == null || inventarioSucursal.StockTotal < cantidadSolicitada)
                    throw new InvalidOperationException("Stock insuficiente en la sucursal.");

                int saldoAnterior = inventarioSucursal.StockTotal;
                int cantidadPendiente = cantidadSolicitada;

                var lotesDisponibles = await _context.IngresosInventario
                    .Where(i => i.ProductoId == productoId && i.SucursalId == sucursalId && i.CantidadRestante > 0)
                    .OrderBy(i => i.FechaVencimiento)
                    .ThenBy(i => i.FechaIngreso)
                    .ToListAsync();

                foreach (var lote in lotesDisponibles)
                {
                    if (cantidadPendiente <= 0) break;

                    int cantidadTomar = Math.Min(lote.CantidadRestante, cantidadPendiente);
                    lote.CantidadRestante -= cantidadTomar;
                    cantidadPendiente -= cantidadTomar;
                }

                if (cantidadPendiente > 0)
                    throw new InvalidOperationException("Inconsistencia en lotes: StockTotal indica disponibilidad pero no hay lotes suficientes.");

                inventarioSucursal.StockTotal -= cantidadSolicitada;
                inventarioSucursal.UltimaActualizacion = DateTime.Now;

                var kardex = new Kardex
                {
                    ProductoId = productoId,
                    SucursalId = sucursalId,
                    Fecha = DateTime.Now,
                    TipoMovimientoId = tipoVenta.Id,
                    Cantidad = -cantidadSolicitada,
                    SaldoAnterior = saldoAnterior,
                    SaldoNuevo = inventarioSucursal.StockTotal,
                    Referencia = referenciaVenta,
                    CostoUnitario = 0,
                    Notas = "Venta Mostrador"
                };

                _context.Kardex.Add(kardex);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        #endregion

        public async Task<CategoriaResponseDto> ObtenerProductosPorSucursalYCategoriaAsync(int sucursalId, int categoriaId)
        {
            // 1. Obtener la categoría
            var categoria = await _context.Categorias.FindAsync(categoriaId);
            if (categoria == null)
            {
                return new CategoriaResponseDto
                {
                    Success = false,
                    Message = "Categoría no encontrada",
                    Data = null
                };
            }

            // 2. Obtener productos de esa categoría en esa sucursal
            // Nota: Usamos InventarioSucursal para saber el stock, pero necesitamos filtrar por categoría del producto
            var inventario = await _context.InventariosSucursal
                .Include(i => i.Producto)
                .ThenInclude(p => p.Brand)
                .Where(i => i.SucursalId == sucursalId && i.Producto.CategoryId == categoriaId)
                .ToListAsync();

            // 3. Mapear a DTOs
            var productosDto = inventario.Select(i => new ProductDtoInvSucursal
            {
                IdProduct = i.ProductoId,
                Name = i.Producto.Name,
                BarCode= i.Producto.BarCode,
                Price = i.Producto.Price,
                Stock = i.StockTotal,
               Marca = i.Producto.Brand?.Name ?? "Sin Marca"
            }).ToList();

            // 4. Extraer marcas únicas de los productos encontrados
            var marcasDto = inventario
                .Where(i => i.Producto.Brand != null)
                .Select(i => i.Producto.Brand)
                .Distinct()
                .Select(m => new GenericBrandDto
                {
                    IdBrand = m.IdBrand,
                    Name = m.Name,
                    IsActive = m.IsActive
                })
                .ToList();

            return new CategoriaResponseDto
            {
                Success = true,
                Message = "Datos obtenidos correctamente",
                Data = new CategoriaDataDto
                {
                    Id = categoria.Id,
                    Nombre = categoria.Nombre,
                    Marcas = marcasDto,
                    Productos = productosDto
                }
            };
        }
    }
}
