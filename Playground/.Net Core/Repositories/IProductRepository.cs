using NetCoreApp.Models;

namespace NetCoreApp.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByNameAsync(string name);
    Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids);
}
