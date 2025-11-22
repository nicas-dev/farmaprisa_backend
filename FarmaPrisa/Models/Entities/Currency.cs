using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities
{
    public class Currency:BaseEntity
    {
        [Key]
        public int IdCurrency { get; set; }

        [Required]
        [StringLength(10)]
        public string CurrencyName { get; set; }

        [Required]
        [StringLength(5)]
        public string Currencysymbol { get; set; }



    }
}
