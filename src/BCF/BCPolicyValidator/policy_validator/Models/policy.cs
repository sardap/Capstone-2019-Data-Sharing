using System;

namespace Policy_Validator.Models
{
    public class PolicyModel
    {
        public int[] Excluded_categories {get; set;}
        public int? Min_price {get; set;}
        public Time Time_period {get; set;}
        public string Data_type {get; set;}
        public string Wallet_ID {get; set;}
        public bool[] Active {get; set;}
        public Report[] Report_log {get; set;}
    }
}