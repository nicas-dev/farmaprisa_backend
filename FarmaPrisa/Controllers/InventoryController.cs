using FarmaPrisa.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FarmaPrisa.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _inventoryService;

        public InventoryController(IInventoryRepository inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetInventoryByBranch(int branchId)
        {
            var response = await _inventoryService.GetInventoryByBranchAsync(branchId);
            return Ok(response);
        }
    }
}
