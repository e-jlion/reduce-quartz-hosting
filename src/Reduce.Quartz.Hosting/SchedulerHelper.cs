using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Reduce.Quartz.Hosting.Enums;
using Reduce.Quartz.Hosting.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reduce.Quartz.Hosting
{
    /// <summary>
    /// 静态的Scheduled 
    /// </summary>
    public static class SchedulerHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static IScheduler Scheduler { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        public static void New(IScheduler scheduler)
        {
            Scheduler = scheduler;
        }

        public static void Deleted()
        {

        }

        /// <summary>
        /// 根据分组名称和Job名称查找Job触发器
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="name">Job名称</param>
        /// <returns></returns>
        public static async Task<(ITrigger,JobKey)> FindTriggerAsync(string groupName, string name)
        {
            var jobKeys = Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)).Result.ToList();
            if (jobKeys == null || jobKeys.Count() == 0)
            {
                return (null,null);//未找到分组
            }
            var jobKey = jobKeys.Where(s => Scheduler.GetTriggersOfJob(s).Result.Any(x => (x as CronTriggerImpl).Name == name)).FirstOrDefault();
            if (jobKey == null)
            {
                return (null,null);//未找到触发器
            }
            var triggers = await Scheduler.GetTriggersOfJob(jobKey);
            var trigger = triggers?.Where(x => (x as CronTriggerImpl).Name == name).FirstOrDefault();

            if (trigger == null)
            {
                return (null,null);//未找到触发器
            }
            return (trigger, jobKey);
        }

        /// <summary>
        /// 删除当前的Job
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteAsync(string groupName, string name)
        {
            var (trigger,jobkey) = await FindTriggerAsync(groupName, name);
            if (trigger == null)
            {
                return false;
            }
            await Scheduler.PauseTrigger(trigger.Key);
            await Scheduler.UnscheduleJob(trigger.Key);// 移除触发器
            return await Scheduler.DeleteJob(trigger.JobKey);
        }

        public static async Task<bool> TriggerAsync(string name, string groupName, EnumTaskJobAction taskJobAction)
        {
            switch (taskJobAction)
            {
                case EnumTaskJobAction.JOB_REMOVE:
                    return await DeleteAsync(groupName, name);
                case EnumTaskJobAction.JOB_UPDATE:
                    //TODO
                    throw new ArgumentNullException("暂时不支持,带实现");
                case EnumTaskJobAction.JOB_PAUSE:
                    return await PauseAsync(groupName, name);
                case EnumTaskJobAction.JOB_EXECUTE:
                    return await ExecuteTiggerAsync(groupName, name);
                case EnumTaskJobAction.JOB_START:
                    return await ResumeAsync(groupName, name);
                case EnumTaskJobAction.JOB_STOP:
                    await Scheduler.Shutdown();
                    return true;
                default:
                    throw new ArgumentNullException("不支持的类型");
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async Task<bool> PauseAsync(string groupName, string name)
        {
            var (trigger, jobkey) = await FindTriggerAsync(groupName, name);
            if (trigger == null)
            {
                return false;
            }
            await Scheduler.PauseTrigger(trigger.Key);
            return true;
        }

        public static async Task<bool> ResumeAsync(string groupName, string name)
        {
            var (trigger, jobkey) = await FindTriggerAsync(groupName, name);
            if (trigger == null)
            {
                return false;
            }
            await Scheduler.ResumeTrigger(trigger.Key);
            return true;
        }

        public static async Task<bool> ExecuteTiggerAsync(string groupName, string name)
        {
            var (trigger, jobkey) = await FindTriggerAsync(groupName, name);
            if (trigger == null)
            {
                return false;
            }
            await Scheduler.TriggerJob(jobkey);
            return true;
        }
    }
}
