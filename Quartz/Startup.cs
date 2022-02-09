using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz.Impl;
using QuartzDemo.Repository;
using QuartzDemo.Repository.IRepository;
using QuartzDemo.Service;
using QuartzDemo.Service.IService;
using QuartzDemo.Util;
using QuartzDemo.Util.autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuartzDemo.Util.jwt;
using QuartzDemo.Interface;
using QuartzDemo.Util.common;
using QuartzDemo.consul;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using QuartzDemo.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Logging;

namespace Quartz
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加跨域配置
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, p => p.AllowAnyOrigin()
                                               //.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
                                               .AllowAnyMethod()
                                               .AllowAnyHeader());
            });
            services.AddControllers().AddJsonOptions(option => {
                option.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                option.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
                option.JsonSerializerOptions.Converters.Add(new DateTimeNullableConvert());
            });
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ApiLoginCon")));
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("QuartzDemo1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "基础数据服务", Version = "QuartzDemo1" });
                var BasePath = PlatformServices.Default.Application.ApplicationBasePath;/* System.AppDomain.CurrentDomain.BaseDirectory;*/
                var xmlPath = Path.Combine(BasePath, "Qka.BasicDataApi.xml");
                options.IncludeXmlComments(xmlPath);
            });
            //consul健康检查
            //services.AddHealthChecks();
            //services.AddConsul();
            //添加配置信息到services
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            //从services中获取配置信息
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            //添加JWT授权
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                    
                };
            });
            //添加Swagger配置
            //services.AddSwaggerDocument(config => {
            //    config.PostProcess = document =>
            //    {
            //        document.Info.Version = "v1";
            //        document.Info.Title = "ToDo API";
            //        document.Info.Description = "A simple ASP.NET Core web API";
            //        document.Info.TermsOfService = "None";
            //        document.Info.Contact = new NSwag.OpenApiContact
            //        {
            //            Name = "Shayne Boyer",
            //            Email = string.Empty,
            //            Url = "https://twitter.com/spboyer"
            //        };
            //        document.Info.License = new NSwag.OpenApiLicense
            //        {
            //            Name = "Use under",
            //            Url = "https://example.com/license"
            //        };
            //    };
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IOptions<ConsulServiceOptions> serviceOptions)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //配置健康检测地址吗，.NET Core内置的健康检测地址中间件
            //app.UseHealthChecks(serviceOptions.Value.HealthCheck);
            //app.UseConsul();
            app.UseHttpsRedirection();
            app.UseRouting();
            //认证
            app.UseAuthentication();
            //跨域
            app.UseCors(MyAllowSpecificOrigins);
            //授权
            app.UseAuthorization();
            //app.UseOpenApi();
            //app.UseSwaggerUi3();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            //c =>
            //{
            //    c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            //}
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/QuartzDemo1/swagger.json", "QuartzDemo1");
            });

        }
        //IOC依赖注册
        public void ConfigureContainer(ContainerBuilder builder)
        {
            ////实例化Autofac容器
            //var builder = new ContainerBuilder();
            ////将Services中的服务填充到Autofac中
            //builder.Populate(services);
            ////新模块组件注册
            //builder.RegisterModule<AutofacModuleRegister>();
            ////创建容器
            //var Container = builder.Build();
            ////第三方IOC接管 core内置DI容器
            //return new AutofacServiceProvider(Container);
            builder.RegisterType<PersonService>().As<IPersonService>();//注册
            builder.RegisterType<PersonRepository>().As<IPersonRepository>();
            builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>();
            builder.RegisterType<TokenAuthenticationService>().As<ITokenAuthenticateService>();
            builder.RegisterType<UserReqService>().As<IUserReqService>();
        }

    }
}
