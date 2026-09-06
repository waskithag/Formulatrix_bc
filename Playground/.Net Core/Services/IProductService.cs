using NetCoreApp.Common;
using NetCoreApp.DTOs;

namespace NetCoreApp.Services;

public interface IProductService
{
    Task<ServiceResult<List<ProductDto>>> GetAllAsync();
    Task<ServiceResult<ProductDto>> GetByIdAsync(int id);
    Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<ServiceResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
