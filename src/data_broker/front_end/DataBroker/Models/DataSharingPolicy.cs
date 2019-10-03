using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataBroker.Models
{
    public class DataSharingPolicy
    {
        [Key]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonProperty("excluded")]
        public string ExcludedBuyers { get; set; }
        [JsonProperty("start")]
        public DateTime Start { get; set; }
        [JsonProperty("end")]
        public DateTime End { get; set; }
        [JsonProperty("min_price")]
        public decimal MinPrice { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
