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

        public string PolicyGatewayPort
        {
            get; set;
        }

		public string StreamName
		{
			get; set;
		}

		public string ChainName
		{
			get; set;
		}

		public string RPCPort
		{
			get; set;
		}

		public string RPCIP
		{
			get; set;
		}

		public string RPCUserName
		{
			get; set;
		}

		public string RPCPassword
		{
			get; set;
		}
	}
}