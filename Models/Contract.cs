
using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{

    public class Contract
    {
        [Key]
        public int contract_id { get; set; }
      //  public string Number { get; set; } = null!;
        public int client_id { get; set; }
        public int product_id { get; set; }
        public int employee_id { get; set; }
        public decimal insurance_premium { get; set; }
        public decimal insurance_amount { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string status { get; set; } = null!;

        // навигационные свойства
        public Client? Client { get; set; }
        public InsuranceProduct? Product { get; set; }
        public Employee? Employee { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<InsuranceSituation> Situations { get; set; } = new List<InsuranceSituation>();
    }
}