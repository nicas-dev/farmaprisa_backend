using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities
{
    public class Company:BaseEntity
    {
        [Key]
        public int IdCompany { get; set; }

        [Required]
        [ForeignKey("Country")]
        public int IdCountry { get; set; }

        [Required]
        [ForeignKey("Currency")]
        public int IdCurrency { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(20)]
        public string Ruc { get; set; }

        [Required, MaxLength] 
        public string Address { get; set; }

        [Required]
        [StringLength(12)]
        public int Telephone { get; set; }

        [Required]
        [StringLength(20)]
        public string Email { get; set; }

        public virtual DivisionesGeografica Country { get; set; }
        public virtual Currency Currency { get; set; }

    }
}
