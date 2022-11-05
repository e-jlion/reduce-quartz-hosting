using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using Reduce.Quartz.Hosting.Options;
using static Quartz.Logging.OperationName;

namespace Reduce.Quartz.Hosting.Builders
{
    /// <summary>
    /// 动态创建相同执行逻辑的Jobbuilder
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskJobBuilder<T> where T : IJob
    {
        /// <summary>
        /// 创建执行Job
        /// </summary>
        /// <param name="job">通用的Job执行</param>
        /// <param name="taskOptions">Job 通用配置</param>
        /// <returns></returns>
        public IJobDetail CreateJob(TaskOptions taskOptions)
        {
            if (!taskOptions.IsVerify())
            {
                throw new ArgumentNullException("动态创建IJobDetail 失败,参数错误[taskOptions]");
            }
            var jobName = taskOptions.Name;
            var groupName = taskOptions.Group;
            var description = taskOptions.Description;

            return JobBuilder.Create<T>()
                    .WithIdentity(jobName, groupName)
                    .WithDescription(description)
                    .Build();
        }

        /// <summary>
        /// 创建触发器
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public ITrigger CreateTrigger(TaskOptions taskOptions)
        {
            if (!taskOptions.IsVerify())
            {
                throw new ArgumentNullException("动态创建ITrigger 触发器失败,参数错误[taskOptions]");
            }
            var jobName = taskOptions.Name;
            var groupName = jobName + "_group";
            var triggerName = jobName + "_trigger";
            var triggerGroupName = groupName;
            var description = taskOptions.Description;


            return TriggerBuilder
                    .Create()
                    .WithIdentity(triggerName, triggerGroupName)
                    .WithCronSchedule(taskOptions.Cron)
                    .WithDescription(description)
                    .StartNow()
                    .Build();
        }

        /// <summary>
        /// 添加Job
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task AddJob(TaskOptions taskOptions, CancellationToken cancellationToken)
        {
            var job = CreateJob(taskOptions);
            var trigger = CreateTrigger(taskOptions);

            await SchedulerHelper.Scheduler.ScheduleJob(job, trigger, cancellationToken);
        }
    }
}
