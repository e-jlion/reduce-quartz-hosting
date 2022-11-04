using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reduce.Quartz.Hosting
{
    /// <summary>
    /// 静态的Scheduled 
    /// </summary>
    public static class RegisterScheduler
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



    }
}
