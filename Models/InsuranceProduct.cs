using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{
    public class InsuranceProduct
    {
        [Key]
        public int product_id { get; set; }
        public string Name { get; set; } = null!;
        public string insurance_type { get; set; } = null!;
        public string description { get; set; } = null!;
        public decimal tariff_rate { get; set; }
        public int min_term_days { get; set; }
        public int max_term_days { get; set; }
    }
}