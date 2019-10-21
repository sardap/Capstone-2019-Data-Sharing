using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Policy_Validator.Models;

namespace Policy_Validator.Controllers
{
    [Route("checkjsonpart")]
    [ApiController]
    public class CheckJsonPart : ControllerBase
    {
        // https://localhost:5001/checkjsonpart/{"excluded_categories":[0],"min_price":10,"time_period":{"start":-4785955200,"end":693705600},"wallet_ID":"xxxxxxxxxxxxxxxxxx","report_log":[{"data":"123","hash":"321"}]}
        [HttpGet("{stringPolicy}")]
        public ActionResult Validate(string stringPolicy)
        {
            var ErrorList = new List<string>();
            PolicyModel Policy;

            try
            {
                Policy = JsonConvert.DeserializeObject<PolicyModel>(stringPolicy, new StrictStringConverter());
            }
            catch(JsonReaderException)
            {
                return BadRequest("Wrong JSON format. Expected JSON format:\n\n{'excluded_categories':[<int array>],'min_price':<int>,'time_period':{'start':<long>,'end':<long>},'wallet_id':'<string>', 'report_log':[{'data':<string>, 'hash':<string>}]'}");
            }
            catch(JsonSerializationException)
            {
                return BadRequest("Wrong JSON format. Expected JSON format:\n\n{'excluded_categories':[<int array>],'min_price':<int>,'time_period':{'start':<long>,'end':<long>},'wallet_id':'<string>', 'report_log':[{'data':<string>, 'hash':<string>}]'}");
            }

            ErrorList.AddRange(JsonPartValidate(Policy));

            if(ErrorList.Count > 0)
                return BadRequest(ErrorList);

            //Return JSON policy.
            return Ok(Policy);
        }

        public static List<string> JsonPartValidate(PolicyModel policy)
        {
            var ErrorList = new List<string>();

            if(policy.Time_period.Start >= policy.Time_period.End)
                ErrorList.Add("Start time should be before end time.");

            if(policy.Min_price == null)
                ErrorList.Add("min_price field is not assigned.");

            if(policy.Time_period.Start == null)
                ErrorList.Add("start field is not assigned.");

            if(policy.Time_period.End == null)
                ErrorList.Add("end field is not assigned.");

            if(policy.Wallet_ID == null)
                ErrorList.Add("wallet_ID field is not assigned.");

            return ErrorList;
        }
    }
}
