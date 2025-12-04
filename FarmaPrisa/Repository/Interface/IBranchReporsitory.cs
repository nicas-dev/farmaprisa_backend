using FarmaPrisa.Models.Dtos.Branch;

namespace FarmaPrisa.Repository.Interface
{
    public interface IBranchReporsitory
    {
        Task<List<BranchWithUsersDto>> GetAllBranchesWithUsersAsync();
        Task<BranchWithUsersDto?> GetBranchWithUsersByIdAsync(int idBranch);
        Task<bool> AssignUserToBranchAsync(int idBranch, int idUsuario);
    }
}
