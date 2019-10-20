using System;
using System.Linq;
using DataBroker.Data;
using DataBroker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataBroker.Controllers
{
	[ApiController]
	public class PolicyController : Controller
	{
		private readonly ApplicationDbContext _context;

		public PolicyController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("/api/GetAllPolicies")]
		public IActionResult GetAll()
		{
			var user = _context.ApplicationUsers.SingleOrDefault(z => z.Email.Equals(User.Identity.Name));
			if (user == null) return Json(new {success = false, message = "Invalid user"});

			var policies = _context.DataSharingPolicies.AsEnumerable().Where(z => z.UserId == user.Id).ToArray();
			return Json(new {success = true, data = policies});
		}

		public class DspPoco
		{
			[JsonProperty("excluded")] public string ExcludedBuyers { get; set; }
			[JsonProperty("start")] public string Start { get; set; }
			[JsonProperty("end")] public string End { get; set; }
			[JsonProperty("min_price")] public decimal MinPrice { get; set; }
			[JsonProperty("active")] public bool Active { get; set; }
			[JsonProperty("id")] public string Id { get; set; }
		}

		[HttpPost("/api/AddPolicy")]
		public IActionResult Add(DspPoco policy)
		{
			var user = _context.ApplicationUsers.SingleOrDefault(z => z.Email.Equals(User.Identity.Name));
			if (user == null) return Json(new {success = false, message = "Invalid user"});

			// TODO: Link to Data Custodian

			var newPolicy = new DataSharingPolicy
			{
				UserId = user.Id,
				ExcludedBuyers = policy.ExcludedBuyers,
				Start = DateTime.Parse(policy.Start),
				End = DateTime.Parse(policy.End),
				MinPrice = policy.MinPrice,
				Active = policy.Active
			};
			_context.DataSharingPolicies.Add(newPolicy);
			_context.SaveChanges();

			// TODO: Send request to policy drop off point

			return Json(new {success = true, message = "Successfully added new policy!"});
		}

		[HttpPost("/api/UpdatePolicy")]
		public IActionResult Update(DspPoco policy)
		{
			var user = _context.ApplicationUsers.SingleOrDefault(z => z.Email.Equals(User.Identity.Name));
			if (user == null) return Json(new {success = false, message = "Invalid user"});

			var existingPolicy = _context.DataSharingPolicies.SingleOrDefault(p => p.Id.ToString().Equals(policy.Id));

			if (existingPolicy == null) return Json(new {success = false, message = "Can't update policy"});

			existingPolicy.ExcludedBuyers = policy.ExcludedBuyers;
			existingPolicy.Start = DateTime.Parse(policy.Start);
			existingPolicy.End = DateTime.Parse(policy.End);
			existingPolicy.MinPrice = policy.MinPrice;

			_context.DataSharingPolicies.Update(existingPolicy);
			_context.SaveChanges();

			// TODO: Send request to policy drop off point

			return Json(new {success = true, message = "Successfully updated policy!"});
		}

		[HttpPost("/api/RemovePolicy")]
		public IActionResult Remove(DspPoco policy)
		{
			var user = _context.ApplicationUsers.SingleOrDefault(z => z.Email.Equals(User.Identity.Name));
			if (user == null) return Json(new {success = false, message = "Invalid user"});

			var existingPolicy = _context.DataSharingPolicies.SingleOrDefault(p => p.Id.ToString().Equals(policy.Id));

			if (existingPolicy == null) return Json(new {success = false, message = "Can't remove policy"});

			_context.DataSharingPolicies.Remove(existingPolicy);
			_context.SaveChanges();

			// TODO: Send request to policy drop off point

			return Json(new {success = true, message = "Successfully updated policy!"});
		}

		[HttpPost("/api/ActivatePolicy")]
		public IActionResult ActivatePolicy(DspPoco policy)
		{
			var user = _context.ApplicationUsers.SingleOrDefault(z => z.Email.Equals(User.Identity.Name));
			if (user == null) return Json(new {success = false, message = "Invalid user"});

			var existingPolicy = _context.DataSharingPolicies.SingleOrDefault(p => p.Id.ToString().Equals(policy.Id));

			if (existingPolicy == null) return Json(new {success = false, message = "Can't update policy"});

			existingPolicy.Active = true;

			_context.DataSharingPolicies.Update(existingPolicy);
			_context.SaveChanges();

			// TODO: Send request to policy drop off point

			return Json(new {success = true, message = "Successfully updated policy!"});
		}

		[HttpPost("/api/DeactivatePolicy")]
		public IActionResult DeactivatePolicy(DspPoco policy)
		{
			var user = _context.ApplicationUsers.SingleOrDefault(z => z.Email.Equals(User.Identity.Name));
			if (user == null) return Json(new {success = false, message = "Invalid user"});

			var existingPolicy = _context.DataSharingPolicies.SingleOrDefault(p => p.Id.ToString().Equals(policy.Id));

			if (existingPolicy == null) return Json(new {success = false, message = "Can't update policy"});

			existingPolicy.Active = false;

			_context.DataSharingPolicies.Update(existingPolicy);
			_context.SaveChanges();

			// TODO: Send request to policy drop off point

			return Json(new {success = true, message = "Successfully updated policy!"});
		}
	}
}