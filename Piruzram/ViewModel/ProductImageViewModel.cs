using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Piruzram.ViewModel
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "تصویر محصول")]
        public string ImageAddress { get; set; }

        public int ProductId { get; set; }
    }
}
