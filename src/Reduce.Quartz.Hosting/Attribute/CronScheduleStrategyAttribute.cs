using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reduce.Quartz.Hosting
{
    /// <summary>
    /// 以 服务器当前时间的 Cron表达式 执行规则策略特性 (WithCronSchedule(Cron))
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CronScheduleStrategyAttribute : Attribute, IScheduleStrategyAttribute
    {
        /// <summary>
        /// Cron 执行表达式
        /// </summary>
        public string CronExpress { set; get; }

        /// <summary>
        /// 执行Job的名称 （默认Job实例的FullName）
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 执行Job的描述
        /// </summary>
        public string Description { set; get; }


        public IJobDetail CreateJob(ScheduledJob schedule)
        {

            var jobName = string.IsNullOrEmpty(Name) ? schedule.Type.FullName : Name;
            var groupName = jobName + "_group";

            return JobBuilder
                    .Create(schedule.Type)
                    .WithIdentity(jobName, groupName)
                    .WithDescription(Description)
                    .Build();
        }


        public ITrigger CreateTrigger(ScheduledJob schedule)
        {
            var jobName = string.IsNullOrEmpty(Name) ? schedule.Type.FullName : Name;
            var groupName = jobName + "_group";
            var triggerName = jobName + "_trigger";
            var triggerGroupName = groupName;


            return TriggerBuilder
                    .Create()
                    .WithIdentity(triggerName, triggerGroupName)
                    .WithCronSchedule(CronExpress)
                    .WithDescription(Description ?? schedule.Type.FullName)
                    .StartNow()
                    .Build();
        }

    }
}
