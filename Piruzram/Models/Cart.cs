using Piruzram.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piruzram.Models
{
    public class Cart
    {
        [Key]
        [Required]
        [Display(Name = "شماره فاکتور")]
        public int Id { get; set; }
        public virtual string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public CartStatus Status { get; set; }
    }
}
