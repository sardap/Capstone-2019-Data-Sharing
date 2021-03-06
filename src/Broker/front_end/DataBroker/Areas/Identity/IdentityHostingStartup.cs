﻿using System;
using DataBroker.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(DataBroker.Areas.Identity.IdentityHostingStartup))]
namespace DataBroker.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                var isProd = context.HostingEnvironment.IsProduction();

                // Disable all the requirements if it's in Dev mode
                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = isProd;
                    options.Password.RequireNonAlphanumeric = isProd;
                    options.Password.RequireUppercase = isProd;
                    options.Password.RequiredLength = isProd ? 6 : 1;
                    options.Password.RequiredUniqueChars = 1;
                });
            });
        }
    }
}