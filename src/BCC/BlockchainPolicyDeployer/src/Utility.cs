using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainPolicyDeployer
{
	public static class Utility
	{
		private static Random _rand = new Random();

		// https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[_rand.Next(s.Length)]).ToArray());
		}

	}
}
