using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Piruzram.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "نام محصول")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "(قیمت (ريال")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "دسته بندی")]
        public int CategoryId { get; set; }
        [Display(Name = "دسته بندی")]
        public string? CategoryName { get; set; }

        public List<SelectListItem>? CategoriesDropDown { get; set; }

        public int Inventory { get; set; }

    }
}
