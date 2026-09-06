using AutoMapper;
using NetCoreApp.DTOs;
using NetCoreApp.Models;

namespace NetCoreApp.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Employee mappings
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();

        // Product mappings
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Sales mappings
        CreateMap<Sales, SalesDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Name : string.Empty))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Products.Sum(p => p.Price)));
    }
}
