using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace BlockchainPolicyDeployer.Controllers
{
	[Route("blockchain_policy_deployer")]
	[ApiController]
	public class DeployerController : ControllerBase
	{
		public class RequestBody
		{
			public string json_policy { get; set; }
			public string wallet_id { get; set; }
		}

		// POST api/values
		[HttpPost]
		public HttpResponseMessage Post(RequestBody requestBody)
		{
			var jsonPolicyStr = HttpUtility.UrlDecode(requestBody.json_policy);
			var walletId = requestBody.wallet_id;

			// Check policy valid
			var url = Paths.Instance.VaildatorPort == null ? Paths.Instance.VaildatorIP : Paths.Instance.VaildatorIP + ":" + Paths.Instance.VaildatorPort;
			var client = new RestClient("http://" + url + "/checkjson/" + jsonPolicyStr);
			IRestResponse response = client.Execute(new RestRequest(Method.GET));

			if(response.Content != jsonPolicyStr.Replace(" ", ""))
			{
				var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
				return result;
			}



			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
