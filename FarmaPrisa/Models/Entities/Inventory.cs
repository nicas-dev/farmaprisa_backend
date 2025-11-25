using FarmaPrisa.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inventory : BaseEntity
{
    [Key]
    public int IdInventory { get; set; }

    [Required, ForeignKey("Product")]
    public int IdProduct { get; set; }

    [Required, ForeignKey("Provider")]
    public int IdProvider { get; set; }

    [Required, ForeignKey("Branch")]
    public int IdBranch { get; set; }

    public DateTime DateIn { get; set; } = DateTime.UtcNow;

    public virtual Product Product { get; set; } = null!;
    public virtual Proveedore Provider { get; set; } = null!;
    public virtual Branch Branch { get; set; } = null!;

    public virtual ICollection<InventoryDetail> InventoryDetails { get; set; } = new List<InventoryDetail>();
}
