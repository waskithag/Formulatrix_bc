using NetCoreApp.Models;

namespace NetCoreApp.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByNameAsync(string name);
}
