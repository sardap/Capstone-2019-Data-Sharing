﻿using System;
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
		private const int MAX_STREAM_KEY_LENGTH = 256;

		public class RequestBody
		{
			public string json_policy { get; set; }
			public string wallet_id { get; set; }
		}

		// POST api/values
		[HttpPost]
		public IActionResult Post(RequestBody requestBody)
		{
			var jsonPolicyStr = HttpUtility.UrlDecode(requestBody.json_policy).Replace(" ", "");
			var walletId = requestBody.wallet_id;

			// Check policy valid
			var url = Paths.Instance.VaildatorPort == null ? Paths.Instance.VaildatorIP : Paths.Instance.VaildatorIP + ":" + Paths.Instance.VaildatorPort;
			var client = new RestClient("http://" + url + "/checkjson/" + jsonPolicyStr);
			IRestResponse response = client.Execute(new RestRequest(Method.GET));

			if(response.Content != jsonPolicyStr)
			{
				return BadRequest();
			}

			var stream = Paths.Instance.StreamName;
			var key = Utility.RandomString(MAX_STREAM_KEY_LENGTH);
			var chainName = Paths.Instance.ChainName;

			client = new RestClient("http://localhost:25565")
			{
				Authenticator = new HttpBasicAuthenticator(Paths.Instance.RPCUserName, Paths.Instance.RPCPassword)
			};
			var request = new RestRequest(Method.POST);
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("Accept-Encoding", "gzip, deflate");
			request.AddHeader("Host", Paths.Instance.RPCIP + ":" + Paths.Instance.RPCPort);
			request.AddHeader("Cache-Control", "no-cache");
			request.AddHeader("Accept", "*/*");
			request.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("undefined", "{\"method\":\"publish\",\"params\":[ \"" + stream + "\", \"" + key + "\", { \"json\":" + jsonPolicyStr + "}],\"chain_name\":\"" + chainName + "\"}", ParameterType.RequestBody);
			response = client.Execute(request);

			dynamic reponseResult = JsonConvert.DeserializeObject(response.Content);

			//Error Handling for Bad submission? 

			var result = new ContentResult
			{
				Content = "{\"trans_id\":\"" + reponseResult.result + "\", \"key\" : \"" + key +"\"}"
			};

			return result;
		}
	}
}
