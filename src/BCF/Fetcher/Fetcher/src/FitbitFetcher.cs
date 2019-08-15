using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Fetcher
{
    public class FitbitFetcher : IFetcher
    {
        private const string FITBIT_API_HOST = "api.fitbit.com";
        private const char CURRENT_LOGGED_IN_USER = '-';
        private string GetNewAccessToken(string refreshToken)
        {
            var requestParams = "grant_type=refresh_token" +
                                $"&refresh_token={refreshToken}";
            var clientIdAndSecretInBytes = Encoding.ASCII.GetBytes($"{Secrets.Instance.FitbitApiClientId}:{Secrets.Instance.FitbitApiClientSecret}");

            var client = new RestClient($"https://{FITBIT_API_HOST}/oauth2/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(clientIdAndSecretInBytes));
            request.AddParameter("undefined", requestParams, ParameterType.RequestBody);

            var rawResponse = client.Execute(request);
            dynamic response = JObject.Parse(rawResponse.Content);

            return response.access_token;
        }

        private string GetUrlFromRequestedDataType(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.HeartRate:
                    return "activities/heart/date/today/1d.json";
                case DataType.Height:
                case DataType.Foo:
                case DataType.Bar:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }

        public bool TestFetch(string apiKey, DataType dataType, List<string> errors)
        {
            var accessToken = GetNewAccessToken(HttpUtility.UrlDecode(apiKey));
            var dataTypeUrl = GetUrlFromRequestedDataType(dataType);

            var client = new RestClient($"https://{FITBIT_API_HOST}/1/user/{CURRENT_LOGGED_IN_USER}/");
            var request = new RestRequest(dataTypeUrl, DataFormat.Json);
            request.AddHeader("Authorization", "Bearer " + accessToken);

            var rawResponse = client.Execute(request);
            if (!(rawResponse.IsSuccessful && 
                  string.IsNullOrWhiteSpace(rawResponse.ErrorMessage) &&
                  rawResponse.ErrorException == null))
                return false;

            dynamic response = JObject.Parse(rawResponse.Content);

            switch (dataType)
            {
                case DataType.HeartRate:
                    return ((JArray) response["activities-heart"]).Count > 0;
                case DataType.Height:
                case DataType.Foo:
                case DataType.Bar:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }
    }
}