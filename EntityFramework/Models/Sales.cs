using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models
{
    public class Sales
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public List<Product> Products { get; set; } =  [];
    }
}