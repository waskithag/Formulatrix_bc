using AutoMapper;
using NetCoreApp.Common;
using NetCoreApp.DTOs;
using NetCoreApp.Models;
using NetCoreApp.Repositories;

namespace NetCoreApp.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<ProductDto>>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        var dtos = _mapper.Map<List<ProductDto>>(products);
        return ServiceResult<List<ProductDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<ProductDto>> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return ServiceResult<ProductDto>.NotFound($"Product with ID {id} not found.");
        }

        var dto = _mapper.Map<ProductDto>(product);
        return ServiceResult<ProductDto>.Ok(dto);
    }

    public async Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return ServiceResult<ProductDto>.Fail("Product name is required.", 400);
        }

        if (dto.Price < 0)
        {
            return ServiceResult<ProductDto>.Fail("Product price cannot be negative.", 400);
        }

        if (dto.Stock < 0)
        {
            return ServiceResult<ProductDto>.Fail("Product stock cannot be negative.", 400);
        }

        var existingName = await _repository.GetByNameAsync(dto.Name);
        if (existingName != null)
        {
            return ServiceResult<ProductDto>.Fail($"A product with the name '{dto.Name}' already exists.", 409);
        }

        var product = _mapper.Map<Product>(dto);
        var created = await _repository.AddAsync(product);
        var resultDto = _mapper.Map<ProductDto>(created);

        return ServiceResult<ProductDto>.Created(resultDto, "Product created successfully.");
    }

    public async Task<ServiceResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return ServiceResult<ProductDto>.Fail("Product name is required.", 400);
        }

        if (dto.Price < 0)
        {
            return ServiceResult<ProductDto>.Fail("Product price cannot be negative.", 400);
        }

        if (dto.Stock < 0)
        {
            return ServiceResult<ProductDto>.Fail("Product stock cannot be negative.", 400);
        }

        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResult<ProductDto>.NotFound($"Product with ID {id} not found.");
        }

        var duplicate = await _repository.GetByNameAsync(dto.Name);
        if (duplicate != null && duplicate.Id != id)
        {
            return ServiceResult<ProductDto>.Fail($"A product with the name '{dto.Name}' already exists.", 409);
        }

        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing);
        var resultDto = _mapper.Map<ProductDto>(existing);

        return ServiceResult<ProductDto>.Ok(resultDto, "Product updated successfully.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResult<bool>.NotFound($"Product with ID {id} not found.");
        }

        await _repository.DeleteAsync(existing);
        return ServiceResult<bool>.Ok(true, "Product deleted successfully.");
    }
}
