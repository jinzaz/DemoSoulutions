using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo.Dto
{
    /// <summary>
    /// 任务实现类
    /// </summary>
    [PersistJobDataAfterExecution]//更新JobDetail的JobDataMap的储存副本，以便下一次执行这个任务接受更新的值二不是储存的值
    public class MyJob :IJob
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            var jobData = context.JobDetail.JobDataMap;//获取Job中的参数
            var triggerData = context.Trigger.JobDataMap;//获取Trigger中的参数
            var data = context.MergedJobDataMap;//获取Job和Trigger中合并的参数
            var value1 = jobData.GetInt("key1");
            var value2 = jobData.GetString("key2");
            var value3 = data.GetString("key2");
            var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Random random = new Random();
            jobData["key1"] = random.Next(1,20);
            jobData["key2"] = dateString;
            return Task.Run( () => {
                FileStream file = new FileStream(@"E:\.NET\DemoSolution\Quartz\bin\Debug\netcoreapp3.1\error.log", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine($"{dateString} value1:{value1} value2: {value2} ");
                }
            });
        }
    }
}
