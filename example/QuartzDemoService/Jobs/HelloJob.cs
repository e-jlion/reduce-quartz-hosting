using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reduce.Quartz.Hosting;

namespace QuartzDemoService
{
    [DisallowConcurrentExecution]
    [UtcTimeCronScheduleAttribute(CronExpress = "* * * * * ? *", Description = "HelloV1")]
    public class HelloJobV2 : IJob
    {
        static int _count = 0;
        public Test2 _test;
        public HelloJobV2(Test2 test2)
        {
            _test = test2;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine($"HelloV2执行 {++_count},次 开始");
            await Task.Delay(1000 * 60);
            System.Console.WriteLine($"HelloV2执行 {++_count},次 完成");

            //_logger.LogInformation($"执行中...");
            //throw new NotImplementedException();
        }
    }
}
