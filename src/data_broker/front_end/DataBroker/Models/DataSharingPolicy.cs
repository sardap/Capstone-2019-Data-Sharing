using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinPrice { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }
}
