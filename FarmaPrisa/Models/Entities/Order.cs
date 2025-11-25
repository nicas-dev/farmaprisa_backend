using FarmaPrisa.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey("Branch")]
    public int BranchId { get; set; }

    [Required, ForeignKey("Company")]
    public int CompanyId { get; set; }

    [Required, ForeignKey("User")]
    public int? UserId { get; set; }

    public string DeliveryType { get; set; } = null!;
    public int? AddressId { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Pending";

    public virtual Usuario User { get; set; } = null!;
    public virtual Branch Branch { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;
}
