namespace FarmaPrisa.Models.Dtos.Branch
{
    public class BranchWithUsersDto
    {
        public int IdBranch { get; set; }
        public string BranchName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public List<UserBranchDto> Usuarios { get; set; } = new List<UserBranchDto>();
    }
}
