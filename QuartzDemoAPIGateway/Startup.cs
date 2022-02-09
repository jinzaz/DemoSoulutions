using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace QuartzDemo
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
            var config = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();
            services.AddOcelot(config)
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                .AddConsul().AddPolly();
            services.AddControllers();
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("ApiGateway",new Microsoft.OpenApi.Models.OpenApiInfo { Title="Íø¹Ø·þÎñ",Version="v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var apis = new List<string> { "QuartzDemo1", "QuartzDemo2" };
            //app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                apis.ForEach(m => {
                    options.SwaggerEndpoint($"/swagger/{m}/swagger.json",m);
                });
            });
            app.UseOcelot().Wait();

        }
    }
}
