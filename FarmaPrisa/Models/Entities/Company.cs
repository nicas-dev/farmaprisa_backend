using FarmaPrisa.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Company : BaseEntity
{
    [Key]
    public int IdCompany { get; set; }

    [Required, ForeignKey("Country")]
    public int IdCountry { get; set; }

    [Required, ForeignKey("Currency")]
    public int IdCurrency { get; set; }

    [Required, StringLength(50)]
    public string CompanyName { get; set; } = null!;

    [Required, StringLength(20)]
    public string Ruc { get; set; } = null!;

    [Required, StringLength(200)]
    public string Address { get; set; } = null!;

    [Required, StringLength(12)]
    public string Telephone { get; set; } = null!;

    [Required, StringLength(50)]
    public string Email { get; set; } = null!;

    public virtual Currency Currency { get; set; } = null!;
    public virtual DivisionesGeografica Country { get; set; } = null!;

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
