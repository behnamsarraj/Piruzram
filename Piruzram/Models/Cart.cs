using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piruzram.Models
{
    public class Cart
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public virtual string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

    }
}
