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
            //��ӿ�������
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
                options.SwaggerDoc("QuartzDemo1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "�������ݷ���", Version = "QuartzDemo1" });
                var BasePath = PlatformServices.Default.Application.ApplicationBasePath;/* System.AppDomain.CurrentDomain.BaseDirectory;*/
                var xmlPath = Path.Combine(BasePath, "Qka.BasicDataApi.xml");
                options.IncludeXmlComments(xmlPath);
            });
            //consul�������
            //services.AddHealthChecks();
            //services.AddConsul();
            //���������Ϣ��services
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            //��services�л�ȡ������Ϣ
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            //���JWT��Ȩ
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
            //���Swagger����
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
            //���ý�������ַ��.NET Core���õĽ�������ַ�м��
            //app.UseHealthChecks(serviceOptions.Value.HealthCheck);
            //app.UseConsul();
            app.UseHttpsRedirection();
            app.UseRouting();
            //��֤
            app.UseAuthentication();
            //����
            app.UseCors(MyAllowSpecificOrigins);
            //��Ȩ
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
        //IOC����ע��
        public void ConfigureContainer(ContainerBuilder builder)
        {
            ////ʵ����Autofac����
            //var builder = new ContainerBuilder();
            ////��Services�еķ�����䵽Autofac��
            //builder.Populate(services);
            ////��ģ�����ע��
            //builder.RegisterModule<AutofacModuleRegister>();
            ////��������
            //var Container = builder.Build();
            ////������IOC�ӹ� core����DI����
            //return new AutofacServiceProvider(Container);
            builder.RegisterType<PersonService>().As<IPersonService>();//ע��
            builder.RegisterType<PersonRepository>().As<IPersonRepository>();
            builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>();
            builder.RegisterType<TokenAuthenticationService>().As<ITokenAuthenticateService>();
            builder.RegisterType<UserReqService>().As<IUserReqService>();
        }

    }
}
