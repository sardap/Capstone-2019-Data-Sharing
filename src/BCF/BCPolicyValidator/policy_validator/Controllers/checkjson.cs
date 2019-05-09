using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using policy_validator.Models;

namespace policy_validator.Controllers
{
    [Route("policy_validator/checkjson/jsonpolicy")]
    [ApiController]
    public class CheckJson : ControllerBase
    {
        // https://localhost:5001/policy_validator/checkjson/jsonpolicy/{"excluded_categories":[0],"min_price":10,"time_period":{"start":-4785955200,"end":693705600},"data_type":"heart rate","wallet_ID":"xxxxxxxxxxxxxxxxxx","active":true}
        [HttpGet("{stringPolicy}")]
        public ActionResult Validate(string stringPolicy)
        {
            var errorList = new List<string>();
            policyModel Policy;

            try
            {
                Policy = JsonConvert.DeserializeObject<policyModel>(stringPolicy);
            }
            catch(JsonReaderException)
            {
                return BadRequest("Wrong JSON format. Expected JSON format:\n\n{'excluded_categories':[<int array>],'min_price':<int>,'time_period':{'start':<long>,'end':<long>},'data_type':'<string>','wallet_id':'<string>','active':<bool>}");
            }

            if(Policy.excluded_categories == null || Policy.excluded_categories.Length == 0 || Policy.min_price == null || Policy.time_period.start == null || Policy.time_period.end == null || Policy.data_type == null || Policy.wallet_id == null || Policy.active == null)
                errorList.Add("Not all required fields are assigned.");
            if(Policy.time_period.start >= Policy.time_period.end)
                errorList.Add("Start time should be before end time.");
            if(errorList.Count > 0)
                return BadRequest(errorList);

            //Return JSON policy
            return Ok(Policy);
        }
    }
}
