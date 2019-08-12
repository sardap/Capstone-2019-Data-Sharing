using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fetcher
{
	public class Secrets
	{
		public static Secrets Instance { get; set; }

		public string GoogleAPIClientID { get; set; }

		public string GoogleAPIClientSecret { get; set; }
	}
}
