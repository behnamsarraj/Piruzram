using System.ComponentModel.DataAnnotations;

namespace Piruzram.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public string PictureLocation { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [Required]
        public virtual int ProductCateguryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

    }
}
