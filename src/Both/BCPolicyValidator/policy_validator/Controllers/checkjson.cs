﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Policy_Validator.Models;

namespace Policy_Validator.Controllers
{
    [Route("checkjson")]
    [ApiController]
    public class CheckJson : ControllerBase
    {
        // https://localhost:5001/checkjson/{"excluded_categories":[0],"min_price":10,"time_period":{"start":-4785955200,"end":693705600},"data_type":"heart rate","wallet_ID":"xxxxxxxxxxxxxxxxxx","active":[true, false],"report_log":[{"data":"123","hash":"321"}]}
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
                return BadRequest("Wrong JSON format. Expected JSON format:\n\n{'excluded_categories':[<int array>],'min_price':<int>,'time_period':{'start':<long>,'end':<long>},'data_type':'<string>','wallet_id':'<string>','active':[<bool array>], 'report_log':[{'data':'<string>', 'hash':'<string>'}]}");
            }
            catch(JsonSerializationException)
            {
                return BadRequest("Wrong JSON format. Expected JSON format:\n\n{'excluded_categories':[<int array>],'min_price':<int>,'time_period':{'start':<long>,'end':<long>},'wallet_id':'<string>', 'report_log':[{'data':<string>, 'hash':<string>}]'}");
            }

            ErrorList.AddRange(CheckJsonPart.JsonPartValidate(Policy));

            if(Policy.Data_type == null)
                ErrorList.Add("data_type field is not assigned.");

            if(Policy.Active.Count == 0)
                ErrorList.Add("active field is not assigned.");

            if(ErrorList.Count > 0)
                return BadRequest(ErrorList);

            //Return JSON policy.
            return Ok(Policy);
        }
    }
}
