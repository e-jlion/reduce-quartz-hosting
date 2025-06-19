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
    /// <summary>
    /// 动态统一执行Job的模板，比如实现调度器和执行器分离的逻辑，这
    /// 里可以做为请求Http或者生产消息到消费者中的统一模板
    /// </summary>
    [DisallowConcurrentExecution]
    public class TemplateJob : IJob
    {
        int _count = 0;
        public TemplateJob()
        {
        }


        public async Task Execute(IJobExecutionContext context)
        {

            System.Console.WriteLine($"HelloV4执行 {++_count},次 开始 name:{context?.JobDetail?.Description},time:{DateTime.Now}");
            //await Task.Delay(1000);
            //System.Console.WriteLine($"HelloV4执行 {++_count},次 完成");

        }
    }
}
