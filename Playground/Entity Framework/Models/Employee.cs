using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
