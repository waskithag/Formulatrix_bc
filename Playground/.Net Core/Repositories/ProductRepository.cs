using Microsoft.EntityFrameworkCore;
using NetCoreApp.Data;
using NetCoreApp.Models;

namespace NetCoreApp.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(SalesDbContext context) : base(context) { }

    public async Task<Product?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
    }

    public async Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();
        return await _dbSet.Where(p => idList.Contains(p.Id)).ToListAsync();
    }
}
