using FarmaPrisa.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class InventoryDetail : BaseEntity
{
    [Key]
    public int IdInventoryDetail { get; set; }

    [Required, ForeignKey("Inventory")]
    public int IdInventory { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public decimal Cost { get; set; }

    [Required]
    public decimal MinQty { get; set; }

    [Required]
    public decimal MaxQty { get; set; }

    [Required]
    public decimal stock { get; set; }

    [Required]
    public DateTime? ExpirationDate { get; set; }

    [Required]
    [StringLength(50)]
    public string? BatchNumber { get; set; }

    public virtual Inventory Inventory { get; set; } = null!;
}
