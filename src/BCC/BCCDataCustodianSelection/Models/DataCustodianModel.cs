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
            public string Wallet_ID { get; set; }
            public string DataCustodian { get; set; }
            public string DataType { get; set; }
            public string APIKey { get; set; }
        }
    }
}