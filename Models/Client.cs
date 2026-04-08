
using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{

    public class Client
    {
        [Key]
        public int client_id { get; set; }
        public string Name { get; set; } = null!;
        public string Step_Name { get; set; } = null!;
        public string? S_Step_Name { get; set; }
        public string phone { get; set; } = null!;
        public string address { get; set; } = null!;
        public string? email { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}