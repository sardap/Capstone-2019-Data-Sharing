using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Net.Http;

namespace BlockchainPolicyDeployer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			var paths = new Paths
			{
				VaildatorIP = Environment.GetEnvironmentVariable("VALIDATOR_IP"),
				VaildatorPort = Environment.GetEnvironmentVariable("VALIDATOR_PORT") ?? null,
				StreamName = Environment.GetEnvironmentVariable("STREAM_NAME"),
				ChainName = Environment.GetEnvironmentVariable("CHAIN_NAME"),
				RPCPort = Environment.GetEnvironmentVariable("RPC_PORT"),
				RPCIP = Environment.GetEnvironmentVariable("RPC_IP"),
				RPCUserName = Environment.GetEnvironmentVariable("RPC_USERNAME"),
				RPCPassword = Environment.GetEnvironmentVariable("RPC_PASSWORD")
			};

			Paths.Instance = paths;

			var client = new RestClient("http://" + Paths.Instance.RPCIP + ":" + Paths.Instance.RPCPort)
			{
				Authenticator = new HttpBasicAuthenticator(Paths.Instance.RPCUserName, Paths.Instance.RPCPassword)
			};
			var request = new RestRequest(Method.POST);
			request.AddHeader("Connection", "keep-alive");
			request.AddHeader("Accept-Encoding", "gzip, deflate");
			request.AddHeader("Accept", "*/*");
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("undefined", "{\"method\":\"listpermissions\",\"params\":[\"admin\"],\"chain_name\":\"chain1\"}", ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);

			dynamic responseContent = JsonConvert.DeserializeObject(response.Content);

			Paths.Instance.AdminAddress = responseContent.result[0].address;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
