using AutoMapper;
using NetCoreApp.Common;
using NetCoreApp.DTOs;
using NetCoreApp.Models;
using NetCoreApp.Repositories;

namespace NetCoreApp.Services;

public class SalesService : ISalesService
{
    private readonly ISalesRepository _salesRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public SalesService(
        ISalesRepository salesRepository,
        IEmployeeRepository employeeRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _salesRepository = salesRepository;
        _employeeRepository = employeeRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<SalesDto>>> GetAllAsync()
    {
        var sales = await _salesRepository.GetAllWithDetailsAsync();
        var dtos = _mapper.Map<List<SalesDto>>(sales);
        return ServiceResult<List<SalesDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<SalesDto>> GetByIdAsync(int id)
    {
        var sale = await _salesRepository.GetByIdWithDetailsAsync(id);
        if (sale == null)
        {
            return ServiceResult<SalesDto>.NotFound($"Sales record with ID {id} not found.");
        }

        var dto = _mapper.Map<SalesDto>(sale);
        return ServiceResult<SalesDto>.Ok(dto);
    }

    public async Task<ServiceResult<List<SalesDto>>> GetByEmployeeIdAsync(int employeeId)
    {
        var sales = await _salesRepository.GetByEmployeeIdAsync(employeeId);
        var dtos = _mapper.Map<List<SalesDto>>(sales);
        return ServiceResult<List<SalesDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<SalesDto>> CreateAsync(CreateSalesDto dto)
    {
        var employeeExists = await _employeeRepository.ExistsAsync(dto.EmployeeId);
        if (!employeeExists)
        {
            return ServiceResult<SalesDto>.Fail($"Employee with ID {dto.EmployeeId} does not exist.", 400);
        }

        var products = new List<Product>();
        if (dto.ProductIds.Count > 0)
        {
            var distinctIds = dto.ProductIds.Distinct().ToList();
            products = await _productRepository.GetByIdsAsync(distinctIds);

            if (products.Count != distinctIds.Count)
            {
                var foundIds = products.Select(p => p.Id);
                var missingIds = distinctIds.Except(foundIds);
                return ServiceResult<SalesDto>.Fail($"Products with IDs [{string.Join(", ", missingIds)}] not found.", 400);
            }
        }

        var sale = new Sales
        {
            EmployeeId = dto.EmployeeId,
            Products = products
        };

        var created = await _salesRepository.AddAsync(sale);
        // Reload with details for mapping employee name
        var detailed = await _salesRepository.GetByIdWithDetailsAsync(created.Id);
        var resultDto = _mapper.Map<SalesDto>(detailed ?? created);

        return ServiceResult<SalesDto>.Created(resultDto, "Sales record created successfully.");
    }

    public async Task<ServiceResult<SalesDto>> UpdateAsync(int id, UpdateSalesDto dto)
    {
        var existing = await _salesRepository.GetByIdWithDetailsAsync(id);
        if (existing == null)
        {
            return ServiceResult<SalesDto>.NotFound($"Sales record with ID {id} not found.");
        }

        if (existing.EmployeeId != dto.EmployeeId)
        {
            var employeeExists = await _employeeRepository.ExistsAsync(dto.EmployeeId);
            if (!employeeExists)
            {
                return ServiceResult<SalesDto>.Fail($"Employee with ID {dto.EmployeeId} does not exist.", 400);
            }
            existing.EmployeeId = dto.EmployeeId;
        }

        if (dto.ProductIds != null)
        {
            var distinctIds = dto.ProductIds.Distinct().ToList();
            var products = await _productRepository.GetByIdsAsync(distinctIds);

            if (products.Count != distinctIds.Count)
            {
                var foundIds = products.Select(p => p.Id);
                var missingIds = distinctIds.Except(foundIds);
                return ServiceResult<SalesDto>.Fail($"Products with IDs [{string.Join(", ", missingIds)}] not found.", 400);
            }

            existing.Products.Clear();
            foreach (var product in products)
            {
                existing.Products.Add(product);
            }
        }

        await _salesRepository.UpdateAsync(existing);
        var updated = await _salesRepository.GetByIdWithDetailsAsync(id);
        var resultDto = _mapper.Map<SalesDto>(updated ?? existing);

        return ServiceResult<SalesDto>.Ok(resultDto, "Sales record updated successfully.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var existing = await _salesRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResult<bool>.NotFound($"Sales record with ID {id} not found.");
        }

        await _salesRepository.DeleteAsync(existing);
        return ServiceResult<bool>.Ok(true, "Sales record deleted successfully.");
    }
}
