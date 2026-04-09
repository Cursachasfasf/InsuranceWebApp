using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{
    public class Employee
    {
        [Key]
        public int employee_id { get; set; }
        public string Name { get; set; } = null!;
        public string Step_Name { get; set; } = null!;
        public string? S_Step_Name { get; set; }
        public string phone { get; set; } = null!;
        public string? email { get; set; }
        public string position { get; set; } = null!;
       
    }
}