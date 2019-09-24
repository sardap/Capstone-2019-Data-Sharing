using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BCCDataCustodianSelection.Models
{
    public class PolicyModel
    {
        [JsonProperty("excluded_categories")]
        public List<int> Excluded_categories {get; set;}

        [JsonProperty("min_price")]
        public int? Min_price {get; set;}

        [JsonProperty("time_period")]
        public Time Time_period {get; set;}

        [JsonProperty("data_type")]
        public string Data_type {get; set;}

        [JsonProperty("wallet_id")]
        public string Wallet_ID {get; set;}

        [JsonProperty("active")]
        public List<bool> Active {get; set;}

        [JsonProperty("report_log")]
        public List<Report> Report_log {get; set;}

        public class Report
        {
            [JsonProperty("data")]
            public string Data {get; set;}

            [JsonProperty("hash")]
            public string Hash {get; set;}
        }

        public class Time
        {
            [JsonProperty("start")]
            public long? Start {get; set;}

            [JsonProperty("end")]
            public long? End {get; set;}
        }
    }
}