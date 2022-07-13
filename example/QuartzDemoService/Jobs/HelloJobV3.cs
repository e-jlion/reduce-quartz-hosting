using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz.Reduce.Hosting;

namespace QuartzDemoService
{
    [DisallowConcurrentExecution]
    [CronScheduleStrategyAttribute(CronExpress = "* * * * * ? *", Description = "HelloV3")]
    public class HelloJobV3 : IJob
    {
        static int _count = 0;
        public HelloJobV3()
        {
        }


        public async Task Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine($"HelloV3执行 {++_count},次 开始");
            await Task.Delay(1000 * 60);
            System.Console.WriteLine($"HelloV3执行 {++_count},次 完成");

            //_logger.LogInformation($"执行中...");
            //throw new NotImplementedException();
        }
    }
}
