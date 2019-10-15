using System;
using System.Collections.Generic;

namespace BCCDataCustodianSelection.Models
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

        public class Report
        {
            public string Data {get; set;}
            public string Hash {get; set;}
        }

        public class Time
        {
            public long? Start {get; set;}
            public long? End {get; set;}
        }
    }
}