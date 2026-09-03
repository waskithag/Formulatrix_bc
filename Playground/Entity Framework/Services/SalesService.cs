using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services;

public class SalesService
{
    private readonly SalesDbContext _context;

    public SalesService(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<Sales> CreateSalesAsync(Sales sales)
    {
        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == sales.EmployeeId);
        if (!employeeExists)
            throw new ArgumentException($"Employee with ID {sales.EmployeeId} does not exist.");

        if (sales.Products != null && sales.Products.Count > 0)
        {
            var productIds = sales.Products.Select(p => p.Id).Distinct().ToList();
            var existingProducts = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (existingProducts.Count != productIds.Count)
            {
                var missingIds = productIds.Except(existingProducts.Select(p => p.Id));
                throw new ArgumentException($"Products with IDs [{string.Join(", ", missingIds)}] not found.");
            }

            sales.Products = existingProducts;
        }

        _context.Sales.Add(sales);
        await _context.SaveChangesAsync();

        return sales;
    }

    public async Task<Sales> CreateSalesAsync(int employeeId, List<int>? productIds = null)
    {
        var sales = new Sales
        {
            EmployeeId = employeeId,
            Products = productIds != null && productIds.Count > 0
                ? productIds.Select(id => new Product { Id = id }).ToList()
                : []
        };

        return await CreateSalesAsync(sales);
    }

    public async Task<List<Sales>> GetAllSalesAsync()
    {
        return await _context.Sales
            .Include(s => s.Products)
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    public async Task<Sales?> GetSalesByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Sales>> GetSalesByEmployeeIdAsync(int employeeId)
    {
        return await _context.Sales
            .Include(s => s.Products)
            .Where(s => s.EmployeeId == employeeId)
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    public async Task<Sales?> UpdateSalesAsync(int id, Sales updatedSales)
    {
        var existingSales = await _context.Sales
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (existingSales == null) return null;

        if (existingSales.EmployeeId != updatedSales.EmployeeId)
        {
            var employeeExists = await _context.Employees.AnyAsync(e => e.Id == updatedSales.EmployeeId);
            if (!employeeExists)
                throw new ArgumentException($"Employee with ID {updatedSales.EmployeeId} does not exist.");

            existingSales.EmployeeId = updatedSales.EmployeeId;
        }

        if (updatedSales.Products != null)
        {
            var productIds = updatedSales.Products.Select(p => p.Id).Distinct().ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != productIds.Count)
            {
                var missingIds = productIds.Except(products.Select(p => p.Id));
                throw new ArgumentException($"Products with IDs [{string.Join(", ", missingIds)}] not found.");
            }

            existingSales.Products.Clear();
            foreach (var prod in products)
            {
                existingSales.Products.Add(prod);
            }
        }

        await _context.SaveChangesAsync();
        return existingSales;
    }

    public async Task<bool> DeleteSalesAsync(int id)
    {
        var sales = await _context.Sales
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sales == null) return false;

        _context.Sales.Remove(sales);
        await _context.SaveChangesAsync();

        return true;
    }
}