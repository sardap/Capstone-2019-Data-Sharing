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
        //https://localhost:5001/{policy}/key
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

            return View();//testing
            // if(CheckPolicyForm())
            // {
            //     if(CheckPolicy().Result)
            //     {
            //         AddPolicy();
            //         return View();
            //     }
            //     return Error();
            // }
            // return Error();
        }

        public IActionResult DataTypeSelection()
        {
            //Data custodian selection
            string DataCustodian = Request.Form["Input.DataCustodian"];

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

        public IActionResult GoogleOAuth()
        {
            //Google OAuth
            string DataType = Request.Form["Input.DataType"];
            string ResultAPIKey = "test";//placeholder



            TempData["resultapikey"] = ResultAPIKey;
            return View();
        }

        public IActionResult FitbitOAuth()
        {
            //Fitbit OAuth
            string DataType = Request.Form["Input.DataType"];
            string ResultAPIKey = "test";//placeholder



            TempData["resultapikey"] = ResultAPIKey;
            return View();
        }

        public bool CheckPolicyForm()
        {
            //Check if Wallet_ID and APIKey were empty
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
            var Response = await Client.GetAsync(Uri + "/checkjsonpart/" + Policy);

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
            var Response = await Client.PostAsync(Uri + "/addpolicy/", Content);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
