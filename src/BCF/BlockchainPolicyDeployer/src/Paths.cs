using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainPolicyDeployer
{
	public class Paths
	{
		public static Paths Instance
		{
			get; set;
		}

		public string VaildatorIP
		{
			get; set;
		}

		public string VaildatorPort
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
