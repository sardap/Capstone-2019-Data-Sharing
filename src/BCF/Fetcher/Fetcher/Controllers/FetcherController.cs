using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fetcher.Controllers
{
	[Route("fetcher")]
	[ApiController]
	public class FetcherController : ControllerBase
	{
		private readonly Dictionary<CustodianType, IFetcher> _fetchers = new Dictionary<CustodianType, IFetcher>
		{
			{ CustodianType.Fake, new FakeFetcher() },
			{ CustodianType.GoogleFit, new GoogleFetcher() },
			{ CustodianType.Fitbit, new FitbitFetcher() }
		};

		[HttpGet()]
		public ActionResult<string> Get()
		{
			return "Working";
		}

		[HttpGet("testfetch/{apiKey}/{custodianType}/{dataType}")]
		public ActionResult<string> Get(string apiKey, int custodianType, int dataType)
		{
			// help
			var fetchResult = new TestFetchResult
			{
				Result = null,
				Errors = new List<string>()
			};

			if (!Enum.IsDefined(typeof(CustodianType), custodianType))
				fetchResult.Errors.Add("custodian_type invalid");

			var custodian = (CustodianType)custodianType;

			if (!Enum.IsDefined(typeof(DataType), dataType))
				fetchResult.Errors.Add("data_type invalid");

			var dataTypeEnum = (DataType)dataType;

			if (fetchResult.Errors.Any())
			{
				Response.StatusCode = (int) HttpStatusCode.BadRequest;
				return JsonConvert.SerializeObject(fetchResult);
			}

			//Missing Fetcher implementation 
			if (!_fetchers.ContainsKey(custodian))
				return StatusCode((int) HttpStatusCode.InternalServerError);

			fetchResult.Result = _fetchers[custodian].TestFetch(apiKey, dataTypeEnum, fetchResult.Errors);
			return JsonConvert.SerializeObject(fetchResult);
		}
	}
}
