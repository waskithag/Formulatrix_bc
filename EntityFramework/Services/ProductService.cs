using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services;

public class ProductService
{
    private readonly SalesDbContext _context;

    public ProductService(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required");

        if (product.Price < 0)
            throw new ArgumentException("Product price cannot be negative");

        if (product.Stock < 0)
            throw new ArgumentException("Product stock cannot be negative");

        var exists = await _context.Products.AnyAsync(p => p.Name == product.Name);
        if (exists)
            throw new InvalidOperationException($"Product with name '{product.Name}' already exists.");

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null) return null;

        if (string.IsNullOrWhiteSpace(updatedProduct.Name))
            throw new ArgumentException("Product name is required");

        if (updatedProduct.Price < 0)
            throw new ArgumentException("Product price cannot be negative");

        if (updatedProduct.Stock < 0)
            throw new ArgumentException("Product stock cannot be negative");

        if (updatedProduct.Name != existingProduct.Name)
        {
            var exists = await _context.Products.AnyAsync(p => p.Name == updatedProduct.Name && p.Id != id);
            if (exists)
                throw new InvalidOperationException($"Product with name '{updatedProduct.Name}' already exists.");

            existingProduct.Name = updatedProduct.Name;
        }

        existingProduct.Price = updatedProduct.Price;
        existingProduct.Stock = updatedProduct.Stock;

        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}