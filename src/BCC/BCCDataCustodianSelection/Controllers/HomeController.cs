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
            TempData.Keep();
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
            TempData.Keep();

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

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            foreach (Object obj in TempData)
            {
                Console.WriteLine(obj.ToString());
                TempData.Keep();
            }

            Console.WriteLine("Does TempData[datacustodian] equal GoogleFit? ");
            Console.WriteLine(TempData["DataCustodian"]);
            Console.WriteLine("GoogleFit");
            if ((string)TempData["DataCustodian"] == "GoogleFit") Console.WriteLine("... yeah");
            else Console.WriteLine("... nah");

            //todo: fix PolicyCheck 
            //bool? policyResult = CheckPolicy().Result;
            //if (policyResult == null) Console.WriteLine("PolicyCheck is returning Null. //todo something about that");
            //if(policyResult != null)
            //{
            //AddPolicy();
            //if ((string)TempData["DataCustodian"] == "GoogleFit")
            if (true)
            {
                return Redirect("https://accounts.google.com/o/oauth2/v2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Ffitness.body.read%20https%3A%2F%2Fwww.googleapis.com%2Fauth%2Ffitness.activity.read&redirect_uri=http%3A%2F%2Fauthorization.secretwaterfall.club&response_type=token&client_id=446983905302-uuv9ap7s6poee19ksl4fkad4c5r9d0b3.apps.googleusercontent.com");
            }
            else if ((string)TempData["DataCustodian"] == "FitBit")
            {
                return Redirect("https://www.fitbit.com/oauth2/authorize?client_id=22B74V&response_type=token&scope=activity%20heartrate%20nutrition%20sleep%20weight&redirect_uri=https%3A%2F%2Fauthorization.secretwaterfall.club&expires_in=6000");
            }
            else throw new Exception("TempData not correctly passing.");
            //}
            //return Error();
        }

        public IActionResult OAuthResult(string access_token, string scope, string token_type, string expires_in, string user_id)
        {
            TempData["access_token"] = access_token;
            TempData["scope"] = scope;
            TempData["token_type"] = token_type;
            TempData["expires_in"] = expires_in;
            TempData["user_id"] = user_id;
            TempData.Keep();
            return View();
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
            PolicyModel Policy = JsonConvert.DeserializeObject<PolicyModel>(((string)TempData["policy"]));
            Policy.Wallet_ID = (string)TempData["Wallet_ID"];
            Policy.Data_type = (string)TempData["DataType"];
            string SerialPolicy = JsonConvert.SerializeObject(Policy);

            var Parameters = new Dictionary<string, string>{
                {"json_policy", SerialPolicy}, 
                {"policy_creation_token", (string)TempData["policykey"]}, 
                {"wallet_id", (string)TempData["Wallet_ID"]},
                {"api_key", (string)TempData["APIKey"]}};
            TempData.Keep();
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
