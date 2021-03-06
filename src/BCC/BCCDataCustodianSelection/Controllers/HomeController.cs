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
using MySql.Data;
using MySql.Data.MySqlClient;

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
            if(!CheckPolicyToken(policyToken))
            {
                return Error();
            }

            ViewData["CreateInfo"] = new PolicyCreation()
            {
                Policy = policy,
                PolicyToken = policyToken
            };
            
            return View();
        }

        public IActionResult CustodianSelection(string policy, string policyToken)
        {
            if(!CheckPolicyToken(policyToken))
            {
                return Error();
            }

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
            if(!CheckPolicyToken(policyToken))
            {
                return Error();
            }

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

            if(!CheckPolicyToken(policyToken))
            {
                return Error();
            }

            string scope = Request.Form["Input.DataType"];

            var googleFit = (string)TempData["DataCustodian"] == "GoogleFit";
            // We need to Get the custType from the fetcher But I just don't have time
            var custType = googleFit ? "1" : "2";
            
            var encodedData = Base64Encode(JsonConvert.SerializeObject
            (
                new PolicyCreation() 
                {
                    Policy = policy,
                    PolicyToken = policyToken,
                    WalletID = walletID,
                    DataType = ScopeToDataType(custType, scope),
                    CustType = custType
                }
            ));
            var encodedRedirectUri = HttpUtility.HtmlEncode(Paths.Instance.RedirectURI);
            if(custType == "1")
            {
                var fullScope = "https://www.googleapis.com/auth/" + scope;

                return Redirect("https://accounts.google.com/o/oauth2/v2/auth?client_id=" + Paths.Instance.GoogleClientID + "&redirect_uri=" + encodedRedirectUri + "&scope=" + fullScope + "&state=" + encodedData + "&access_type=offline&response_type=code");
            }
            else
            {
                return Redirect("https://www.fitbit.com/oauth2/authorize?client_id=22B74V&response_type=token&scope=activity%20" +  scope + "%20nutrition%20sleep%20weight&redirect_uri=" + encodedRedirectUri + "&expires_in=6000");
            }
        }

        public IActionResult OAuthResult(string code, string access_token, string scope, string token_type, string expires_in, string user_id, string state)
        {
            var createInfo = JsonConvert.DeserializeObject<PolicyCreation>(Base64Decode(state));

            if(access_token == null)
            {
                access_token = WebUtility.UrlEncode(GetRefreshToken(code, createInfo.WalletID, scope));
            }

            if(!CheckPolicyToken(createInfo.PolicyToken))
            {
                return Error();
            }

            createInfo.AccessToken = access_token;
            
            ViewData["CreateInfo"] = createInfo;

            ViewData["PolicyGatewayResponse"] = AddPolicy
            (
                createInfo.WalletID, 
                createInfo.DataType, 
                access_token, 
                createInfo.Policy, 
                createInfo.PolicyToken,
                createInfo.CustType
            );

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool CheckPolicyToken(string policyToken)
        {
            var client = new RestClient("http://" + Paths.Instance.PolicyTokenCheckerLocation + "/bcc_policy_token_gateway/checktoken/" + policyToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            return response.StatusCode == HttpStatusCode.OK;
        }

        private string GetRefreshToken(string access_token, string walletID, string scope)
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

            // Refresh Tokens are only returned on the first request 
            // which means that we need to store them if that scope has already been granted to our service

            dynamic responseParsed = JsonConvert.DeserializeObject(response.Content);

            var refreshToken = responseParsed.refresh_token;

            string connStr = "server=" + Paths.Instance.MysqlIP + ";user=" + Paths.Instance.MysqlUsername + ";database=" + Paths.Instance.MysqlDatabase + ";port=" + Paths.Instance.MysqlPort +";password=" + Paths.Instance.MysqlUserPassword;
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                if(refreshToken == null && responseParsed.access_token != null)
                {
                    string sql = "SELECT APIAddress FROM Token WHERE WalletID=\"" +  walletID  + "\" AND Scope=\"" + scope + "\"";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    refreshToken = rdr[0];
                    rdr.Close();
                }
                else
                {
                    string sql = "INSERT INTO Token (WalletID, Scope, APIAddress) VALUES ('" + walletID + "','" + scope + "', '" + refreshToken + "')";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return refreshToken;
        }

        // Link to fetcher version later
        private static Dictionary<string, string> _googleFitDataTypeDict = new Dictionary<string, string>()
        {
            {"fitness.body.read", "1"}
        };

        private static Dictionary<string, string> _fitbitDataTypeDict = new Dictionary<string, string>()
        {
            {"heartRate", "0"}
        };


        private string ScopeToDataType(string custType, string scope)
        {
            if(custType == "1")
            {
                return _googleFitDataTypeDict[scope];
            }
            else
            {
                return _fitbitDataTypeDict[scope];
            }
        }

        private string AddPolicy(string walletID, string dataType, string apiKey, string policyStr, string policyToken, string custType)
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

            var body = "{\"json_policy\":\"" + jsonPolicy.Replace("\"", "\\\"") + "\",\"policy_creation_token\":\"" + policyToken + "\",\"wallet_id\":\"" + walletID + "\",\"cust_type\":\"" + custType + "\",\"data_type\":\"" + dataType + "\",\"api_key\":\"" + apiKey + "\"} ";
            Console.WriteLine("REQUEST STRING: " + body);

            ViewData["RequestBody"] = body;

            request.AddParameter("undefined", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            
            Console.WriteLine("Policy Gateway: " + response.Content);
            return response.Content;
        }
        
        private static string Base64Encode(string plainText) 
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData) 
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
