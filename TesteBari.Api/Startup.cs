using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TesteBari.Core.App.Extensoes;
using TesteBari.Core.App.Interface;

namespace TesteBari.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)  
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();

            services.InicializarServicos(Configuration);

            services.AddHangfire(x => x.UseMemoryStorage());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IControleServicos controleServicos)
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

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerName = $"TesteBari_{Environment.MachineName}"
            });
            app.UseHangfireDashboard();

            controleServicos.Iniciar();

            app.UseHttpsRedirection();
        }
    }
}
