using System;
using System.Collections.Generic;

namespace Policy_Validator.Models
{
    public class PolicyModel
    {
        public List<int> Excluded_categories {get; set;}
        public int? Min_price {get; set;}
        public Time Time_period {get; set;}
        public string Data_type {get; set;}
        public string Wallet_ID {get; set;}
        public List<bool> Active {get; set;}
        public List<Report> Report_log {get; set;}
    }
}