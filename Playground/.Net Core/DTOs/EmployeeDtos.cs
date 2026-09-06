namespace NetCoreApp.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateEmployeeDto
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateEmployeeDto
{
    public string Name { get; set; } = string.Empty;
}
