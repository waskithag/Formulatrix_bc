using Microsoft.EntityFrameworkCore;
using NetCoreApp.Models;

namespace NetCoreApp.Data;

public class SalesDbContext : DbContext
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sales> Sales => Set<Sales>();

    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.HasIndex(p => p.Name).IsUnique();
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Sales>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(s => s.Products)
                .WithMany();
        });
    }

    public void SeedInitialData()
    {
        if (!Employees.Any())
        {
            Employees.AddRange(
                new Employee { Id = 1, Name = "John Doe" },
                new Employee { Id = 2, Name = "James" }
            );
        }

        if (!Products.Any())
        {
            Products.AddRange(
                new Product { Id = 1, Name = "Sabun", Price = 10000m, Stock = 100 },
                new Product { Id = 2, Name = "Indomie", Price = 3000m, Stock = 100 }
            );
        }

        SaveChanges();
    }
}
