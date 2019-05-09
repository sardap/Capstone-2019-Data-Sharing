using System;

namespace policy_validator.Models
{
    public class policyPartModel
    {
        public int[] excluded_categories {get; set;}
        public int? min_price {get; set;}
        public time time_period {get; set;}
        public string wallet_id {get; set;}
    }
}