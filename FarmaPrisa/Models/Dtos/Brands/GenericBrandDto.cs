using System.ComponentModel.DataAnnotations;

namespace FarmaPrisa.Models.Dtos.Brands
{
    public class GenericBrandDto
    {
        [Key]
        public int IdBrand { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

    }
}
