using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fetcher
{
	public class GoogleFetcher : IFetcher
	{
		public bool TestFetch(string apiKey, DataType dataType, List<string> errors)
		{
			string dataSourceID;

			switch (dataType)
			{
				case DataType.Height:
					dataSourceID = "raw:com.google.height:com.google.android.apps.fitness:user_input";
					break;

				default:
					throw new NotImplementedException();
			}

			var client = new RestClient("https://www.googleapis.com/fitness/v1/users/me/dataSources/" + dataSourceID + "/datasets/0-" + long.MaxValue);
			var request = new RestRequest(Method.GET);
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("accept-encoding", "gzip, deflate");
			request.AddHeader("Host", "www.googleapis.com");
			request.AddHeader("Postman-Token", "13a04649-4eec-46f1-b961-a489aa5e6549,1c4d306a-795a-4ef4-a3a5-70835571a991");
			request.AddHeader("Cache-Control", "no-cache");
			request.AddHeader("Accept", "*/*");
			request.AddHeader("User-Agent", "PostmanRuntime/7.15.0");
			request.AddHeader("Authorization", "Bearer " + apiKey);
			IRestResponse response = client.Execute(request);

			return response.ErrorException == null && response.ErrorMessage == null && response.IsSuccessful;
		}
	}
}
