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

			[JsonProperty("broker_wallet_id")]
			public string BrokerWalletID { get; set; }
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
			var jsonPolicyStr = CleanPolicy(requestBody.JsonPolicy);

			if(!ValidPolicy(jsonPolicyStr, requestBody.WalletID))
			{
				return BadRequest();
			}

			Console.WriteLine("Policy Valid");
			
			var stream = Utility.RandomString(MAX_STREAM_NAME_LENGTH);

			dynamic transID = DeployToMultichain(stream, jsonPolicyStr, requestBody.WalletID, requestBody.BrokerWalletID);

			var result = new ContentResult
			{
				Content = "{\"trans_id\":\"" + transID + "\", \"key\" : \"" + stream + "\"}"
			};

			return result;
		}

		private string CleanPolicy(string jsonPolicyStr)
		{
			return jsonPolicyStr.Replace(" ", "");
		}

		private bool ValidPolicy(string jsonPolicyStr, string walletID)
		{
			dynamic policyWalletID = JsonConvert.DeserializeObject(jsonPolicyStr);

			if (walletID != policyWalletID.wallet_ID.Value)
			{
				return false;
			}

			// Check policy valid
			var url = Paths.Instance.VaildatorPort == null ? Paths.Instance.VaildatorIP : Paths.Instance.VaildatorIP + ":" + Paths.Instance.VaildatorPort;
			var client = new RestClient("http://" + url + "/checkjson/" + jsonPolicyStr);
			IRestResponse response = client.Execute(new RestRequest(Method.GET));

			if(response.ErrorException != null)
			{
				throw new Exception("Failed to check policy valid: " + response.ErrorMessage);
			}

			return response.Content == jsonPolicyStr;
		}

		private IRestResponse ExcuteMultichainRPC(RestClient client, string body)
		{
			var request = new RestRequest(Method.POST);
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("undefined", body, ParameterType.RequestBody);
			var response = client.Execute(request);

			if(response.StatusCode != HttpStatusCode.OK)
			{
				Console.WriteLine(response.ErrorException);
			}

			return response;
		}

		private string DeployToMultichain(string stream, string jsonPolicyStr, string walletID, string brokerWalletID)
		{
			// Create Stream
			var filterName = Utility.RandomString(MAX_STREAM_NAME_LENGTH);
			var key = "policy";
			var chainName = Paths.Instance.ChainName;
			var ipPort = Paths.Instance.RPCIP + ":" + Paths.Instance.RPCPort;

			var client = new RestClient("http://" + ipPort)
			{
				Authenticator = new HttpBasicAuthenticator(Paths.Instance.RPCUserName, Paths.Instance.RPCPassword)
			};

			var response = ExcuteMultichainRPC(client, "{\"method\":\"create\",\"params\":[\"stream\",\"" + stream + "\",false],\"id\":\"44789892-1568698363\",\"chain_name\":\"" + chainName + "\"}");

			Console.WriteLine("Stream Created");

			// Deploy policy
			response = ExcuteMultichainRPC(client, "{\"method\":\"publish\",\"params\":[ \"" + stream + "\", \"" + key + "\", { \"json\":" + jsonPolicyStr + "}],\"chain_name\":\"" + chainName + "\"}");

			Console.WriteLine("Policy Deployed to Stream");

			// Create Smart filter 
			// Should put this on multiple lines but it's very touchy about the \\\" for reasons I can't and Don't want to understand Im sure im just a idiot but I can't figure it out and yes using @ then "" the double quotes didn't work
			var smartFilter = "function filterstreamitem() { var item=getfilterstreamitem(); if (item.publishers[0] != \\\"" + walletID + "\\\" && item.publishers[0] != \\\"" + brokerWalletID + "\\\") return \\\"Only data subject or data broker can modify policy\\\"; if (item.keys.length > 1 || item.keys.indexOf(\\\"policy\\\") != 0) return \\\"can only change the policy key\\\"; if (Object.keys(item.data.json).length != 1) return \\\"Can Only change the active field: \\\"; if(item.data.json.active != \\\"true\\\" && item.data.json.active != \\\"false\\\") return \\\"Can only set the active field to true or false\\\" }";

			response = ExcuteMultichainRPC(client,"{\"method\":\"create\",\"params\":[\"streamfilter\",\"" + filterName + "\",{},\"" + smartFilter + "\"],\"chain_name\":\"" + chainName + "\"}");

			Console.WriteLine("Smart Filter Created");

			// Apply Smart Filter 
			response = ExcuteMultichainRPC(client, "{\"method\":\"approvefrom\",\"params\":[\"" + Paths.Instance.AdminAddress + "\",\"" + filterName + "\",{\"for\":\"" + stream + "\",\"approve\":true}],\"chain_name\":\"" + chainName + "\"}");

			Console.WriteLine("Smart Filter Applied");

			dynamic responseContent = JsonConvert.DeserializeObject(response.Content);

			return responseContent.result;
		}
	}
}
