using System.ComponentModel.DataAnnotations;

namespace Piruzram.Models
{
    public class ProductCategory
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
