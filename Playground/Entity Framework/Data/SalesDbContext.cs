using System.Data.Common;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Data;

public class SalesDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Employee> Employess { get => Employees; set => Employees = value; }
    public DbSet<Sales> Sales { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Product> Product { get => Products; set => Products = value; }

    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

    public SalesDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=SalesDatabase.db");
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {

        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(p => p.Name).IsUnique();
        });

        modelBuilder.Entity<Sales>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(s => s.EmployeeId);
            entity.HasMany(s => s.Products)
                .WithMany();
        });
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Name = "John Doe" },
            new Employee { Id = 2, Name = "James" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Sabun", Price = 10000, Stock = 100 },
            new Product { Id = 2, Name = "Indomie", Price = 3000, Stock = 100 }
        );
    }

    public static void SeedData(SalesDbContext context)
    {
        context.SeedData();
    }

    public void SeedData()
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
                new Product { Id = 1, Name = "Sabun", Price = 10000, Stock = 100 },
                new Product { Id = 2, Name = "Indomie", Price = 3000, Stock = 100 }
            );
        }

        SaveChanges();
    }
}
