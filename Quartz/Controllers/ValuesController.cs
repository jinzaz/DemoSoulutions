using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzDemo.Dto;
using QuartzDemo.Service.IService;
using QuartzDemo.Util.common;

namespace QuartzDemo.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        private IPersonService _personService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="personService"></param>
        public ValuesController(ISchedulerFactory schedulerFactory,IPersonService personService)
        {
            _schedulerFactory = schedulerFactory;
            _personService = personService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string[]> Get()
        {
            //1.通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
            //2.开启调度器
            await _scheduler.Start();
            UserInfo userInfo =new UserInfo();
            //3.创建一个触发器
            var trigger = TriggerBuilder.Create()
                             //.WithCronSchedule("0 30 8 * * ?")每天上午8点半触发
                             .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())//每两秒执行一次
                             .UsingJobData("key1", 123)
                             .UsingJobData("key2", "321")
                             .WithIdentity("trigger2", "group1")
                             .Build();
            //4.创建任务
            var jobDetail = JobBuilder.Create<MyJob>()
                             .UsingJobData("key1", 123)
                             .UsingJobData("key2", "123")
                             .WithIdentity("job", "group")
                             .Build();
            //5.将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);

                FileStream file = new FileStream(@".\error.log", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} api/values ");
                }
            return await Task.FromResult(new string[] { "value1", "value2" });
        }
        [HttpGet]
        [Route("{id},{OrginPrice}")]
        public string Get(int id,decimal OrginPrice)
        {
            MyValueProcessor _myValueProcessor = new MyValueProcessor();
            return (_myValueProcessor.DaZhe((short)id, OrginPrice).ToString());
        }

        [HttpPost]
        public bool Post([FromBody]UserInfo userInfo)
        {
            if (userInfo != null)
            {
                return true;
            }
            return false;
        }

        [HttpPut]
        [Route("{id}")]
        public void Put(int id ,[FromBody]UserInfo userInfo)
        {
            string eat =  _personService.Eat();
        }
    }
}