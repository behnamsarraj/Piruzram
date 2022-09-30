using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Piruzram.Models;
using Piruzram.ViewModel;

namespace Piruzram.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<ProductCategory> ProductCategories { set; get; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { set; get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Piruzram.ViewModel.ProductCategoryViewModel> ProductCategoryViewModel { get; set; }
    }
}