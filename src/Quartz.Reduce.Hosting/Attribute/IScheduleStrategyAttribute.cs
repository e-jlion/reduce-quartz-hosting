using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Quartz.Reduce.Hosting
{
    /// <summary>
    /// Job 执行Schedule 策略 特效接口,可以继承该策略接口实现自己的创建IJobDetail 和Trigger 的方法，会自动注入和启动Job
    /// </summary>
    public interface IScheduleStrategyAttribute
    {
        /// <summary>
        /// 创建Job
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        IJobDetail CreateJob(ScheduledJob schedule);

        /// <summary>
        /// 创建Trigger
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        ITrigger CreateTrigger(ScheduledJob schedule);
    }
}
