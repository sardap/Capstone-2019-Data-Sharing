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

namespace BlockchainPolicyDeployer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			/*
			if(Environment.GetEnvironmentVariable("HOME") != "HOME=/root")
			{
				//For testing outside docker
				Environment.SetEnvironmentVariable("VALIDATOR_IP", "127.0.0.1");
				Environment.SetEnvironmentVariable("VALIDATOR_PORT", "5005");
				Environment.SetEnvironmentVariable("STREAM_NAME", "stream1");
				Environment.SetEnvironmentVariable("CHAIN_NAME", "chain1");
				Environment.SetEnvironmentVariable("RPC_PORT", "25565");
				Environment.SetEnvironmentVariable("RPC_IP", "localhost");
				Environment.SetEnvironmentVariable("RPC_USERNAME", "multichainrpc");
				Environment.SetEnvironmentVariable("RPC_PASSWORD", "");
			}
			*/

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
