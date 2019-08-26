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
            TempData["APIKey"] = APIKey;
            TempData["Wallet_ID"] = Wallet_ID;

            return View();
            // if(CheckPolicyForm())
            // {
            //     if(CheckPolicy().Result)
            //     {
            //         AddPolicy();
            //         return View();
            //     }
            //     return Error();
            // }
            // return BadRequest("Wallet ID and API Key are not filled");
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
            return View();
        }

        public IActionResult OAuthResult(string access_token, string scope, string token_type, string expires_in, string user_id)
        {
            TempData["access_token"] = access_token;
            TempData["scope"] = scope;
            TempData["token_type"] = token_type;
            TempData["expires_in"] = expires_in;
            TempData["user_id"] = user_id;
            return View();
        }

        public bool CheckPolicyForm()
        {
            //Check if Wallet_ID and APIKey are empty
            string Wallet_ID = Request.Form["Input.Wallet_ID"];
            string APIKey = Request.Form["Input.APIKey"];
            if(Wallet_ID != null && APIKey != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckPolicy()
        {
            //Check if policy is valid
            //checkjsonpart/{jsonpolicy}
            PolicyModel Policy = JsonConvert.DeserializeObject<PolicyModel>(((string)TempData["policy"]));
            Policy.Wallet_ID = Request.Form["Input.Wallet_ID"];

            var Client = new HttpClient();
            var Uri = Paths.Instance.ValidatorIP + ":" + Paths.Instance.ValidatorPort;
            var Response = await Client.GetAsync("http://" + Uri + "/checkjsonpart/" + Policy);

            return Response.IsSuccessStatusCode;
        }

        public async void AddPolicy()
        {
            var Parameters = new Dictionary<string, string>{
                {"json_policy", (string)TempData["policy"]}, 
                {"policy_creation_token", (string)TempData["policykey"]}, 
                {"wallet_id", Request.Form["Input.Wallet_ID"]},
                {"api_key", Request.Form["Input.APIKey"]}};
            var Content = new FormUrlEncodedContent(Parameters);
            var Client = new HttpClient();
            var Uri = Paths.Instance.PolicyGatewayIP + ":" + Paths.Instance.PolicyGatewayPort;
            var Response = await Client.PostAsync("http://" + Uri + "/addpolicy/", Content);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
