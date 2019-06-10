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
		private Dictionary<CustodianType, IFetcher> _fetchers = new Dictionary<CustodianType, IFetcher>()
		{
			{ CustodianType.Fake, new FakeFetcher() }
		};

		[HttpGet()]
		public ActionResult<string> Get()
		{
			return "Working";
		}

		[HttpGet("testfetch/{apiKey}/{custodianType}/{dataType}")]
		public ActionResult<string> Get(string apiKey, int custodianType, int dataType)
		{
			var errors = new List<string>();

			if (!Enum.IsDefined(typeof(CustodianType), custodianType))
				errors.Add("custodian_type invaild");

			var custodian = (CustodianType)custodianType;

			if (!Enum.IsDefined(typeof(DataType), dataType))
				errors.Add("data_type invaild");

			var dataTypeEnum = (DataType)dataType;

			bool? result = null;

			if (errors.Count == 0 && _fetchers.ContainsKey(custodian))
			{
				result = _fetchers[custodian].TestFetch(apiKey, dataTypeEnum, errors);
			}
			else if(errors.Count == 0)
			{
				//Missing Fetcher implementation 
				return StatusCode(500);
			}

			if(errors.Count > 0)
				Response.StatusCode = 400;

			return JsonConvert.SerializeObject(new TestFetchResult() { Result = result, Errors = errors });
		}
	}
}
