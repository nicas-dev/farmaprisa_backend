using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FarmaPrisa.Models.Entities
{
    public class DetailType
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = null!; //"Warning", "Prescription"

        [JsonIgnore]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
