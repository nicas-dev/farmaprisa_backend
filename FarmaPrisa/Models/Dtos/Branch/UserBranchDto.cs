namespace FarmaPrisa.Models.Dtos.Branch
{
    public class UserBranchDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }
    }
}
