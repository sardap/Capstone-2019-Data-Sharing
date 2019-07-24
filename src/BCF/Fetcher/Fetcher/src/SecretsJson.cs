using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fetcher
{
	public class Secrets
	{
		public static Secrets Instance { get; set; }

		public string Google_api_client_id { get; set; }

		public string Google_api_client_secret { get; set; }
	}
}
