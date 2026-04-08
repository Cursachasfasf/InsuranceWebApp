using System;

namespace InsuranceWebApp.Models
{
    public class ActiveContractView
    {
        public int contract_id { get; set; }
        public string ClientName { get; set; } = null!;
        public string ClientPhone { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string insurance_type { get; set; } = null!;
        public decimal insurance_premium { get; set; }
        public decimal insurance_amount { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string EmployeeName { get; set; } = null!;
    }
}