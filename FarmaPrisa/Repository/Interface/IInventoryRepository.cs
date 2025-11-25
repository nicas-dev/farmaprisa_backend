using FarmaPrisa.Models.Entities;

namespace FarmaPrisa.Repository.Interface
{
    public interface IInventoryRepository
    {
        Task<ApiResponse<IEnumerable<InventoryDto>>> GetInventoryByBranchAsync(int branchId);
    }
}
