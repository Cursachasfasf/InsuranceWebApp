using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{
    public class Payment
    {
        [Key]
        public int payment_id { get; set; }
        public int contract_id { get; set; }
        public DateTime payment_date { get; set; }
        public decimal amount { get; set; }
        public string payment_type { get; set; } = null!;
        public string status { get; set; } = null!;

        public Contract? Contract { get; set; }
    }
}