using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace BlockchainPolicyDeployer.Controllers
{
	[Route("blockchain_policy_deployer")]
	[ApiController]
	public class DeployerController : ControllerBase
	{
		private const int MAX_STREAM_NAME_LENGTH = 32;

		public class RequestBody
		{
			[JsonProperty("json_policy")]
			public string JsonPolicy { get; set; }
			
			[JsonProperty("wallet_id")]
			public string WalletID { get; set; }
		}

		[HttpGet("working")]
		public string Working()
		{
			return "Working";
		}

		// POST api/values
		[HttpPost("deploy")]
		public IActionResult Post(RequestBody requestBody)
		{
			var jsonPolicyStr = requestBody.JsonPolicy.Replace(" ", "");
			var walletId = requestBody.WalletID;

			dynamic policyWalletID = JsonConvert.DeserializeObject(jsonPolicyStr);

			if (walletId != policyWalletID.wallet_ID.Value)
			{
				return BadRequest();
			}

			// Check policy valid
			var url = Paths.Instance.VaildatorPort == null ? Paths.Instance.VaildatorIP : Paths.Instance.VaildatorIP + ":" + Paths.Instance.VaildatorPort;
			var client = new RestClient("http://" + url + "/checkjson/" + jsonPolicyStr);
			IRestResponse response = client.Execute(new RestRequest(Method.GET));

			if(response.ErrorException != null)
			{
				return StatusCode(500, "Failed to check policy valid: " + response.ErrorMessage);
			}

			if(response.Content != jsonPolicyStr)
			{
				return BadRequest();
			}

			// Create Stream
			var stream = Utility.RandomString(MAX_STREAM_NAME_LENGTH);
			var filterName = Utility.RandomString(MAX_STREAM_NAME_LENGTH);
			var key = "policy";
			var chainName = Paths.Instance.ChainName;
			var ipPort = Paths.Instance.RPCIP + ":" + Paths.Instance.RPCPort;

			client = new RestClient("http://" + ipPort)
			{
				Authenticator = new HttpBasicAuthenticator(Paths.Instance.RPCUserName, Paths.Instance.RPCPassword)
			};
			var request = new RestRequest(Method.POST);
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("Accept-Encoding", "gzip, deflate");
			request.AddHeader("Host", ipPort);
			request.AddHeader("Accept", "*/*");
			request.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("undefined", "{\"method\":\"create\",\"params\":[\"stream\",\"" + stream + "\",false],\"id\":\"44789892-1568698363\",\"chain_name\":\"" + chainName + "\"}", ParameterType.RequestBody);
			response = client.Execute(request);

			if(response.ErrorException != null)
			{
				return StatusCode(500, response.ErrorException);
			}

			// Deploy policy
			client = new RestClient("http://" + ipPort)
			{
				Authenticator = new HttpBasicAuthenticator(Paths.Instance.RPCUserName, Paths.Instance.RPCPassword)
			};
			request = new RestRequest(Method.POST);
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("Accept-Encoding", "gzip, deflate");
			request.AddHeader("Host", ipPort);
			request.AddHeader("Accept", "*/*");
			request.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("undefined", "{\"method\":\"publish\",\"params\":[ \"" + stream + "\", \"" + key + "\", { \"json\":" + jsonPolicyStr + "}],\"chain_name\":\"" + chainName + "\"}", ParameterType.RequestBody);
			response = client.Execute(request);

			if(response.ErrorException != null)
			{
				return StatusCode(500, response.ErrorException);
			}

			// Create Smart filter 
			client = new RestClient("http://" + ipPort)
			{
				Authenticator = new HttpBasicAuthenticator(Paths.Instance.RPCUserName, Paths.Instance.RPCPassword)
			};
			request = new RestRequest(Method.POST);
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("Accept-Encoding", "gzip, deflate");
			request.AddHeader("Host", ipPort);
			request.AddHeader("Accept", "*/*");
			request.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("undefined", "{\"method\":\"create\",\"params\":[\"streamfilter\",\"" + filterName + "\",{},\"function filterstreamitem() { var item=getfilterstreamitem(); if (item.publishers[0] != \\\"" + policyWalletID.wallet_ID.Value + "\\\") return \\\"Only data subject can modify policy\\\"; }\"],\"chain_name\":\"" + chainName + "\"}", ParameterType.RequestBody);

			//request.AddParameter("undefined", "{\"method\":\"create\",\"params\":[\"streamfilter\",\"" + filterName + "\",{},\"function filterstreamitem() { var item=getfilterstreamitem(); if (item.keys.length<2) return \\\"At least two keys required\\\"; }\"],\"id\":\"68970363-1568698856\",\"chain_name\":\"" + chainName + "\"}", ParameterType.RequestBody);
			response = client.Execute(request);

			if(response.StatusCode != HttpStatusCode.OK)
			{
				return StatusCode(500, response.Content);
			}

			// Apply Smart Filter 
			client = new RestClient("http://" + ipPort)
			{
				Authenticator = new HttpBasicAuthenticator(Paths.Instance.RPCUserName, Paths.Instance.RPCPassword)
			};
			request = new RestRequest(Method.POST);
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("Accept-Encoding", "gzip, deflate");
			request.AddHeader("Host", ipPort);
			request.AddHeader("Accept", "*/*");
			request.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("undefined", "{\"method\":\"approvefrom\",\"params\":[\"" + Paths.Instance.AdminAddress + "\",\"" + filterName + "\",{\"for\":\"" + stream + "\",\"approve\":true}],\"chain_name\":\"" + chainName + "\"}", ParameterType.RequestBody);
			response = client.Execute(request);

			if(response.ErrorException != null)
			{
				return StatusCode(500, response.ErrorException);
			}

			dynamic reponseResult = JsonConvert.DeserializeObject(response.Content);

			//Error Handling for Bad submission? 

			var result = new ContentResult
			{
				Content = "{\"trans_id\":\"" + reponseResult.result + "\", \"key\" : \"" + stream + "\"}"
			};

			return result;
		}
	}
}
