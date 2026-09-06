using NetCoreApp.Models;

namespace NetCoreApp.Repositories;

public interface ISalesRepository : IRepository<Sales>
{
    Task<List<Sales>> GetAllWithDetailsAsync();
    Task<Sales?> GetByIdWithDetailsAsync(int id);
    Task<List<Sales>> GetByEmployeeIdAsync(int employeeId);
}
