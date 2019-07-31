using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blockchain_policy_deployer.Models
{
    public class UserPolicy
    {
        public JObject JsonPolicy { get; set; }
        public string DataSubjectWalletID { get; set; }
    }
}
