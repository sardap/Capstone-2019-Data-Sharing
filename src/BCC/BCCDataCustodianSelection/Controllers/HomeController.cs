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

namespace BCCDataCustodianSelection.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //https://localhost:5001/{policy}/policykey
        //policy must be a json policy that is URL encoded
        //policy key is a policy creation token

        [Route("{policy}/{policykey}")]
        public IActionResult Index(string policy, string policykey)
        {
            TempData["policy"] = policy;
            TempData["policykey"] = policykey;

            return View();
        }

        public IActionResult CustodianSelection()
        {
            string Wallet_ID = Request.Form["Input.Wallet_ID"];
            string APIKey = Request.Form["Input.APIKey"];
            string BrokerID = Request.Form["Input.BrokerID"];

            TempData["APIKey"] = APIKey;
            TempData["Wallet_ID"] = Wallet_ID;
            TempData["BrokerID"] = BrokerID;

            if (Wallet_ID == null || APIKey == null)
            {
                return BadRequest("Wallet ID, API Key and Broker ID are not filled");
            }
            return View();
        }

        public IActionResult DataTypeSelection()
        {
            //Data type selection based on custodian
            string DataCustodian = Request.Form["Input.DataCustodian"];
            TempData["DataCustodian"] = DataCustodian;
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

        public IActionResult Idle()
        {
            string DataType = Request.Form["Input.DataType"];
            TempData["DataType"] = DataType;

            if (!TempData.ContainsKey("policy")) throw new Exception("Policy not set. Cannot check or add policy");
            if (CheckPolicy().Result)
            {
                string custodian = (string)TempData["DataCustodian"];

                if (custodian == "GoogleFit")
                {
                    return Redirect("https://accounts.google.com/o/oauth2/v2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Ffitness.body.read%20https%3A%2F%2Fwww.googleapis.com%2Fauth%2Ffitness.activity.read&redirect_uri=https%3A%2F%2Fauthorization.secretwaterfall.club&response_type=token&client_id=446983905302-uuv9ap7s6poee19ksl4fkad4c5r9d0b3.apps.googleusercontent.com");
                }
                else if (custodian == "Fitbit")
                {
                    return Redirect("https://www.fitbit.com/oauth2/authorize?client_id=22B74V&response_type=token&scope=activity%20heartrate%20nutrition%20sleep%20weight&redirect_uri=https%3A%2F%2Fauthorization.secretwaterfall.club&expires_in=6000");
                }
                else throw new Exception("TempData not correctly passing.");
            }
            return Error();
        }

        public IActionResult OAuthResult(string access_token, string scope, string token_type, string expires_in, string user_id)
        {
            Console.WriteLine("Successful OAuth. Attemping to add policy using gateway");

            TempData["access_token"] = access_token;
            TempData["scope"] = scope;
            TempData["token_type"] = token_type;
            TempData["expires_in"] = expires_in;
            TempData["user_id"] = user_id;
            AddPolicy();
            return View();
        }

        public async Task<bool> CheckPolicy()
        {
            //Check if policy is valid
            //checkjsonpart/{jsonpolicy}
            //PolicyModel Policy = JsonConvert.DeserializeObject<PolicyModel>(((string)TempData["policy"]));
            //Policy.Wallet_ID = Request.Form["Input.Wallet_ID"];

            System.Console.WriteLine("Policy2: " + (string)TempData["policy"]);
            System.Console.WriteLine("PolicyKey2: " + (string)TempData["policyKey"]);


            var Client = new HttpClient();
            //Todo: figure out how to set these variables and exactly what they should be
            //var Uri = Paths.Instance.ValidatorIP + ":" + Paths.Instance.ValidatorPort;
            var validatorIp = Environment.GetEnvironmentVariable("ValidatorIP");
            var validatorPort = Environment.GetEnvironmentVariable("ValidatorPort");
            var Uri = validatorIp + ":" + validatorPort;
            var Response = await Client.GetAsync("http://" + Uri + "/checkjsonpart/" + (string)TempData["policy"]);

            return Response.IsSuccessStatusCode;
        }

        public async void AddPolicy()
        {
            /*
            PolicyModel Policy = JsonConvert.DeserializeObject<PolicyModel>(((string)TempData["policy"]));
            Policy.Wallet_ID = (string)TempData["Wallet_ID"];
            Policy.Data_type = (string)TempData["DataType"];
            string SerialPolicy = JsonConvert.SerializeObject(Policy);*/



            var Parameters = new Dictionary<string, string>{
                {"json_policy", (string)TempData["policy"]}, 
                {"policy_creation_token", (string)TempData["policykey"]}, 
                {"wallet_id", (string)TempData["Wallet_ID"]},
                {"api_key", (string)TempData["APIKey"]},
                {"api_key", (string)TempData["access_token"]},
                {"broker_id", Environment.GetEnvironmentVariable("BrokerID")}};
            var Content = new FormUrlEncodedContent(Parameters);

            var Client = new HttpClient();
            //var Uri = Paths.Instance.PolicyGatewayIP + ":" + Paths.Instance.PolicyGatewayPort;
            var policyGatewayIP = Environment.GetEnvironmentVariable("PolicyGatewayIP");
            var policyGatewayPort = Environment.GetEnvironmentVariable("PolicyGatewayPort");
            var Uri = policyGatewayIP + ":" + policyGatewayPort;

            System.Console.WriteLine("Policy Gateway: " + Uri);

            var Response = await Client.PostAsync("http://" + Uri + "/addpolicy/", Content);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
