using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCCDataCustodianSelection
{
	public class Paths
	{
		public static Paths Instance
		{
			get; set;
		}

		public string ValidatorIP
		{
			get; set;
		}

		public string ValidatorPort
		{
			get; set;
		}

        public string PolicyGatewayIP
        {
            get; set;
        }

		public string RedirectURI
		{
			get; set;
		}

		public string GoogleClientID
		{
			get; set;
		}

		public string GoogleSecert
		{
			get; set;
		}
	}
}