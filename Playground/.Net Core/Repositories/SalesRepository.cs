using Microsoft.EntityFrameworkCore;
using NetCoreApp.Data;
using NetCoreApp.Models;

namespace NetCoreApp.Repositories;

public class SalesRepository : Repository<Sales>, ISalesRepository
{
    public SalesRepository(SalesDbContext context) : base(context) { }

    public async Task<List<Sales>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(s => s.Employee)
            .Include(s => s.Products)
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    public async Task<Sales?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Employee)
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Sales>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _dbSet
            .Include(s => s.Employee)
            .Include(s => s.Products)
            .Where(s => s.EmployeeId == employeeId)
            .OrderBy(s => s.Id)
            .ToListAsync();
    }
}
