using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Entities
{
    public class Brand : BaseEntity
    {

        [Key]
        public int IdBrand { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = null!;

      
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
