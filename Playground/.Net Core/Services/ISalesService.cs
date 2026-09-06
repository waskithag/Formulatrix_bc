using NetCoreApp.Common;
using NetCoreApp.DTOs;

namespace NetCoreApp.Services;

public interface ISalesService
{
    Task<ServiceResult<List<SalesDto>>> GetAllAsync();
    Task<ServiceResult<SalesDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<SalesDto>>> GetByEmployeeIdAsync(int employeeId);
    Task<ServiceResult<SalesDto>> CreateAsync(CreateSalesDto dto);
    Task<ServiceResult<SalesDto>> UpdateAsync(int id, UpdateSalesDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
