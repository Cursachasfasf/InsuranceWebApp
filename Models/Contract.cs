
using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{

    public class Contract
    {
        [Key]
        public int contract_id { get; set; }
        [Required]
        public int client_id { get; set; }
        [Required]
        public int product_id { get; set; }
        [Required]
        public int employee_id { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal insurance_premium { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal insurance_amount { get; set; }
        [Required]
        public DateTime start_date { get; set; }
        [Required]
        public DateTime end_date { get; set; }
        [Required]
        [StringLength(50)]
        public string status { get; set; } = null!;

        // навигационные свойства
        public Client? Client { get; set; }
        public InsuranceProduct? Product { get; set; }
        public Employee? Employee { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<InsuranceSituation> Situations { get; set; } = new List<InsuranceSituation>();
    }
}