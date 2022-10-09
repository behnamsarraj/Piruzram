using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piruzram.Models
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public virtual string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual int ProductCateguryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }

        public ICollection<ProductImage> Images { get; set; }
        public ICollection<Inventory> Inventories { get; set; }


    }
}
