using Microsoft.EntityFrameworkCore;
using RPInventories.Models;

namespace RPInventories.Data
{
    public class InventoriesContext : DbContext
    {
        public InventoriesContext (DbContextOptions<InventoriesContext> options)
            : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().ToTable("Brand");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Product>()
                .ToTable("Product")
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }
    }
}
