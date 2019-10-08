using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fetcher
{
	public class TestFetchResult
	{
		public List<string> Errors { get; set; }

		/// <summary>
		/// Json result of data leave in the purest form not sure what it should really look like until data fetcher 
		/// </summary>
		public bool? Result { get; set; }
	}
}
