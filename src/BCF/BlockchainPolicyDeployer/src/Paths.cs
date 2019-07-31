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
			get;
			set;
		}

		public string VaildatorIP
		{
			get;
			set;
		}

		public string VaildatorPort
		{
			get;
			set;
		}
	}
}
