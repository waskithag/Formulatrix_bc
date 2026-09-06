namespace NetCoreApp.DTOs;

public class SalesDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public List<ProductDto> Products { get; set; } = [];
    public decimal TotalPrice { get; set; }
}

public class CreateSalesDto
{
    public int EmployeeId { get; set; }
    public List<int> ProductIds { get; set; } = [];
}

public class UpdateSalesDto
{
    public int EmployeeId { get; set; }
    public List<int> ProductIds { get; set; } = [];
}
