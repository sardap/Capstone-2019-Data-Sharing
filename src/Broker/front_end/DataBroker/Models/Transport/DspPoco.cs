using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataBroker.Models.Transport
{
	public class DspPoco
	{
		[JsonProperty("excluded")] public string ExcludedBuyers { get; set; }
		[JsonProperty("start")] public string Start { get; set; }
		[JsonProperty("end")] public string End { get; set; }
		[JsonProperty("min_price")] public decimal MinPrice { get; set; }
		[JsonIgnore] private int MinPriceInCents => (int) MinPrice * 100;
		[JsonProperty("active")] public bool Active { get; set; }
		[JsonProperty("verified")] public bool Verified { get; set; }
		[JsonProperty("id")] public string Id { get; set; }

		public JObject ToJObject()
		{
			dynamic paramPolicy = new JObject();
			paramPolicy.excluded_categories = new JArray(ExcludedBuyers.Split(",").Select((s, i) => i));
			paramPolicy.min_price = MinPriceInCents;
			paramPolicy.time_period = new JObject();
			paramPolicy.time_period.start = DateTime.Parse(Start).Ticks;
			paramPolicy.time_period.end = DateTime.Parse(End).Ticks;
			paramPolicy.active = new JArray(new[] {Active});
			paramPolicy.wallet_id = new Guid().ToString();
			paramPolicy.data_type = "test data";
			dynamic reportLogElement = new JObject();
			reportLogElement.data = "none";
			reportLogElement.hash = "none";
			paramPolicy.report_log = new JArray(new[] {reportLogElement});
			return paramPolicy;
		}
	}
}