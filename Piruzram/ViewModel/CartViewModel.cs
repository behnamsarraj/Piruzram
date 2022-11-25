using Piruzram.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Piruzram.ViewModel
{
    public class CartViewModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Display(Name = "کاربر")]
        public virtual string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
