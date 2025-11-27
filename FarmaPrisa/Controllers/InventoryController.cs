using FarmaPrisa.Models.Dtos.Inventory;
using FarmaPrisa.Repository.Interface;
using FarmaPrisa.Repository.Service;
using Microsoft.AspNetCore.Mvc;

namespace FarmaPrisa.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _service;

        public InventoryController(IInventoryRepository service)
        {
               _service = service; 
        }

        #region Registrar compra
        [HttpPost("comprar")]
        public async Task<IActionResult> RegistrarCompra([FromBody] CompraDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.RegistrarCompraAsync(
                    dto.ProductoId,
                    dto.SucursalId,
                    dto.Cantidad,
                    dto.Lote,
                    dto.FechaVencimiento,
                    dto.Costo,
                    "COMPRA-API" // Podrías recibir esto en el DTO si es necesario
                );
                return Ok(new { message = "Compra registrada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion Finaliza endpoint registrar compra.

        #region Listar Inventario por sucursal.




        [HttpPost("vender")]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.RegistrarVentaAsync(
                    dto.ProductoId,
                    dto.SucursalId,
                    dto.Cantidad,
                    "VENTA-API"
                );
                return Ok(new { message = "Venta registrada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("stock/{sucursalId}")]
        public async Task<IActionResult> GetStockSucursal(int sucursalId)
        {
            var stock = await _service.ObtenerInventarioPorSucursalAsync(sucursalId);
            return Ok(stock);
        }

        #endregion Aqui termina Listar Inventario por sucursal.




        [HttpGet("stock/{sucursalId}/categoria/{categoriaId}")]
        public async Task<IActionResult> GetProductosPorCategoria(int sucursalId, int categoriaId)
        {
            var result = await _service.ObtenerProductosPorSucursalYCategoriaAsync(sucursalId, categoriaId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }


    }
}
