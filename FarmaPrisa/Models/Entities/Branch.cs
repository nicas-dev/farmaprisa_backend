using DocumentFormat.OpenXml.Bibliography;
using FarmaPrisa.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Branches")]
public class Branch : BaseEntity
{
    [Key]
    public int IdBranch { get; set; }

    [Required, ForeignKey("Company")]
    public int IdCompany { get; set; }

    [Required, StringLength(100)]
    public string BranchName { get; set; } = null!;

    [Required, StringLength(200)]
    public string Address { get; set; } = null!;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }  // Nuevo campo

    public virtual Company Company { get; set; } = null!;

    // Relación con usuarios
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    //public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
