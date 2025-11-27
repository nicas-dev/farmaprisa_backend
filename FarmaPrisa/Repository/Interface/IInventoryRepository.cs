using FarmaPrisa.Models.Dtos.Categoria;
using FarmaPrisa.Models.Dtos.Inventory;

namespace FarmaPrisa.Repository.Interface
{
    public interface IInventoryRepository
    {
        Task RegistrarCompraAsync(int productoId, int sucursalId, int cantidad, string lote, DateTime? fechaVencimiento, decimal costoUnitario, string referenciaCompra);
        Task RegistrarVentaAsync(int productoId, int sucursalId, int cantidadSolicitada, string referenciaVenta);
        Task<List<InventoryDto>> ObtenerInventarioGlobalAsync();
        Task<List<InventoryDto>> ObtenerInventarioPorSucursalAsync(int sucursalId);

        Task<CategoriaResponseDto> ObtenerProductosPorSucursalYCategoriaAsync(int sucursalId, int categoriaId);
    }
}
