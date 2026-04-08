using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{
    public class InsuranceSituation
    {
        [Key]
        public int situation_id { get; set; }
        public int contract_id { get; set; }
        public int employee_id { get; set; }
        public string status { get; set; } = null!;
        public decimal damage_amount { get; set; }
        public string description { get; set; } = null!;
        public DateTime incident_date { get; set; }
        public DateTime claim_date { get; set; }

        public Contract? Contract { get; set; }
        public Employee? Employee { get; set; }
        public virtual ICollection<Payout> Payouts { get; set; } = new List<Payout>();
    }
}