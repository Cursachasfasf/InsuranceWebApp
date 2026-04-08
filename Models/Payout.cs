using System;
using System.ComponentModel.DataAnnotations;

namespace InsuranceWebApp.Models
{
    public class Payout
    {
        [Key]
        public int payout_id { get; set; }
        public int situation_id { get; set; }
        public DateTime payout_date { get; set; }
        public decimal payout_amount { get; set; }
        public string payment_method { get; set; } = null!;

        public InsuranceSituation? Situation { get; set; }
    }
}