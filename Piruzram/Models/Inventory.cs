using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piruzram.Models
{
    public class Inventory
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public virtual int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int Count { get; set; }
        public virtual int CartId { get; set; }
        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
    }
}
