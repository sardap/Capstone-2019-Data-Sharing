using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BCCDataCustodianSelection.Models;
using RestSharp;

namespace BCCDataCustodianSelection.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //https://localhost:5001/{policy}/policykey
        [Route("{policy}/{policyToken}")]
        public IActionResult Index(string policy, string policyToken)
        {
            TempData["policy"] = policy;
            TempData["policyToken"] = policyToken;

            ViewData["CreateInfo"] = new PolicyCreation()
            {
                Policy = policy,
                PolicyToken = policyToken
            };
            
            return View();
        }

        public IActionResult CustodianSelection(string policy, string policyToken)
        {
            string walletID = Request.Form["Input.Wallet_ID"];

            TempData["Wallet_ID"] = walletID;

            if (walletID == null)
            {
                return BadRequest("Wallet ID, API Key and Broker ID are not filled");
            }

            ViewData["CreateInfo"] = new PolicyCreation()
            {
                Policy = policy,
                PolicyToken = policyToken,
                WalletID = walletID
            };

            return View();
        }

        public IActionResult DataTypeSelection(string policy, string policyToken, string walletID)
        {
            //Data type selection based on custodian
            string DataCustodian = Request.Form["Input.DataCustodian"];
            TempData["DataCustodian"] = DataCustodian;

            ViewData["CreateInfo"] = new PolicyCreation()
            {
                Policy = policy,
                PolicyToken = policyToken,
                WalletID = walletID
            };

            if (DataCustodian == "GoogleFit")
            {
                return View("GoogleTypeSelection");
            }
            else if (DataCustodian == "Fitbit")
            {
                return View("FitbitTypeSelection");
            }
            else if (DataCustodian == "")
            {
                return View("CustodianSelection");
            }
            return Error();
        }

        public IActionResult GoogleTypeSelection()
        {
            return View();
        }

        public IActionResult FitbitTypeSelection()
        {
            return View();
        }

        public IActionResult Idle(string policy, string policyToken, string walletID)
        {
            string dataType = Request.Form["Input.DataType"];
            TempData["DataType"] = dataType;

            //todo: fix PolicyCheck 
            //bool? policyResult = CheckPolicy().Result;
            //if (policyResult == null) Console.WriteLine("PolicyCheck is returning Null. //todo something about that");
            //if(policyResult != null)
            //{ 
            
            var encodedData = Base64Encode(JsonConvert.SerializeObject
            (
                new PolicyCreation() 
                {
                    Policy = policy,
                    PolicyToken = policyToken,
                    WalletID = walletID
                }
            ));
            var encodedRedirectUri = HttpUtility.HtmlEncode(Paths.Instance.RedirectURI);
            if((string)TempData["DataCustodian"] == "GoogleFit")
            {
                var scope = "https%3A%2F%2Fwww.googleapis.com%2Fauth%2Ffitness.body.read";

                return Redirect("https://accounts.google.com/o/oauth2/v2/auth?client_id=" + Paths.Instance.GoogleClientID + "&redirect_uri=" + encodedRedirectUri + "&scope=" + scope + "&state=" + encodedData + "&access_type=offline&response_type=code");

                //http://lvh.me/?state=&code=&scope=
            }
            else
            {
                return Redirect("https://www.fitbit.com/oauth2/authorize?client_id=22B74V&response_type=token&scope=activity%20heartrate%20nutrition%20sleep%20weight&redirect_uri=" + encodedRedirectUri + "&expires_in=6000");
            }
            //}
            //return Error();
        }

        public IActionResult OAuthResult(string code, string access_token, string scope, string token_type, string expires_in, string user_id, string state)
        {
            if(access_token == null)
            {
                access_token = WebUtility.UrlEncode(GetRefreshToken(code));
            }

            var createInfo = JsonConvert.DeserializeObject<PolicyCreation>(Base64Decode(state));

            createInfo.AccessToken = access_token;
            
            ViewData["CreateInfo"] = createInfo;

            ViewData["PolicyGatewayResponse"] = AddPolicy(createInfo.WalletID, "1", access_token, createInfo.Policy, createInfo.PolicyToken);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetRefreshToken(string access_token)
        {
            var client = new RestClient("https://www.googleapis.com/oauth2/v4/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "www.googleapis.com");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "code=" + access_token + "&client_id=" + Paths.Instance.GoogleClientID + "&client_secret=" + Paths.Instance.GoogleSecert + "&redirect_uri=" + Paths.Instance.RedirectURI + "&grant_type=authorization_code", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            Console.WriteLine("Refresh Token: " + response.Content);

            dynamic responseParsed = JsonConvert.DeserializeObject(response.Content);
            return responseParsed.refresh_token;
        }

        private string AddPolicy(string walletID, string dataType, string apiKey, string policyStr, string policyToken)
        {

            dynamic policy = JsonConvert.DeserializeObject(policyStr);
            policy.wallet_ID = walletID;
            policy.data_type = dataType;
            string jsonPolicy = JsonConvert.SerializeObject(policy);

            var url = "http://" + Paths.Instance.PolicyGatewayIP + "/addpolicy";
            Console.WriteLine(url);
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            var body = "{\"json_policy\":\"" + jsonPolicy.Replace("\"", "\\\"") + "\",\"policy_creation_token\":\"" + policyToken + "\",\"wallet_id\":\"" + walletID + "\",\"cust_type\":\"1\",\"data_type\":\"1\",\"api_key\":\"" + apiKey + "\"} ";
            Console.WriteLine("REQUEST STRING: " + body);

            ViewData["RequestBody"] = body;

            request.AddParameter("undefined", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            
            Console.WriteLine("Policy Gateway: " + response.Content);
            return response.Content;
        }
        
        public static string Base64Encode(string plainText) 
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData) 
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
