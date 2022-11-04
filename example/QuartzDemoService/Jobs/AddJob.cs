using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reduce.Quartz.Hosting;
using Reduce.Quartz.Hosting.Builders;
using Reduce.Quartz.Hosting.Options;
using Microsoft.Extensions.DependencyInjection;

namespace QuartzDemoService
{
    [DisallowConcurrentExecution]
    [CustomerScheduleStrategyAttribute(CronExpress = "* * * * * ? *", Description = "HelloV4")]
    public class AddJob : IJob
    {
        int _count = 0;
        TaskJobBuilder<TemplateJob> _taskJobBuilder;
        IServiceProvider serviceProvider;
        public AddJob(TaskJobBuilder<TemplateJob> taskJobBuilder)
        {
            _taskJobBuilder = taskJobBuilder;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            _taskJobBuilder = serviceProvider.GetRequiredService<TaskJobBuilder<TemplateJob>>();
            if (_count > 0)
            {
                return;

            }

            //动态注入创建Job ,这里可以通过界面管理方式，通过api方式进行动态创建Job ，并持久化数据库中
            await _taskJobBuilder.AddJob(new TaskOptions()
            {
                Cron = "* * * * * ? *",

                Description = "test01",
                Group = "testgroup01",
                Name = "test01"
            }, context.CancellationToken);

            await _taskJobBuilder.AddJob(new TaskOptions()
            {
                Cron = "0/4 * * * * ? *",
                Description = "test02",
                Group = "testgroup02",
                Name = "test02"
            }, context.CancellationToken);


        }
    }
}
