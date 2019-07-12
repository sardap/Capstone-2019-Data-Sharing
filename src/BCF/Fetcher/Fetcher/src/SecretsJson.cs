using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fetcher
{
	/// <summary>
	/// Doesn't follow naming conventions because it needs to match the json file
	/// 
	/// </summary>
	public class SecretsJson
	{
		public static SecretsJson Instance { get; set; }

		public string google_api_client_id { get; set; }

		public string google_api_client_secert { get; set; }
	}
}
