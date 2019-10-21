using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fetcher
{
	public class FakeFetcher : IFetcher
	{
		public bool TestFetch(string apiKey, DataType dataType, List<string> errors)
		{
			return false;
		}
	}
}
