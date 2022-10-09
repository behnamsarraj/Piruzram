using Microsoft.AspNetCore.Identity;

namespace Piruzram.Models
{
    public class ApplicationUser: IdentityUser
    {
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
