using System;
using System.Linq;
using DataBroker.Data;
using DataBroker.Models;
using DataBroker.Models.Transport;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

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

		[Route("/Policies")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[HttpGet]
		[Route("/Policy/Authorization")]
		public IActionResult Authorization(string url)
		{
			ViewBag.RedirectUrl = url;
			return View();
		}

		[HttpGet("/api/GetAllPolicies")]
		public IActionResult GetAll()
		{
			var user = _context.ApplicationUsers.SingleOrDefault(z => z.Email.Equals(User.Identity.Name));
			if (user == null) return Json(new {success = false, message = "Invalid user"});

			var policies = _context.DataSharingPolicies.AsEnumerable().Where(z => z.UserId == user.Id).ToArray();
			return Json(new {success = true, data = policies});
		}

		private bool IsValid(JObject paramPolicy)
		{
			var client = new RestClient("http://136.186.108.17:30552/checkjson/" +
			                            paramPolicy.ToString().Replace(Environment.NewLine, "").Replace(" ", ""));
			var request = new RestRequest(Method.GET);
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("Accept-Encoding", "gzip, deflate");
			request.AddHeader("Host", "136.186.108.17:30552");
			request.AddHeader("Cache-Control", "no-cache");
			request.AddHeader("Accept", "*/*");
			var rawResponse = client.Execute(request);
			if (rawResponse.Content.Contains("Wrong")) return false;
			dynamic response = JsonConvert.DeserializeObject(rawResponse.Content);
			return !response.GetType().ToString().Contains("Array");
		}

		private string RequestCreationToken()
		{
			var client = new RestClient("http://136.186.108.52:30164/bcc_policy_token_gateway/newtoken/broker0");
			var request = new RestRequest(Method.GET);
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("Accept-Encoding", "gzip, deflate");
			request.AddHeader("Host", "136.186.108.52:30164");
			request.AddHeader("Cache-Control", "no-cache");
			request.AddHeader("Accept", "*/*");
			var rawResponse = client.Execute(request);
			dynamic response = JsonConvert.DeserializeObject(rawResponse.Content);
			return response.status == "success" ? response.policy_creation_token : string.Empty;
		}

		[HttpPost("/api/ValidatePolicy")]
		public IActionResult Validate(DspPoco policy)
		{
			var user = _context.ApplicationUsers.SingleOrDefault(z => z.Email.Equals(User.Identity.Name));
			if (user == null) return Json(new {success = false, message = "Invalid user"});

			var json = policy.ToJObject();
			if (!IsValid(json))
			{
				return Json(new {success = false, message = "Invalid policy, please check your policy again!"});
			}

			var token = RequestCreationToken();
			if (string.IsNullOrWhiteSpace(token))
			{
				return Json(new {success = false, message = "Invalid policy, please check your policy again!"});
			}

			var newPolicy = new DataSharingPolicy
			{
				UserId = user.Id,
				ExcludedBuyers = policy.ExcludedBuyers,
				Start = DateTime.Parse(policy.Start),
				End = DateTime.Parse(policy.End),
				MinPrice = policy.MinPrice,
				Active = policy.Active,
				Verified = false
			};
			_context.DataSharingPolicies.Add(newPolicy);
			_context.SaveChanges();

			var url = "https://authorization.secretwaterfall.club/" +
			          json.ToString().Replace(Environment.NewLine, "").Replace(" ", "") + "/" + token;
			return Json(new {success = true, message = url});
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

			return Json(new {success = true, message = "Successfully updated policy!"});
		}
	}
}