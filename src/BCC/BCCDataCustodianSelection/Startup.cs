﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCCDataCustodianSelection.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BCCDataCustodianSelection
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var paths = new Paths
            {
                ValidatorIP = Environment.GetEnvironmentVariable("ValidatorIP"),
                ValidatorPort = Environment.GetEnvironmentVariable("ValidatorPort"),
                PolicyGatewayIP = Environment.GetEnvironmentVariable("PolicyGatewayIP"),
                RedirectURI = Environment.GetEnvironmentVariable("REDIRECT_URI"),
                GoogleClientID = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"),
                GoogleSecert = Environment.GetEnvironmentVariable("GOOGLE_API_CLIENT_SECRET"),
                PolicyTokenCheckerLocation = Environment.GetEnvironmentVariable("POLICY_TOKEN_CHECKER"),
                MysqlUsername = Environment.GetEnvironmentVariable("MYSQL_USERNAME"),
                MysqlUserPassword = Environment.GetEnvironmentVariable("MYSQL_USER_PASSWORD"),
                MysqlPort = Environment.GetEnvironmentVariable("MYSQL_PORT"),
                MysqlIP = Environment.GetEnvironmentVariable("MYSQL_IP"),
                MysqlDatabase = Environment.GetEnvironmentVariable("MYSQL_DATABASE")
            };

            Paths.Instance = paths;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<PolicyCreationContex>(opt =>
               opt.UseInMemoryDatabase("PolicyCreation"));


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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            app.UseCookiePolicy();
        }
    }
}
