using System.ComponentModel.DataAnnotations;

namespace Piruzram.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string ImageAddress { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
