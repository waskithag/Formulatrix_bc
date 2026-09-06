using Microsoft.EntityFrameworkCore;
using NetCoreApp.Data;
using NetCoreApp.Models;

namespace NetCoreApp.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(SalesDbContext context) : base(context) { }

    public async Task<Employee?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower());
    }
}
