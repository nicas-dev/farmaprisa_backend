using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmaPrisa.Models.Entities
{
    public class ProductDetail
    {

        [Key]
        public int IdProductDetail { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("DetailType")]
        public int DetailTypeId { get; set; }

        [Required, MaxLength]
        public string DetailText { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
        public virtual DetailType DetailType { get; set; } = null!;
    }
}
