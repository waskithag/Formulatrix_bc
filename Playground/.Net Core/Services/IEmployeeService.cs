using NetCoreApp.Common;
using NetCoreApp.DTOs;

namespace NetCoreApp.Services;

public interface IEmployeeService
{
    Task<ServiceResult<List<EmployeeDto>>> GetAllAsync();
    Task<ServiceResult<EmployeeDto>> GetByIdAsync(int id);
    Task<ServiceResult<EmployeeDto>> CreateAsync(CreateEmployeeDto dto);
    Task<ServiceResult<EmployeeDto>> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
