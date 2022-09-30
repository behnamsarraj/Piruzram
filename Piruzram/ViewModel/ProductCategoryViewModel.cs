using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Piruzram.ViewModel
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "نام محصول")]
        public string Name { get; set; }

    }
}
