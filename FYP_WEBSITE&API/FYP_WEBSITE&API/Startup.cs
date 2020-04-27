﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using FYP_3DPrinterMonitor.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FYP_3DPrinterMonitor
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //Adds the settings files into the program to be used later.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Add(new ServiceDescriptor(typeof(ConnectedDevicesContext), new ConnectedDevicesContext(Configuration.GetConnectionString("DefaultConnection"))));
    
            services.AddMvc().AddControllersAsServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //If statement that varies the error responses depending on mode.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            // Tells the program that files are available to be served.
            app.UseFileServer();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
