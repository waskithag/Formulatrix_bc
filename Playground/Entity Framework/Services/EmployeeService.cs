using Entity_Framework.Data;
using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Services;

public class EmployeeService
{
    private readonly SalesDbContext _context;

    public EmployeeService(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Name))
            throw new ArgumentException("Employee name is required");
        
        _context.Employees.Add(employee);

        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<List<Employee>> GetAllEmployeeAsync()
    {
        return await _context.Employees
            .OrderBy(e => e.Id)
            .ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees.FindAsync(id);
    }

    public async Task<Employee?> GetEmployeeByNameAsync(string name)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Name == name);
    }

    public async Task<Employee?> UpdateEmployeeAsync(int id, Employee updatedEmployee)
    {
        var existingEmployee = await _context.Employees.FindAsync(id);

        if (existingEmployee == null) return null;

        existingEmployee.Name = updatedEmployee.Name;

        await _context.SaveChangesAsync();

        return existingEmployee;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return true;
    }
}
