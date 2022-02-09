using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace QuartzDemo.consul
{
    //consul服务注册扩展类
    public static class ConsulRegistrationExtensions
    {
        static string host = "172.0.0.1";
        static int port = 8501;
        public static void AddConsul(this IServiceCollection service)
        {
            //读取服务配置文件
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder().AddJsonFile("consulconfig.json").Build();
            service.Configure<ConsulServiceOptions>(config);
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            //获取本机生命周期管理接口
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            //获取服务配置项
            var serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ConsulServiceOptions>>().Value;
            //服务ID必须保证唯一
            serviceOptions.ServiceId = Guid.NewGuid().ToString();

            var consulClient = new ConsulClient(configuration => {
                //服务注册的地址，集群中任意一个地址
                configuration.Address = new Uri(serviceOptions.ConsulAddress);
            });

            //过去当前服务地址和端口，配置方式
            var uri = new Uri(serviceOptions.ServiceAddress);

            //节点服务注册对象
            var registration = new AgentServiceRegistration()
            {
                ID = serviceOptions.ServiceId,
                Name = serviceOptions.ServiceName,
                Address = uri.Host,
                Port = uri.Port,
                Check = new AgentServiceCheck {
                    //注册超时
                    Timeout = TimeSpan.FromSeconds(5),
                    //服务停止多久后注销服务
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5),
                    //健康检查地址
                    HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}{serviceOptions.HealthCheck}",
                    //健康检查时间间隔
                    Interval = TimeSpan.FromSeconds(10)
                }

            };

            //注册服务
            consulClient.Agent.ServiceRegister(registration).Wait();

            //string[] idlist =  {
            //    "50da20bd-d043-4902-9829-4c376f97979b",
            //    "81714ae1-aecb-4ea9-985a-aad51e36e4b1",
            //    "a0c1f024-8b96-4d27-9151-070dc728415c",
            //    "e8ee3ae1-3e2e-40a4-bdde-383cdb83f840"
            //};
            //for (int i = 0; i < idlist.Length; i++)
            //{
            //    consulClient.Agent.ServiceDeregister(idlist[i]).Wait();
            //}
            //应用程序终止时，注销服务
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceOptions.ServiceId).Wait();
            });

            return app;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetServices()
        {
            string[] list = { };
            string Url = "http://" + host + ":" + port.ToString() + "/v1/agent/services";
            var data = new HttpClient().GetAsync(Url).Result;
            return list;
        }
    }
}
