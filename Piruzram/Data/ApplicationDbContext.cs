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
        public virtual DbSet<Inventory> Inventories { set; get; }
        public virtual DbSet<Cart> Carts { set; get; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>();
            /*
            builder.Entity<Inventory>()
            .HasOne(m => m.Product)
            .WithMany(m => m.Inventories).IsRequired()
            .HasForeignKey(m => m.Count)
            .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Inventory>()
            .HasOne(m => m.Cart)
            .WithMany(m => m.Inventories).IsRequired()
            .HasForeignKey(m => m.Count)
            .OnDelete(DeleteBehavior.NoAction);
            */

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}