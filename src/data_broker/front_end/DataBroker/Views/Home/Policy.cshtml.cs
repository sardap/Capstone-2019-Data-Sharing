using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBroker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataBroker.Views.Home
{
    public class PolicyModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public IList<DataSharingPolicy> DataSharingPolicies = new List<DataSharingPolicy>();
        }

        public void OnGet()
        {
            Input = new InputModel();
        }
    }
}