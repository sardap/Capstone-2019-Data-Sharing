using System;

namespace DataBroker.Models
{
	public sealed class Secret
	{
		private static readonly Lazy<Secret> _instance = new Lazy<Secret>(() => new Secret());

		public static Secret Instance => _instance.Value;
		public string PolicyValidatorIp { get; }
		public string PolicyValidatorPort { get; }
		public string PolicyTokenGatewayIp { get; }
		public string PolicyTokenGatewayPort { get; }
		public string PolicyAuthorizationUrl { get; }

		private Secret()
		{
			PolicyValidatorIp = Environment.GetEnvironmentVariable("VALIDATOR_IP_ADDRESS");
			PolicyValidatorPort = Environment.GetEnvironmentVariable("VALIDATOR_PORT");
			PolicyTokenGatewayIp = Environment.GetEnvironmentVariable("POLICY_GATEWAY_IP_ADDRESS");
			PolicyTokenGatewayPort = Environment.GetEnvironmentVariable("POLICY_GATEWAY_PORT");
			PolicyAuthorizationUrl = Environment.GetEnvironmentVariable("POLICY_AUTHORIZATION_URL");
		}
	}
}