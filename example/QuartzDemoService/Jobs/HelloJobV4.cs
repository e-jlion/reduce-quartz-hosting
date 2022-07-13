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
    [CustomerScheduleStrategyAttribute(CronExpress = "* * * * * ? *", Description = "HelloV4")]
    public class HelloJobV4 : IJob
    {
        static int _count = 0;
        public HelloJobV4()
        {
        }


        public async Task Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine($"HelloV4执行 {++_count},次 开始");
            await Task.Delay(1000 * 60);
            System.Console.WriteLine($"HelloV4执行 {++_count},次 完成");

        }
    }
}
