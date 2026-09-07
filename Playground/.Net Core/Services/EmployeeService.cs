using AutoMapper;
using NetCoreApp.Common;
using NetCoreApp.DTOs;
using NetCoreApp.Models;
using NetCoreApp.Repositories;

namespace NetCoreApp.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<EmployeeDto>>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();
        var dtos = _mapper.Map<List<EmployeeDto>>(employees);
        return ServiceResult<List<EmployeeDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<EmployeeDto>> GetByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee == null)
        {
            return ServiceResult<EmployeeDto>.NotFound($"Employee with ID {id} not found.");
        }

        var dto = _mapper.Map<EmployeeDto>(employee);
        return ServiceResult<EmployeeDto>.Ok(dto);
    }

    public async Task<ServiceResult<EmployeeDto>> CreateAsync(CreateEmployeeDto dto)
    {
        var employee = _mapper.Map<Employee>(dto);
        var created = await _repository.AddAsync(employee);
        var resultDto = _mapper.Map<EmployeeDto>(created);

        return ServiceResult<EmployeeDto>.Created(resultDto, "Employee created successfully.");
    }

    public async Task<ServiceResult<EmployeeDto>> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResult<EmployeeDto>.NotFound($"Employee with ID {id} not found.");
        }

        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing);
        var resultDto = _mapper.Map<EmployeeDto>(existing);

        return ServiceResult<EmployeeDto>.Ok(resultDto, "Employee updated successfully.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResult<bool>.NotFound($"Employee with ID {id} not found.");
        }

        await _repository.DeleteAsync(existing);
        return ServiceResult<bool>.Ok(true, "Employee deleted successfully.");
    }
}
