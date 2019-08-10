using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BCCDataCustodianSelection.Models
{
    public class DataCustodianModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string DataCustodian { get; set; }
            [Required]
            public string DataType { get; set; }
            [Required]
            public string APIKey { get; set; }
        }
    }
}