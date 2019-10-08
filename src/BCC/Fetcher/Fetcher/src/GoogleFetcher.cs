using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using System.Web;

namespace Fetcher
{
	public class GoogleFetcher : IFetcher
	{
		private string GetNewAccessToken(string refreshToken)
		{
			string baseURL = "https://www.googleapis.com/oauth2/v4/token";
			string grantType = "refresh_token";

			string url =
				baseURL +
				"?client_id=" + Secrets.Instance.GoogleAPIClientID +
				"&client_secret=" + Secrets.Instance.GoogleAPIClientSecret +
				"&refresh_token=" + refreshToken +
				"&grant_type=" + grantType;

			var client = new RestClient(url);
			var request = new RestRequest(Method.POST);
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("Connection", "keep-alive");
			var response = client.Execute(request);

			dynamic responseJson = JObject.Parse(response.Content);

			return responseJson.access_token;
		}

		public bool TestFetch(string apiKey, DataType dataType, List<string> errors)
		{
			string accessToken =  GetNewAccessToken(HttpUtility.UrlDecode(apiKey));

			string dataSourceID;

			switch (dataType)
			{
				case DataType.Height:
					dataSourceID = "raw:com.google.height:com.google.android.apps.fitness:user_input";
					break;

				default:
					throw new NotImplementedException();
			}

			string url = "https://www.googleapis.com/fitness/v1/users/me/dataSources/" + dataSourceID + "/datasets/0-" + long.MaxValue;

			var client = new RestClient(url);
			var request = new RestRequest(Method.GET);
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("accept-encoding", "gzip, deflate");
			request.AddHeader("Host", "www.googleapis.com");
			request.AddHeader("Postman-Token", "13a04649-4eec-46f1-b961-a489aa5e6549,1c4d306a-795a-4ef4-a3a5-70835571a991");
			request.AddHeader("Cache-Control", "no-cache");
			request.AddHeader("Accept", "*/*");
			request.AddHeader("User-Agent", "PostmanRuntime/7.15.0");
			request.AddHeader("Authorization", "Bearer " + accessToken);
			var response = client.Execute(request);

			if (!(response.ErrorException == null && response.ErrorMessage == null && response.IsSuccessful))
				return false;

			dynamic responseJson = JObject.Parse(response.Content);

			bool result;

			switch (dataType)
			{
				case DataType.Height:
					result = responseJson.point.Count >= 1;
					break;

				default:
					throw new NotImplementedException();
			}

			return result;
		}
	}
}
