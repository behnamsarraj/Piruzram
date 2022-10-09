using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piruzram.Models
{
    public class ProductImage
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string ImageAddress { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
