using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fetcher
{
	interface IFetcher
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="apiKey"></param>
		/// <param name="dataType"></param>
		/// <param name="errors"></param>
		/// <returns>If it could fetch the target data type</returns>
		bool TestFetch(string apiKey, DataType dataType, List<string> errors);
	}
}
