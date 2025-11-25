using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities
{
    public class Brand : BaseEntity
    {

        [Key]
        public int IdBrand { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = null!;

        //[Required, ForeignKey("Branch")]
        //public int IdBranch { get; set; } aun no se define si sera global o por sucursal.

        public virtual Branch Branch { get; set; } = null!;


        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
