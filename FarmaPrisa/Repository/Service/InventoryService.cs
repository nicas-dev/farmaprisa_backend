using FarmaPrisa.Data;
using FarmaPrisa.Models.Entities;
using FarmaPrisa.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Repository.Service
{
    public class InventoryService: IInventoryRepository
    {
        private readonly FarmaPrisaContext _context;
        public InventoryService(FarmaPrisaContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<InventoryDto>>> GetInventoryByBranchAsync(int branchId)
        {
            var data = await _context.Inventories
                .Where(i => i.IdBranch == branchId && i.IsActive)
                .Select(i => new InventoryDto
                {
                    IdInventory = i.IdInventory,
                    IdProduct = i.IdProduct,
                    IdBranch = i.IdBranch,
                    DateIn = i.DateIn,
                    AvailableQty = i.InventoryDetails.Sum(d => d.stock)
                })
                .ToListAsync();

            return new ApiResponse<IEnumerable<InventoryDto>>
            {
                Success = true,
                Message = "Inventory retrieved successfully",
                Data = data
            };
        }

    }
}
