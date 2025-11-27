using FarmaPrisa.Models.Dtos.Products;
using FarmaPrisa.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Brands
{
    public class BrandsDto
    {
        [Key]
        public int IdBrand { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = null!;

        //nuevo
        //public bool IsActive { get; set; }

        public virtual ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
