using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.DTOs;
using NetCoreApp.Services;

namespace NetCoreApp.Controllers;

public class SalesController : BaseApiController
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _salesService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _salesService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("employee/{employeeId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _salesService.GetByEmployeeIdAsync(employeeId);
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateSalesDto dto)
    {
        var result = await _salesService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSalesDto dto)
    {
        var result = await _salesService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _salesService.DeleteAsync(id);
        return HandleResult(result);
    }
}
