using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blockchain_policy_deployer.Models;
using System.Net;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

namespace blockchain_policy_deployer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class DeployController : ControllerBase
    {
        private readonly UserPolicyContext _context;

        public DeployController(UserPolicyContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "test", "get" };
        }

        /*
        [HttpPost]
        public string TestMethod(UserPolicy Model)
        {
            return "Hello from http post web api controller: " + Model.ToString();
        }
        */

        
        // POST: api/test
        [HttpPost]
        //public async Task<ActionResult<UserPolicy>> Deploy(UserPolicy model)

        public string Deploy(UserPolicy model)
        {
            //This class will attempt to build a UserPolicy model based on passed-in information.
            //String - Data Subject Wallet ID
            //JSON - Policy
            //It will check the JSON policy via the policy-validator service
            //and then output the policy in the appropriate format on the Blockchain.
     

            //Step 1: Send received JSON Policy to policy validator
            //https://stackoverflow.com/questions/27108264/c-sharp-how-to-properly-make-a-http-web-get-request/27108442

            //This is the URL of the Policy Validator
            string url = @"https://policy_validator/checkjson/jsonpolicy";
            
            //The policy validator needs variables passed via the URL, so need to extract all of the variables of the JSON
            string urlParams = "";
            JObject jsonString = model.JsonPolicy;

            IList<string> jsonKeys = jsonString.Properties().Select(p => p.Name).ToList();

            foreach (string str in jsonKeys)
            {
                if (urlParams == "") urlParams += "?";
                else urlParams += "&";
 
                urlParams += str + "=" + jsonString.GetValue(str);
            }

            url += urlParams;

            //Perform a GET request on the URL
            try
            {
                string html = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
            } catch (Exception e)
            {
                //TODO: Implement proper error code
                return "Cannot connect to JSON policy validator. Error: " + e.ToString();
            }

            //Check to see if the validator declares that the JSON policy is fine
            //TODO: Find out exactly what the response will be from Ahmad

            //Step 2: Reformat the JSON policy to the appropriate format
            //to be placed on the Blockchain

            //Step 3: Place the policy on the blockchain

            //The return below is for debugging purposes
            return ("URL + extracted parameters : " + url + " | Wallet ID: " + model.DataSubjectWalletID);
        }
        
    }
}