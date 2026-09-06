namespace NetCoreApp.Models;

public class Sales
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public List<Product> Products { get; set; } = [];
}
