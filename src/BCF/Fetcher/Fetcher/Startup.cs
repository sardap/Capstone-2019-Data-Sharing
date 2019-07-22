using System;
using System.Collections.Generic;
using System.IO;
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

namespace Fetcher
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			Secrets.Instance = new Secrets();
			Secrets.Instance.Google_api_client_id = Environment.GetEnvironmentVariable("GOOGLE_API_CLIENT_ID");
			Secrets.Instance.Google_api_client_secert = Environment.GetEnvironmentVariable("GOOGLE_API_CLIENT_SECERT");
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			// Only added when running in Development
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// Only added when running in Production
			if (env.IsProduction())
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
